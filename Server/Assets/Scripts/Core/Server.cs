using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; set; }
        public static readonly Dictionary<int, Client> Clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;
        private static TcpListener _tcpListener;
        private static UdpClient _udpListener;
        
        public static void Stop()
        {
            _tcpListener.Stop();
            _udpListener.Close();
        }
        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;
            InitializeServerData();
            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            _udpListener = new UdpClient(Port);
            _udpListener.BeginReceive(UdpReceiveCallback, null);
        }

        private static void TcpConnectCallback(IAsyncResult result)
        {
            TcpClient client = _tcpListener.EndAcceptTcpClient(result);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            ServerConsoleWriter.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");
            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (Clients[i].TcpInstance.Socket == null)
                {
                    Clients[i].TcpInstance.Connect(client);
                    return;
                }
            }
            ServerConsoleWriter.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void UdpReceiveCallback(IAsyncResult result)
        {
            try
            {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = _udpListener.EndReceive(result, ref clientEndPoint);
                _udpListener.BeginReceive(UdpReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                using (var packet = new Packet(data))
                {
                    var clientId = packet.ReadInt();

                    if (clientId == 0)
                    {
                        return;
                    }

                    if (Clients[clientId].UdpInstance.EndPoint == null)
                    {
                        Clients[clientId].UdpInstance.Connect(clientEndPoint);
                        return;
                    }

                    if (Clients[clientId].UdpInstance.EndPoint.ToString() == clientEndPoint.ToString())
                    {
                        Clients[clientId].UdpInstance.HandleData(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error receiving UDP data: {ex}");
            }
        }

        public static void SendUdpData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if (clientEndPoint != null)
                {
                    _udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to {clientEndPoint} via UDP: {ex}");
            }
        }

        private static void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayers; i++)
            {
                Clients.Add(i, new Client(i));
            }
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomereceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
                {(int)ClientPackets.sendLoginInfo, ServerHandle.LoginInformation},
                {(int)ClientPackets.sendInviteClient, ServerHandle.SendInvitationServer},
                {(int)ClientPackets.inviteAnswer, ServerHandle.SendInviteAnswer},
                {(int)ClientPackets.startMatchMaking, ServerHandle.AddToMatchMaking},
                {(int)ClientPackets.stopMatchMaking, ServerHandle.RemoveFromMatchMaking},
                {(int)ClientPackets.mmPopUpAnswer, ServerHandle.MatchAnswer},
                {(int)ClientPackets.notifyPickUpdate, ServerHandle.HandleLobbyPlayer},
                {(int)ClientPackets.sceneLoaded, ServerHandle.HandleSceneUpdate},
                {(int)ClientPackets.sceneCompleted, ServerHandle.HandleSceneCompleted},
                {(int)ClientPackets.enteredConflictZone, ServerHandle.HandleGamePlayConflictZone},
                {(int)ClientPackets.enteredNeutralZone, ServerHandle.HandleGamePlayConflictZone},
                {(int)ClientPackets.enteredBeneficialZone, ServerHandle.HandleGamePlayConflictZone},
            };
            ServerConsoleWriter.WriteLine("Initialized packets.");
        }
    }
