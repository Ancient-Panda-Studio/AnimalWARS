using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerSend
{
    private static void SendTcpData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.Clients[_toClient].TcpInstance.SendData(_packet);
        }

        private static void SendUdpData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.Clients[_toClient].UdpInstance.SendData(_packet);
        }
        private static void SendTcpDataToList(Packet packet, ICollection sendToList)
        {
            packet.WriteLength();
            for (var i = 0; i <= sendToList.Count; i++)
            {
                Server.Clients[i].TcpInstance.SendData(packet);
            }
        }
        private static void SendTcpDataToListExcept(Packet packet, ICollection sendToList, int Except)
        {
            packet.WriteLength();
            for (var i = 0; i <= sendToList.Count; i++)
            {
                if(i == Except) continue;
                Server.Clients[i].TcpInstance.SendData(packet);
            }
        }
        private static void SendTcpDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.Clients[i].TcpInstance.SendData(_packet);
            }
        }

        public static void PlayerDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
            {
                _packet.Write(_playerId);
                SendTcpDataToAll(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.Clients[i].TcpInstance.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.Clients[i].UdpInstance.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.Clients[i].UdpInstance.SendData(_packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTcpData(_toClient, _packet);
            }
        }
        public static void LoginResult(int _playerId, bool _result, string _error,int _dbID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.handleLoginInfo))
            {
                _packet.Write(_playerId);
                _packet.Write(_result);
                _packet.Write(_error);
                _packet.Write(_dbID);
                SendTcpData(_playerId,_packet);
                // if(_result)
                // Server.clients[_playerId].SendIntoGame(_username);    
            }
        }
        public static void SpawnPlayer(List<int> toClient, GameObject player)
        {
            using (var packet = new Packet((int) ServerPackets.spawnPlayer))
            {
                // packet.Write(Dictionaries.PlayerDataHolders[toClient].InParty);
                // packet.Write(Dictionaries.PlayerDataHolders[toClient].Username);
                packet.Write(player.transform.position);
                packet.Write(player.transform.rotation);
                SendTcpDataToList(packet,toClient);
            }
        }
        public static void SpawnPlayer(List<int> toClient,int except, GameObject player)
        {
            using (var packet = new Packet((int) ServerPackets.spawnPlayer))
            {
                // packet.Write(Dictionaries.PlayerDataHolders[toClient].InParty);
                // packet.Write(Dictionaries.PlayerDataHolders[toClient].Username);
                packet.Write(player.transform.position);
                packet.Write(player.transform.rotation);
                SendTcpDataToListExcept(packet,toClient,except);
            }
        }
        public static void PlayerPosition(PlayerDataHolder player, List<int> sendToList)
        {
            using (var packet = new Packet((int)ServerPackets.playerPosition))
            {
                packet.Write(player.GetPlayerId());
                packet.Write(player.GetGameObject().transform.position);
                SendTcpDataToList(packet,sendToList);
            }
        }
        public static void PlayerRotation(PlayerDataHolder player, List<int> sendToList)
        {
            using (var packet = new Packet((int)ServerPackets.playerRotation))
            {
                packet.Write(player.GetPlayerId());
                packet.Write(player.GetGameObject().transform.rotation);
                SendTcpDataToList(packet,sendToList);
            }
        }
        public static void SendInvite(int fromID, string userName, string toUserName,int sendTo)
        {
            using (var packet = new Packet((int)ServerPackets.sendInviteServer))
            {
                packet.Write(fromID); //Who sent it
                packet.Write(userName); //Who sent it Username
                packet.Write(toUserName); //Name of who is this for
                SendTcpData(sendTo,packet);
            }
        }
        public static void SendInviteAnswer(int fromClient, bool answer, string name, int sendTo)
        {
            using (var packet = new Packet((int)ServerPackets.sendInviteAnswer))
            {
                packet.Write(fromClient);
                packet.Write(answer);
                packet.Write(name);
                SendTcpData(sendTo,packet);
            }
            
        }
        public static void MatchMakingState(int sendTo)
        {
            using (var packet = new Packet((int)ServerPackets.mmOk))
            {
                SendTcpData(sendTo,packet);
            }
        }
        public static void RemoveLfgButton(int sendTo)
        {
            using (var packet = new Packet((int)ServerPackets.removeLFButtons))
            {
                SendTcpData(sendTo,packet);
            }
        }
        #endregion


       
}
