' FurryArtStudio - 本地稿件管理工具
' Copyright 2026 xionglongztz/PawLaboratory
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
'     http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar
Imports Dapper
''' <summary>
''' 稿件库实例
''' </summary>
Public Class ArtworkLibrary
    ''' <summary>
    ''' 当前稿件库名称
    ''' </summary>
    Public Property LibraryName As String
    ''' <summary>
    ''' 当前稿件库路径
    ''' </summary>
    Public Property LibraryPath As String
    ''' <summary>
    ''' 当前数据库连接参数字符串
    ''' </summary>
    Public Property ConnectionString As String
    ''' <summary>
    ''' 当前是否已初始化
    ''' </summary>
    Public Property IsLoaded As Boolean = False
    ''' <summary>
    ''' 当前数据库连接
    ''' </summary>
    Public Property DbConnection As SQLiteConnection

    ''' <summary>
    ''' 初始化稿件库实例
    ''' </summary>
    Public Sub New(name As String)
        LibraryName = name
        Dim artworksPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks")
        If Not Directory.Exists(artworksPath) Then
            Directory.CreateDirectory(artworksPath) '检查 Artworks 文件夹是否存在
        End If
        LibraryPath = Path.Combine(artworksPath, LibraryName)
        If Not Directory.Exists(LibraryPath) Then
            Directory.CreateDirectory(LibraryPath) '如果目录不存在, 则创建新的稿件库
        End If
        Dim dbPath As String = Path.Combine(LibraryPath, "Metadata.db")
        Dim builder As New SQLiteConnectionStringBuilder With {
            .DataSource = dbPath,
            .ForeignKeys = True, '启用外键约束
            .Pooling = False, '禁用连接池
            .DefaultTimeout = 30 '默认超时30秒
            }
        ConnectionString = builder.ConnectionString
        DbConnection = New SQLiteConnection(ConnectionString) '创建新连接
        DbConnection.Open()
        Using conn As New SQLiteConnection(_ConnectionString)
            conn.Open()

            Dim sql As String = "
            -- 主表
            CREATE TABLE IF NOT EXISTS Artworks (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                UUID TEXT UNIQUE NOT NULL,
                Title TEXT NOT NULL,
                Author TEXT,
                Characters TEXT,
                CreateTime INTEGER DEFAULT 0,
                ImportTime INTEGER DEFAULT (strftime('%s', 'now')),
                UpdateTime INTEGER DEFAULT (strftime('%s', 'now')),
                IsDeleted INTEGER DEFAULT 0,
                Tags TEXT,           -- JSON数组存储
                Notes TEXT
            );

            -- 索引
            CREATE INDEX IF NOT EXISTS idx_uuid ON Artworks(UUID);
            CREATE INDEX IF NOT EXISTS idx_isdeleted ON Artworks(IsDeleted); 
            CREATE INDEX IF NOT EXISTS idx_import_time ON Artworks(ImportTime);
            CREATE INDEX IF NOT EXISTS idx_author ON Artworks(Author);

            -- 标签搜索表
            CREATE TABLE IF NOT EXISTS Tags (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                TagName TEXT UNIQUE NOT NULL,
                UsageCount INTEGER DEFAULT 0
            );

            CREATE INDEX IF NOT EXISTS idx_tag_name ON Tags(TagName);

            -- 标签关联表
            CREATE TABLE IF NOT EXISTS ArtworkTags (
                ArtworkID INTEGER,
                TagID INTEGER,
                FOREIGN KEY (ArtworkID) REFERENCES Artworks(ID) ON DELETE CASCADE,
                FOREIGN KEY (TagID) REFERENCES Tags(ID) ON DELETE CASCADE,
                PRIMARY KEY (ArtworkID, TagID)
            );

            -- 缩略图表
            CREATE TABLE IF NOT EXISTS Thumbnails (
                ID          INTEGER PRIMARY KEY AUTOINCREMENT,
                ArtworkID   INTEGER NOT NULL,
                ImageData   BLOB NOT NULL,
                FOREIGN KEY (ArtworkID) REFERENCES Artworks(ID) ON DELETE CASCADE
            );

            CREATE INDEX IF NOT EXISTS idx_thumbnail_artwork ON Thumbnails(ArtworkID);

            -- 全文搜索表
            CREATE VIRTUAL TABLE IF NOT EXISTS ArtworksFTS USING fts4(
                Title, 
                Author, 
                Notes,
                content='Artworks'
            );

            -- FTS4 触发器
            CREATE TRIGGER IF NOT EXISTS artworks_ai AFTER INSERT ON Artworks BEGIN
                INSERT INTO ArtworksFTS(docid, Title, Author, Notes)
                VALUES (new.ID, new.Title, new.author, new.Notes);
            END;

            CREATE TRIGGER IF NOT EXISTS artworks_ad AFTER DELETE ON Artworks BEGIN
                DELETE FROM ArtworksFTS WHERE docid = old.ID;
            END;

            CREATE TRIGGER IF NOT EXISTS artworks_au AFTER UPDATE ON Artworks BEGIN
                DELETE FROM ArtworksFTS WHERE docid = old.ID;
                INSERT INTO ArtworksFTS(docid, Title, Author, Notes)
                VALUES (new.ID, new.Title, new.Author, new.Notes);
            END;"

            Using cmd As New SQLiteCommand(sql, conn)
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As SqlException
                    Throw
                End Try
            End Using
            IsLoaded = True
        End Using
    End Sub

    ''' <summary>
    ''' 添加新的稿件
    ''' </summary>
    ''' <exception cref="SqlException"></exception>
    Public Sub AddArtwork(artwork As Artwork)
        Using conn As New SQLiteConnection(_ConnectionString)
            conn.Open()
            Using trans = conn.BeginTransaction()
                If artwork.UUID = Guid.Empty Then artwork.UUID = Guid.NewGuid() '确保UUID唯一
                Directory.CreateDirectory(Path.Combine(LibraryPath, artwork.UUID.ToString))
                '设置时间
                artwork.ImportTime = DateTime.Now
                artwork.UpdateTime = DateTime.Now
                If artwork.CreateTime = DateTime.MinValue Then artwork.CreateTime = DateTime.Parse("1970-01-01 00:00:00")
                Dim sql As String = "
                    INSERT INTO Artworks 
                    (UUID, Title, Author, CreateTime, ImportTime, UpdateTime, IsDeleted, Tags, Notes, Characters)
                    VALUES 
                    (@UUID, @Title, @Author, @CreateTime, @ImportTime, @UpdateTime, @IsDeleted, @Tags, @Notes, @Characters);
                    SELECT last_insert_rowid();"
                Try
                    '使用Dapper执行
                    Dim newId As Integer = conn.ExecuteScalar(Of Integer)(sql, New With {
                                    .UUID = artwork.UUID.ToString(),
                                    .Title = artwork.Title,
                                    .Author = artwork.Author,
                                    .CreateTime = DateTimeToUnixTimestamp(artwork.CreateTime),
                                    .ImportTime = DateTimeToUnixTimestamp(artwork.ImportTime),
                                    .UpdateTime = DateTimeToUnixTimestamp(artwork.UpdateTime),
                                    .IsDeleted = 0,
                                    .Tags = JsonSerializer.Serialize(If(artwork.Tags, Array.Empty(Of String)())),
                                    .Notes = artwork.Notes,
                                    .Characters = JsonSerializer.Serialize(If(artwork.Characters, Array.Empty(Of String)()))
                                })
                    If artwork.Thumbnail IsNot Nothing Then
                        InsertThumbnail(newId, artwork.Thumbnail, conn, trans) '存入缩略图
                    End If
                    trans.Commit() '提交事务
                Catch ex As Exception
                    trans.Rollback() '保证缩略图与元数据的原子性
                    Throw
                End Try
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' 更新已有稿件
    ''' </summary>
    Public Sub UpdateArtwork(artwork As Artwork)
        Using conn As New SQLiteConnection(_ConnectionString)
            artwork.UpdateTime = DateTime.Now
            Dim sql As String = "
            UPDATE Artworks SET
                Title = @Title,
                Author = @Author,
                Characters = @Characters,
                CreateTime = @CreateTime,
                UpdateTime = @UpdateTime,
                IsDeleted = @IsDeleted,
                Tags = @Tags,
                Notes = @Notes
            WHERE UUID = @UUID"
            Try
                conn.Execute(sql, New With {
                                .UUID = artwork.UUID.ToString(),
                                .Title = artwork.Title,
                                .Author = artwork.Author,
                                .CreateTime = DateTimeToUnixTimestamp(artwork.CreateTime),
                                .UpdateTime = DateTimeToUnixTimestamp(artwork.UpdateTime),
                                .IsDeleted = artwork.IsDeleted,
                                .Tags = JsonSerializer.Serialize(If(artwork.Tags, Array.Empty(Of String)())),
                                .Notes = artwork.Notes,
                                .Characters = JsonSerializer.Serialize(If(artwork.Characters, Array.Empty(Of String)()))
                            })
                conn.Open()
                If artwork.Thumbnail IsNot Nothing Then
                    UpdateThumbnail(artwork.ID, artwork.Thumbnail, conn) '存入缩略图
                End If
            Catch ex As SqlException
                Throw
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 根据UUID获取稿件
    ''' </summary>
    Public Function GetArtworkByUUID(uuid As Guid) As Artwork
        Using conn As New SQLiteConnection(_ConnectionString)
            Dim sql As String = "SELECT * FROM Artworks WHERE UUID = @UUID"
            Try
                Dim result = conn.Query(sql, New With {.UUID = uuid.ToString()}).FirstOrDefault()

                If result Is Nothing Then
                    Return Nothing
                End If
                Return MapToArtwork(result)
            Catch ex As SqlException
                Throw
            End Try
        End Using
    End Function

    ''' <summary>
    ''' 获取所有稿件(可分页)
    ''' </summary>
    Public Function GetAllArtworks(Optional isDeleted As Integer = 0,
                                   Optional page As Integer = 1,
                                   Optional pageSize As Integer = 100) As List(Of Artwork)
        Using conn As New SQLiteConnection(_ConnectionString)
            Dim offset As Integer = (page - 1) * pageSize

            Dim sql As String = "
            SELECT * FROM Artworks 
            WHERE IsDeleted = @IsDeleted 
            ORDER BY ImportTime DESC 
            LIMIT @PageSize OFFSET @Offset"

            Try
                Dim results = conn.Query(sql, New With {
                                .IsDeleted = isDeleted,
                                .PageSize = pageSize,
                                .Offset = offset
                            })
                Return results.Select(Function(r) MapToArtwork(r)).ToList()

            Catch ex As SqlException
                Throw
            End Try

        End Using
    End Function

    ''' <summary>
    ''' 获得全部数据
    ''' </summary>
    Public Function GetAllArtworksComplete(Optional isDeleted As Integer = 0,
                                      Optional batchSize As Integer = 1000) As List(Of Artwork)
        Dim allArtworks As New List(Of Artwork)()
        Dim page As Integer = 1
        Dim continueFetching As Boolean = True

        Using conn As New SQLiteConnection(_ConnectionString)
            conn.Open()

            While continueFetching
                Try
                    Dim offset As Integer = (page - 1) * batchSize

                    Dim sql As String = "
                SELECT * FROM Artworks 
                WHERE IsDeleted = @IsDeleted 
                ORDER BY ImportTime DESC 
                LIMIT @PageSize OFFSET @Offset"

                    Dim results = conn.Query(sql, New With {
                    .IsDeleted = isDeleted,
                    .PageSize = batchSize,
                    .Offset = offset
                })

                    Dim currentBatch As List(Of Artwork) = results.Select(Function(r) MapToArtwork(r)).ToList()

                    If currentBatch.Count > 0 Then
                        allArtworks.AddRange(currentBatch)
                        If currentBatch.Count < batchSize Then
                            continueFetching = False
                        Else
                            page += 1
                        End If
                    Else
                        continueFetching = False
                    End If

                Catch ex As SqlException
                    Throw
                End Try
            End While
        End Using

        Return allArtworks
    End Function

    ''' <summary>
    ''' 根据标签筛选稿件
    ''' </summary>
    Public Function GetArtworksByTag(tag As String, Optional isDeleted As Integer = 0) As List(Of Artwork)
        Using conn As New SQLiteConnection(_ConnectionString)
            '使用JSON函数查询(SQLite 3.9+ 支持)
            Dim sql As String = "
            SELECT * FROM Artworks 
            WHERE IsDeleted = @IsDeleted 
            AND json_array_contains(Tags, @Tag)"


            Try
                Dim results = conn.Query(sql, New With {
                                .IsDeleted = isDeleted,
                                .Tag = tag
                            })
                Return results.Select(Function(r) MapToArtwork(r)).ToList()
            Catch ex As SqlException
                Throw
            End Try

        End Using
    End Function

    ''' <summary>
    ''' 搜索稿件(标题, 作者, 备注)
    ''' </summary>
    Public Function SearchArtworks(keyword As String, Optional isDeleted As Integer = 0) As List(Of Artwork)
        Using conn As New SQLiteConnection(_ConnectionString)
            Dim sql As String = "
            SELECT a.* FROM Artworks a
            JOIN ArtworksFTS f ON a.ID = f.rowid
            WHERE a.IsDeleted = @IsDeleted
            AND ArtworksFTS MATCH @Keyword"

            Try
                Dim results = conn.Query(sql, New With {
                    .IsDeleted = isDeleted,
                    .Keyword = keyword
                })
                Return results.Select(Function(r) MapToArtwork(r)).ToList()
            Catch ex As SqlException
                Throw
            End Try

        End Using
    End Function

    ''' <summary>
    ''' 软删除艺术作品(标记为已删除)
    ''' </summary>
    Public Sub SoftDeleteArtwork(uuid As Guid)
        Using conn As New SQLiteConnection(_ConnectionString)
            Dim sql As String = "
            UPDATE Artworks SET 
                IsDeleted = @IsDeleted,
                UpdateTime = @UpdateTime
            WHERE UUID = @UUID"

            Try
                conn.Execute(sql, New With {
                .UUID = uuid.ToString(),
                .IsDeleted = 1,
                .UpdateTime = DateTimeToUnixTimestamp(DateTime.Now)
            })

            Catch ex As SqlException
                Throw
            End Try

        End Using
    End Sub

    ''' <summary>
    ''' 获取所有标签及其使用次数
    ''' </summary>
    Public Function GetAllTags() As Dictionary(Of String, Integer)
        Using conn As New SQLiteConnection(_ConnectionString)
            Dim sql As String = "
            SELECT TagName, UsageCount FROM Tags 
            ORDER BY UsageCount DESC"

            Try
                Return conn.Query(sql).ToDictionary(
                                Function(row) CStr(row.TagName),
                                Function(row) CInt(row.UsageCount)
                            )
            Catch ex As SqlException
                Throw
            End Try

        End Using
    End Function

    ''' <summary>
    ''' 将缩略图存入数据库
    ''' </summary>
    ''' <param name="artworkId">关联的作品ID</param>
    ''' <param name="img">要插入的图片</param>
    ''' <param name="conn">数据库连接</param>
    ''' <param name="trans">数据库事务</param>
    Private Sub InsertThumbnail(artworkId As Integer, img As Image, conn As SQLiteConnection, trans As SQLiteTransaction)
        Using ms As New MemoryStream()
            img.Save(ms, ImageFormat.Jpeg)
            Dim imageBytes As Byte() = ms.ToArray()

            Using cmd As New SQLiteCommand("INSERT INTO Thumbnails (ArtworkID, ImageData) VALUES (@id, @data)", conn, trans)
                cmd.Parameters.AddWithValue("@id", artworkId)
                cmd.Parameters.AddWithValue("@data", imageBytes)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' 更新缩略图数据
    ''' </summary>
    ''' <param name="artworkId">关联的作品ID</param>
    ''' <param name="img">新的缩略图图片</param>
    ''' <param name="conn">数据库连接</param>
    Private Sub UpdateThumbnail(artworkId As Integer, img As Image, conn As SQLiteConnection)
        Using ms As New MemoryStream()
            img.Save(ms, ImageFormat.Jpeg)
            Dim imageBytes As Byte() = ms.ToArray()

            Using cmd As New SQLiteCommand("UPDATE Thumbnails SET ImageData = @data WHERE ArtworkID = @id", conn)
                cmd.Parameters.AddWithValue("@id", artworkId)
                cmd.Parameters.AddWithValue("@data", imageBytes)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' 从数据库读取特定ID的缩略图
    ''' </summary>
    ''' <returns>Image 对象, 若不存在则返回 Nothing</returns>
    Public Function GetThumbnail(artworkId As Integer) As Image
        Using conn As New SQLiteConnection(ConnectionString)
            conn.Open()
            Using cmd As New SQLiteCommand("SELECT ImageData FROM Thumbnails WHERE ArtworkID = @id LIMIT 1", conn)
                cmd.Parameters.AddWithValue("@id", artworkId)
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim imageBytes As Byte() = DirectCast(reader("ImageData"), Byte())
                        Using ms As New MemoryStream(imageBytes) '创建临时 Image 对象
                            Dim tempImage As Image = Image.FromStream(ms)
                            Return New Bitmap(tempImage) '保证不被 Using 清理掉
                        End Using
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 数据库行映射到Artwork对象
    ''' </summary>
    Private Function MapToArtwork(row As Object) As Artwork
        Dim targetDir As String = Path.Combine(Me._LibraryPath, row.UUID.ToString())
        Dim files As String() = Array.Empty(Of String)()
        If Directory.Exists(targetDir) Then
            files = Directory.GetFiles(targetDir)
        End If

        Return New Artwork() With {
            .ID = CInt(row.ID),
            .UUID = Guid.Parse(row.UUID.ToString()),
            .Title = If(row.Title, ""),
            .Author = If(row.Author, ""),
            .Characters = If(row.Characters IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(row.Characters),
                      JsonSerializer.Deserialize(Of String())(row.Characters),
                      Array.Empty(Of String)()),
            .CreateTime = UnixTimestampToDateTime(row.CreateTime),
            .ImportTime = UnixTimestampToDateTime(row.ImportTime),
            .UpdateTime = UnixTimestampToDateTime(row.UpdateTime),
            .IsDeleted = CType(row.IsDeleted, Integer),
            .Tags = If(row.Tags IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(row.Tags),
                      JsonSerializer.Deserialize(Of String())(row.Tags),
                      Array.Empty(Of String)()),
            .Notes = If(row.Notes, ""),
            .FilePaths = files,
            .Thumbnail = GetThumbnail(CInt(row.ID))
        }
    End Function

#Region "CSV相关处理"

    ''' <summary>
    ''' 将数据导出为CSV文件
    ''' </summary>
    ''' <param name="csvFilePath">csv文件路径</param>
    Public Sub ExportTableToCSV(csvFilePath As String)
        Try
            Using conn As New SQLiteConnection(_ConnectionString)
                conn.Open()
                '查询表中的所有数据
                Dim sql As String = $"SELECT * FROM Artworks"
                Dim data = conn.Query(sql)
                '检查是否有数据
                If data Is Nothing OrElse Not data.Any() Then
                    Throw New Exception("没有数据可以导出")
                End If
                Using writer As New StreamWriter(csvFilePath, False, Encoding.UTF8)
                    '获取第一行数据以获取列信息
                    Dim firstRow = data.First()
                    Dim columnNames As New List(Of String)
                    '如果是动态类型
                    If TypeOf firstRow Is IDictionary(Of String, Object) Then
                        Dim dictRow = CType(firstRow, IDictionary(Of String, Object))
                        columnNames.AddRange(dictRow.Keys)
                        '写入列名
                        writer.WriteLine(String.Join(",", columnNames.Select(Function(c) EscapeCsvField(c))))
                        '写入数据
                        For Each row In data
                            Dim dict = CType(row, IDictionary(Of String, Object))
                            Dim fields As New List(Of String)
                            For Each colName In columnNames
                                Dim value = dict(colName)
                                Dim stringValue = If(value Is Nothing, "", value.ToString())
                                fields.Add(EscapeCsvField(stringValue))
                            Next
                            writer.WriteLine(String.Join(",", fields))
                        Next
                    Else
                        '如果是强类型对象
                        Dim properties = firstRow.GetType().GetProperties()
                        '写入列名
                        Dim headers = properties.Select(Function(p) EscapeCsvField(p.Name)).ToList()
                        writer.WriteLine(String.Join(",", headers))
                        '写入数据
                        For Each row In data
                            Dim fields As New List(Of String)
                            For Each prop In properties
                                Dim value = prop.GetValue(row)
                                Dim stringValue = If(value Is Nothing, "", value.ToString())
                                fields.Add(EscapeCsvField(stringValue))
                            Next
                            writer.WriteLine(String.Join(",", fields))
                        Next
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception($"导出CSV文件失败: {ex.Message}", ex)
        End Try
    End Sub
    Private Function EscapeCsvField(field As String) As String
        If String.IsNullOrEmpty(field) Then
            Return ""
        End If
        '检查是否包含逗号, 引号或换行符
        If field.Contains(",") OrElse field.Contains("""") OrElse
           field.Contains(vbCrLf) OrElse field.Contains(vbCr) OrElse
           field.Contains(vbLf) Then
            '将双引号替换为两个双引号
            field = field.Replace("""", """""")
            '用双引号括起整个字段
            field = """" & field & """"
        End If
        Return field
    End Function

#End Region

End Class