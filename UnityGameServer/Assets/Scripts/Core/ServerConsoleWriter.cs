
using System;

public class ServerConsoleWriter
{
    private static readonly string SysetmUser = Environment.UserName;
    public static void WriteLine(string _toWrite)
    {
        using (var file =
            new System.IO.StreamWriter($@"C:\Users\{SysetmUser}\Desktop\ServerLog\ServerLog.txt", true))
        {
            file.WriteLine($"{DateTime.UtcNow} : {_toWrite}");
        }
    }

    public static void WriteUserLog(string user, string _toWrite)
    {
        using (var file =
            new System.IO.StreamWriter($@"C:\Users\{SysetmUser}\Desktop\ServerLog\UserLogs\{user}_log.txt", true))
        {
            file.WriteLine($"{DateTime.UtcNow} : {_toWrite}");
        }    
    }

    public static void WriteMatchLog(int getMatchIdNoStatic, string _toWrite)
    {
        using (var file =
            new System.IO.StreamWriter($@"C:\Users\{SysetmUser}\Desktop\ServerLog\MatchLogs\Match_{getMatchIdNoStatic}_log.txt", true))
        {
            file.WriteLine($"{DateTime.UtcNow} : {_toWrite}");
        }    
    }
}
