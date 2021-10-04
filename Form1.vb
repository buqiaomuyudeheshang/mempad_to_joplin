Imports System.IO

Public Class Form1
    Dim save_dir As String = "D:\目录" '定义处理好的文件存放基路径
    Dim index_name(99) As String '定义数组，用于存放所有的划分层级，目前定义为99级
    Dim read_file As StreamReader
    Dim line As String
    Dim writer_file As StreamWriter
    Dim file_dir As String '定义目录，是选择的程序目录
    Dim Flag As Boolean = True '定义标识，如果是true就是目录，如果是false就是文本


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        file_dir = selectFolder()
        If Len(file_dir) Then
            do_work()
        Else
            MsgBox("您没有选择文件，无法处理")
        End If
        read_file.Close()
        writer_file.Close()
    End Sub

    Public Function selectFolder() As String
        Dim obj As New System.Windows.Forms.OpenFileDialog
        Dim sFile As String
        With obj
            .Title = "请选择配置文件"
            .Filter = "txt files(*.txt)|*.txt"
            .ShowDialog()
            sFile = .FileName
        End With
        Return sFile
    End Function
    Public Sub do_work() '开始处理文件
        read_file = New StreamReader(file_dir)
        Do
            line = read_file.ReadLine()
            If line IsNot Nothing Then '空行不处理
                If line.Length <> 0 Then
                    Judge_Format()
                    If Flag Then
                        Index_Judge()
                    Else
                        Writer_File_Text()
                    End If
                End If
            End If
        Loop Until line Is Nothing
        read_file.Close()
    End Sub
    Public Sub Judge_Format() '判断当前行是目录还是文字
        Dim fist_char As String
        fist_char = line.Substring(0, 1)
        If fist_char = "□" Then
            Flag = True
        Else
            Flag = False
        End If
    End Sub
    Public Sub Writer_File_Text() '如果是文字行则想入文件中
        If writer_file Is Nothing Then
            MsgBox("没有需要写入的文件，程序有问题，请处理")
            Exit Sub
        End If
        writer_file.WriteLine(line)
    End Sub
    Public Sub Index_Judge() '处理目录层级，并关闭和打开写入文件
        Dim i As Integer = 0
        Dim fist_char As String
        If Not writer_file Is Nothing Then
            writer_file.Close()
        End If
        Do
            fist_char = line.Substring(i, 1)
            i += 1
        Loop While fist_char = "□"
        i -= 2
        index_name(i) = line.Substring(i + 1)
        '处理当时名称中的不符合文件名称的字符。并统一替换为_
        Dim ban() As String = {"\", "/", ":", "*", "?", """", "<", ">", "|"} '要过滤的字符
        For Each Strs In ban
            index_name(i) = Replace(index_name(i), Strs, "_")
        Next

        Dim Text_File_Dir As String = save_dir
        For j As Integer = 0 To i
            Text_File_Dir = Text_File_Dir & "\" & index_name(j)
        Next
        Dim dir As String = Text_File_Dir & "\"
        If Not Directory.Exists(dir) Then
            Directory.CreateDirectory(dir)
        End If
        Text_File_Dir = Text_File_Dir & "\" & index_name(i) & ".md"
        writer_file = New StreamWriter(Text_File_Dir)
    End Sub
End Class
