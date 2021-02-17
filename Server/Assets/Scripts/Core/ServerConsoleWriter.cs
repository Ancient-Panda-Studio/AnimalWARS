
using System;
using System.IO;

public class ServerConsoleWriter
{
    private static readonly string SysetmUser = Environment.UserName;
    public static void WriteLine(string _toWrite)
    {
        return;
        if (Directory.Exists($@"/home/{SysetmUser}/ServerLog/"))
        {
            using (var file =
                new StreamWriter($@"/home/{SysetmUser}/ServerLog/ServerLog.txt", true))
            {
                file.WriteLine($"{DateTime.UtcNow} : {_toWrite}");
            }
        }
        else
        {
            Directory.CreateDirectory($@"/home/{SysetmUser}/ServerLog/");
        }
    }

    public static void WriteUserLog(string user, string _toWrite)
    {
        return;
        if (Directory.Exists($@"/home/{SysetmUser}/ServerLog/UserLogs/"))
        {
            using (var file =
                new System.IO.StreamWriter($@"/home/{SysetmUser}/ServerLog/UserLogs/{user}_log.txt", true))
            {
                file.WriteLine($"{DateTime.UtcNow} : {_toWrite}");
            }
        }
        else
        {
            Directory.CreateDirectory($@"/home/{SysetmUser}/ServerLog/UserLogs/");
        }
    }

    public static void WriteMatchLog(int getMatchIdNoStatic, string _toWrite)
    {
        return;
        if (Directory.Exists($@"/home/{SysetmUser}/ServerLog/MatchLogs/"))
        {
            using (var file =
                new System.IO.StreamWriter(
                    $@"/home/{SysetmUser}/ServerLog/MatchLogs/Match_{getMatchIdNoStatic}_log.txt", true))
            {
                file.WriteLine($"{DateTime.UtcNow} : {_toWrite}");
            }
        }
        else
        {
            Directory.CreateDirectory($@"/home/{SysetmUser}/ServerLog/MatchLogs/");

        }
    }
}
