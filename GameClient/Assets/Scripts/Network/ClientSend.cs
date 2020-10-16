using UnityEngine;

namespace Network
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTcpData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        private static void SendUdpData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #region Packets

        public static void SendLoginInfo()
        {
            using (var _packet = new Packet((int) ClientPackets.sendLoginInfo))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(UIManager.Instance.usernameField.text);
                _packet.Write(UIManager.Instance.passwordField.text);
                SendTcpData(_packet);
            }
            PlayerVariables.UserName = UIManager.Instance.usernameField.text;
        }

        public static void WelcomeReceived()
        {
            using (var _packet = new Packet((int) ClientPackets.welcomeReceived))
            {
                _packet.Write(Client.instance.myId);

                SendTcpData(_packet);
            }
        }

        public static void SendInvitation(int _destinationID, string _fromUser, string _toUser)
        {
            using (var _packet = new Packet((int) ClientPackets.sendInviteClient))
            {
                _packet.Write(_destinationID);
                _packet.Write(_fromUser);
                _packet.Write(_toUser);
                SendTcpData(_packet);
            }
        }

        public static void SendInviteResponse(bool x, string whoSent) //After Pressing Decline or Accept
        {
            using (var _packet = new Packet((int) ClientPackets.inviteAnswer))
            {
                _packet.Write(x);
                _packet.Write(whoSent);
                _packet.Write(Constants.Username);
                SendTcpData(_packet);
            }
        }

        public static void PlayerMovement(bool[] _inputs)
        {
            using (var _packet = new Packet((int) ClientPackets.playerMovement))
            {
                _packet.Write(_inputs.Length);
                foreach (var _input in _inputs) _packet.Write(_input);
                _packet.Write(GameManager.Players[Client.instance.myId].transform.rotation);
                SendUdpData(_packet);
            }
        }
        public static void AddMatchMaking()
        {
            using (var _packet = new Packet((int) ClientPackets.startMatchMaking))
            {
                _packet.Write(Constants.InParty);
                SendTcpData(_packet);
            }
        }
        
        public static void RemoveMatchMaking()
        {
             using (var _packet = new Packet((int) ClientPackets.stopMatchMaking))
             {
                 _packet.Write(Constants.InParty);
                 SendTcpData(_packet);
             }
        }
        #endregion

       
    }
}