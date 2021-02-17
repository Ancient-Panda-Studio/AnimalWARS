using System;
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
        private static void SendTcpDataToList(Packet packet, List<int> sendToList)
        {
            packet.WriteLength();
            foreach (var x in sendToList.Where(x => Server.Clients.ContainsKey(x)))
            {
                Server.Clients[x].TcpInstance.SendData(packet);
            }
        }
        private static void SendTcpDataToListExcept(Packet packet, List<int> sendToList, int Except)
        {
            packet.WriteLength();
            foreach (var x in sendToList.Where(x => Server.Clients.ContainsKey(x) && x != Except))
            {
                Server.Clients[x].TcpInstance.SendData(packet);
            }
        }
        private static void SendTcpDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (var i = 1; i <= Server.MaxPlayers; i++)
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
        public static void MatchFound(int _playerId, int _matchId) {
            using (Packet _packet = new Packet((int) ServerPackets.matchFound))
            {
                _packet.Write(_matchId);
                try {
                SendTcpData(_playerId, _packet);
                } catch {
                    ServerConsoleWriter.WriteLine($"Error sending {_packet} to {_playerId}");
                }
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
        /*public static void JordiMatch(int toClient) {
            using (var packet = new Packet((int) ServerPackets.jordiMatch))
            {
                       packet.Write(1);
                        SendTcpData(toClient, packet);
                   }
       }*/

        public static void BeginLobby(int toClient, int matchId, List<PlayerDataHolder> myTeam, List<PlayerDataHolder> enemyTeam)
        {
            using (var packet = new Packet((int) ServerPackets.lobbyconnect))
            {
                packet.Write(toClient);
                packet.Write(matchId);
                packet.Write(Dictionaries.dictionaries.Matches[matchId].GetPlayerTeam(toClient));
                packet.Write(myTeam[0].Username);
                packet.Write(myTeam[0].GetPlayerId());
                packet.Write(myTeam[1].Username);
                packet.Write(myTeam[1].GetPlayerId());
                packet.Write(myTeam[2].Username);
                packet.Write(myTeam[2].GetPlayerId());
                packet.Write(enemyTeam[0].Username);
                packet.Write(enemyTeam[0].GetPlayerId());
                packet.Write(enemyTeam[1].Username);
                packet.Write(enemyTeam[1].GetPlayerId());
                packet.Write(enemyTeam[2].Username);
                packet.Write(enemyTeam[2].GetPlayerId());
                SendTcpData(toClient, packet);
            }
        }
        public static void SpawnPlayer(int toClient,int who,int whatPick,Vector3 playerMovement)
        {
            using (var packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                packet.Write(who);
                packet.Write(whatPick);
                packet.Write(playerMovement);
                SendTcpData(toClient, packet);
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

        public static void StartMatch(int sendTo, int scene)
        {
            if(!Server.Clients.ContainsKey(sendTo)) return;
            using (var packet = new Packet((int) ServerPackets.beginMatch))
            {
                packet.Write(scene);
                SendTcpData(sendTo, packet);
            }
        }

        public static void UpdatePlayerPickLobby(int sendTo, int whoToUpdate, int whichPick)
        {
            using (var packet = new Packet((int) ServerPackets.sendPickUpdate))
            {
               packet.Write(whoToUpdate);  
               packet.Write(whichPick);
               SendTcpData(sendTo,packet);
            }
        }

        public static void EndLobby(int playerKey, int scene)
        {
            using (var packet = new Packet((int) ServerPackets.endLobby)){
                packet.Write(scene); 
                SendTcpData(playerKey, packet);
            }
        }
        
        public static void PlayerPosition(int playerID,int fromClient, Vector3 position, Quaternion rotation) //UPDATES THE PLAYER POSITION
        {
            using (var packet = new Packet((int) ServerPackets.playerPosition))
            {
                packet.Write(fromClient);
                packet.Write(position);
                packet.Write(rotation);
                SendUdpData(playerID, packet);
            }
        }

        public static void RemoveCanva(int playerId)
        {
            using (var packet = new Packet((int) ServerPackets.removeCanvas) )
            {
                packet.Write(playerId);
                SendTcpData(playerId, packet);
            }
        }

        public static void SendZoneValues(int zone1Value, int zone2Value, int zone3Value, int zone4Value, int zone7Value, int playerId)
        {
            using (var packet = new Packet((int) ServerPackets.sendPlayerConflictZone) )
            {
                packet.Write(zone1Value);
                packet.Write(zone2Value);
                packet.Write(zone3Value);
                packet.Write(zone4Value);
                packet.Write(zone7Value);
                SendUdpData(playerId, packet);
            }
        }

        public static void UpdateZoneState(int zoneToUpdate, int team, int matchId)
        {
            foreach (var sx in Dictionaries.dictionaries.Matches[matchId].GetAllPlayers())
            {
                using (var packet = new Packet((int) ServerPackets.updateZoneState) )
                {
                    packet.Write(zoneToUpdate);
                    packet.Write(team);
                    SendUdpData(sx.GetPlayerId(), packet);
                }
            }
        
        }
        public static void UpdatePlayerHealth(int playerId, float currentHp, int sendTo)
        {
            using (var packet = new Packet((int) ServerPackets.sendPlayerHealth) )
            {
                packet.Write(playerId);
                packet.Write(currentHp);
                SendUdpData(sendTo, packet);
            }
        }

        public static void SetDeathOnPlayer(int playerId, float deathTimer, int deathCount, int sendTo)
        {
            using (var packet = new Packet((int) ServerPackets.playerDied) )
            {
                packet.Write(playerId);
                packet.Write(deathTimer);
                packet.Write(deathCount);
                SendTcpData(sendTo, packet);
            }
        }

        public static void RemoveGameObjectFromMatch(int playerId)
        {
            using (var packet = new Packet((int) ServerPackets.removeFromMatch))
            {
                SendTcpData(playerId,packet);
            }
        }

        public static void SetMatchEndResult(string teamWhoWon, bool perfectScore, Match myMatch)
        {
            foreach (var player in myMatch.GetAllPlayers())
            {
                using (var packet = new Packet((int) ServerPackets.setMatchEndResult))
                {
                    packet.Write(teamWhoWon);
                    packet.Write(perfectScore);
                    SendTcpData(player.GetPlayerId(),packet);
                }
            }
        }

        public static void ForceSceneLoad(int scene, int toClient)
        {
            using (var packet = new Packet((int) ServerPackets.forceSceneLoad))
            {
                packet.Write(scene);
                SendTcpData(toClient,packet);
            }
        }

        public static void UpdateLocalPlayerTutorialStag(int toClient, int currentPlayerStage)
        {
            using (var packet = new Packet((int) ServerPackets.updateTutorialStage))
            {
                packet.Write(currentPlayerStage);
                SendTcpData(toClient,packet);
            }        
        }

        public static void BeginTutorial(int toClient)
        {
            using (var packet = new Packet((int) ServerPackets.beginTutorial))
            {
                SendTcpData(toClient,packet);
            }         
        }
}
