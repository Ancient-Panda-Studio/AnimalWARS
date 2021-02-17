using UnityEngine;

namespace Network
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }

        private static void SendUdpData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.udp.SendData(packet);
        }

        #region Packets

        public static void SendLoginInfo()
        {
            Debug.Log("Sending...");
            using (var packet = new Packet((int) ClientPackets.sendLoginInfo))
            {
                packet.Write(Client.instance.myId);
                packet.Write(UIObjects.Instance.Login_usernameInputField.text);
                packet.Write(UIObjects.Instance.Login_passwordInputField.text);
                SendTcpData(packet);
            }
            PlayerVariables.UserName = UIObjects.Instance.Login_usernameInputField.text;
        }
        public static void SendLoginInfo(string user, string pass)
        {
            Debug.Log("Sending...");
            using (var packet = new Packet((int) ClientPackets.sendLoginInfo))
            {
                packet.Write(Client.instance.myId);
                packet.Write(user);
                packet.Write(pass);
                SendTcpData(packet);
            }
            PlayerVariables.UserName = UIObjects.Instance.Login_usernameInputField.text;
        }

        public static void WelcomeReceived()
        {
            using (var packet = new Packet((int) ClientPackets.welcomereceived))
            {
                packet.Write(Client.instance.myId);
                SendTcpData(packet);
            }
            SendLoginInfo(Constants.User, Constants.Pass);
        }

        public static void SendInvitation(int _destinationID, string _fromUser, string _toUser)
        {
            using (var packet = new Packet((int) ClientPackets.sendInviteClient))
            {
                packet.Write(_destinationID);
                packet.Write(_fromUser);
                packet.Write(_toUser);
                SendTcpData(packet);
            }
        }
        public static void MatchFoundAnswer(bool answer)
        {
            Debug.Log("Calling Answer -> NOW");
            using (var packet = new Packet((int) ClientPackets.mmPopUpAnswer))
            {
                packet.Write(answer);
                SendTcpData(packet);
            } 
        }
        public static void SendInviteResponse(bool x, string whoSent) //After Pressing Decline or Accept
        {
            using (var packet = new Packet((int) ClientPackets.inviteAnswer))
            {
                packet.Write(x);
                packet.Write(whoSent);
                packet.Write(Constants.Username);
                SendTcpData(packet);
            }
        }

        public static void PlayerMovement(Vector3 position, Quaternion rotation)
        {
            using (var packet = new Packet((int) ClientPackets.playerMovement))
            {
                packet.Write(MatchVariables.MatchId);
                packet.Write(position);
                packet.Write(rotation);
                SendUdpData(packet);
            }
        }
        public static void AddMatchMaking(int mapId)
        {
            using (var packet = new Packet((int) ClientPackets.startMatchMaking))
            {
                packet.Write(mapId);
                packet.Write(Constants.InParty);
                SendTcpData(packet);
            }
        }
        public static void RemoveMatchMaking()
        {
             using (var packet = new Packet((int) ClientPackets.stopMatchMaking))
             {
                 packet.Write(Constants.InParty);
                 SendTcpData(packet);
             }
        }

        public static void SendPickUpdate(int whatPick)
        {
            
            Debug.Log(MatchVariables.MatchId);
            using (var packet = new Packet((int) ClientPackets.notifyPickUpdate))
            {
                packet.Write(MatchVariables.MatchId);
                packet.Write(whatPick);
                SendTcpData(packet);
            } 
        }
        public static void SendSceneLoaded()
        {
            using (var packet = new Packet((int) ClientPackets.sceneLoaded))
            {
               packet.Write(MatchVariables.MatchId);
               SendTcpData(packet);
            }
            
        }
        public static void SceneIsFullyReady()
        {
            using (var packet = new Packet((int)    ClientPackets.sceneCompleted))
            {
                packet.Write(MatchVariables.MatchId);
                SendTcpData(packet);
            }
        }

        private static void SendZoneState(Packet packet)
        {
            SendTcpData(packet);
        }
        #endregion

        public static void EnteredConflictZone(int zoneID)
        {
            using (var packet = new Packet((int) ClientPackets.enteredConflictZone))
            {
                packet.Write(MatchVariables.MatchId);
                packet.Write(zoneID);
                SendZoneState(packet);
            }
        }
        public static void EnteredBeneficialNeutral(int zoneID)
        {
            using (var packet = new Packet((int) ClientPackets.enteredNeutralZone))
            {
                packet.Write(MatchVariables.MatchId);
                packet.Write(zoneID);
                SendZoneState(packet);
            }
        }
        public static void EnteredBeneficialZone(int zoneID)
        {
            using (var packet = new Packet((int) ClientPackets.enteredBeneficialZone))
            {
                packet.Write(MatchVariables.MatchId);
                packet.Write(zoneID);
                SendZoneState(packet);
            }
        }
    }
}