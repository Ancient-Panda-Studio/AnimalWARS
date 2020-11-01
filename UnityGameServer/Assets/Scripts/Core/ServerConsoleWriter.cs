
public class ServerConsoleWriter
{
    public static void WriteLine(string _toWrite)
    {
        using (var file =
            new System.IO.StreamWriter(@"C:\Users\k\Desktop\ServerLog.txt", true))
        {
            file.WriteLine(_toWrite);
        }
    }
}
