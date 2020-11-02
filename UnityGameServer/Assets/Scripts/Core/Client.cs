using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    private const int DataBufferSize = 4096;

    private readonly int id;
    public readonly Tcp TcpInstance;
    public readonly Udp UdpInstance;

    public Client(int clientId)
    {
        id = clientId;
        TcpInstance = new Tcp(id);
        UdpInstance = new Udp(id);
    }

    public class Tcp
    {
        public TcpClient Socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public Tcp(int id)
        {
            this.id = id;
        }

        /// <summary>Initializes the newly connected client's TCP-related info.</summary>
        /// <param name="socket">The TcpClient instance of the newly connected client.</param>
        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            stream = Socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[DataBufferSize];

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome to the server!");
        }

        /// <summary>Sends data to the client via TCP.</summary>
        /// <param name="packet">The packet to send.</param>
        public void SendData(Packet packet)
        {
            try
            {
                if (Socket != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null); // Send data to appropriate client
                }
            }
            catch (Exception ex)
            {
                //Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        /// <summary>Reads incoming data from the stream.</summary>
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    if (Server.Clients.ContainsKey(id)) Server.Clients[id].Disconnect();
                    return;
                }

                var data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                receivedData.Reset(HandleData(data)); // Reset receivedData if all data was handled
                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                //
                ServerConsoleWriter.WriteLine($"Error receiving TCP data: {ex}");
                Server.Clients[id].Disconnect();
            }
        }

        /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
        /// <param name="data">The recieved data.</param>
        private bool HandleData(byte[] data)
        {
            var packetLength = 0;

            receivedData.SetBytes(data);

            if (receivedData.UnreadLength() >= 4)
            {
                // If client's received data contains a packet
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    // If packet contains no data
                    return true; // Reset receivedData instance to allow it to be reused
                }
            }

            while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
            {
                // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
                var packetBytes = receivedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (var packet = new Packet(packetBytes))
                    {
                        var packetId = packet.ReadInt();
                        Server.PacketHandlers[packetId](id, packet); // Call appropriate method to handle the packet
                    }
                });

                packetLength = 0; // Reset packet length
                if (receivedData.UnreadLength() >= 4)
                {
                    // If client's received data contains another packet
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        // If packet contains no data
                        return true; // Reset receivedData instance to allow it to be reused
                    }
                }
            }

            return packetLength <= 1;
        }

        /// <summary>Closes and cleans up the TCP connection.</summary>
        public void Disconnect()
        {
           
        
                var item = Dictionaries.PlayersByName.FirstOrDefault(kvp => kvp.Value == id);

                if (item.Key != null)
                {
                    if (Dictionaries.PlayerDataHolders[id].GetGameObject() != null)
                    {
                        ThreadManager.ExecuteOnMainThread(() =>
                        {
                            ServerConsoleWriter.WriteLine($"Destroying GameObjects related to Player {id}...");
                            Dictionaries.PlayerDataHolders[id].DestroyGameObject();
                            Dictionaries.PlayerDataHolders.Remove(id);
                            Dictionaries.PlayersById.Remove(id);
                            Dictionaries.PlayersByName.Remove(item.Key);
                        });
                    }else
                    {
                        ServerConsoleWriter.WriteLine($"Player {id} had not started a game so no gameobject should be destroyed");
                        Dictionaries.PlayerDataHolders.Remove(id);
                        Dictionaries.PlayersById.Remove(id);
                        Dictionaries.PlayersByName.Remove(item.Key);
                    }
                    ServerConsoleWriter.WriteLine($"Player {id} has been successfully disconnected");
                }

                //Dictionaries.Parties.Remove(id);
            Socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            Socket = null;
        }

        
    }    

    public class Udp
    {
        public IPEndPoint EndPoint;

        private readonly int id;

        public Udp(int id)
        {
            this.id = id;
        }

        /// <summary>Initializes the newly connected client's UDP-related info.</summary>
        /// <param name="endPoint">The IPEndPoint instance of the newly connected client.</param>
        public void Connect(IPEndPoint endPoint)
        {    
            EndPoint = endPoint;
        }

        /// <summary>Sends data to the client via UDP.</summary>
        /// <param name="packet">The packet to send.</param>
        public void SendData(Packet packet)
        {
            Server.SendUdpData(EndPoint, packet);
        }

        /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
        /// <param name="packetData">The packet containing the recieved data.</param>
        public void HandleData(Packet packetData)
        {
            var packetLength = packetData.ReadInt();
            var packetBytes = packetData.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (var packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Server.PacketHandlers[packetId](id, packet); // Call appropriate method to handle the packet
                }
            });
        }

        /// <summary>Cleans up the UDP connection.</summary>
        public void Disconnect()
        {
            EndPoint = null;
        }
    }

    /// <summary>Sends the client into the game and informs other clients of the new player.</summary>
    /// <param name="_match"></param>
    /// <param name="_caller"></param>
    /// <param name="_newMapGameObject"></param>
    public void SendIntoMatch(Match _match, PlayerDataHolder _caller, SpawnedMap _newMapGameObject)
    {
        /* LOBBY
            PLAYER PICKS AN ANIMAL
            THAT ANIMAL GETS LOCKED FOR OTHER PLAYERS
            TIMER OF 30 SECONDS TO FINISH PICKS 
                IF TIMER RUNS OUT AND SOMEONE HAS NOT PICKED THAT PLAYER GETS TEMPORARILY SUSPENDED
            ELSE IF ALL PLAYERS HAVE PICKED START GAME
         */
        var sendTo = Parser.ParseHolderToInt(_match.GetAllPlayers());
        //GET SPAWN POINT
        var team = _match.FindPlayerTeam(_caller);
        Debug.Log("Team is : " + team);
        var spawns =  _newMapGameObject.GetFreeSpawn(team);
        Debug.Log(_newMapGameObject.GetFreeSpawns(team).Count);
        spawns.SetFull(true);
        Debug.Log(_newMapGameObject.GetFreeSpawns(team).Count);
       var newPlayer = NetworkManager.Instance.InstantiatePlayer(spawns.myGameObject.transform);

        Dictionaries.PlayerDataHolders[id].SetGameObject(newPlayer);
        Debug.Log(Dictionaries.PlayerDataHolders[id].GetGameObject().name = $"UWU{id}");
        ServerSend.SpawnPlayer(sendTo,_caller.GetPlayerId(),newPlayer);
        ServerSend.SpawnPlayer(sendTo,newPlayer);
    }
    public void RequestLogin(string username, string password)
    {
        NetworkManager.Instance.StartCoroutine(LoginStart(username,password));
    }

    private IEnumerator LoginStart(string user, string pass)
    {
        var form = new WWWForm();
        form.AddField("user",user);
        form.AddField("pass",pass);
        var www = new WWW(Constants.SQL_NAME_SERVER + "login.php",form);
        yield return www;
        if (www.text[0] == '0')
        {
            //Allow Login
            Dictionaries.PlayersByName.Add(user,id);
            Dictionaries.PlayersById.Add(id,user);
            ServerSend.LoginResult(id,true, "pepelaugh",           int.Parse(www.text.Split('\t')[1]));
            Dictionaries.PlayerDataHolders.Add(id,new PlayerDataHolder(id,user));
        }
        else
        {
            ServerSend.LoginResult(id,false, www.text, -9);
        }
        
    }
    /// <summary>Disconnects the client and stops all network traffic.</summary>
    
    private void Disconnect()
    {
      
        TcpInstance.Disconnect();
        UdpInstance.Disconnect();

        ServerSend.PlayerDisconnected(id);
        ServerConsoleWriter.WriteLine($"Player {id} has successfully disconnected from the server");

    }
}
