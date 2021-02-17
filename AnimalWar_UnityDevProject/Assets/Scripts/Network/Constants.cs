using System;
using System.Collections.Generic;
using Player;

public class Constants
{
    public const string WebServer = "127.0.0.1/sqlconnect/";
    public static string Inviter;
    public static readonly string SysetmUser = Environment.UserName;
    public static string FilePath = $@"C:\Users\{SysetmUser}\Documents\AnimalWars\Settings";
    public static bool InParty = false;
    public static bool IsLogged = false;
    public static int DbId { get; set; }
    public static string Username { get; set; }
    public static int Score { get; set; }
    public static int InvitationID { get; set; }
    public static int PartyID { get; set; }
    public static int ServerID { get; set; }
    public static LoadingScene LoadingSceneScript { get; set; }
    public static string IpAdress { get; set; }
    public static string User { get; set; }
    public static string Pass { get; set; }

    public static string DirectoryPath = $@"C:\Users\{SysetmUser}\Documents\AnimalWars";
    public static string TestPath = $@"C:\Users\{SysetmUser}\Documents\AnimalVar.txt";

    public static LobbyManager LobbyManager;
}
public class MatchVariables
{
    public static Dictionary<int,string> Enemies = new Dictionary<int,string>();
    public static Dictionary<int,string> TeamMates = new Dictionary<int,string>();
    public static int MatchId; 
    public static int MyTeam;
    public static int WhoWon { get; set; }
}

public class UtilsOwn
{
    public static bool Between(int num, int lower, int upper, bool inclusive = false)
    {
        return inclusive
            ? lower <= num && num <= upper
            : lower < num && num < upper;
    }
}