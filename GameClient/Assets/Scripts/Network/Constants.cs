using System;

public class Constants
{
    public const string WebServer = "http://localhost/sqlconnect/";
    public static string Inviter;
    public static readonly string SysetmUser = Environment.UserName;
    public static string FilePath = $@"C:\Users\{SysetmUser}\Documents\AnimalWars\Settings\Settings.xml";
    public static bool InParty = false;
    public static int ID { get; set; }
    public static string Username { get; set; }
    public static int Score { get; set; }
    public static int InvitationID { get; set; }
    public static int PartyID { get; set; }
    public static int ServerID { get; set; }
}