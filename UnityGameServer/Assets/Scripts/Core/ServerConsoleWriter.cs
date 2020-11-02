
using System;

public class ServerConsoleWriter
{
    public static readonly string SysetmUser = Environment.UserName;
    public static void WriteLine(string _toWrite)
    {
        using (var file =
            new System.IO.StreamWriter($@"C:\Users\{SysetmUser}\Desktop\ServerLog.txt", true))
        {
            file.WriteLine(_toWrite);
        }
    }
}
