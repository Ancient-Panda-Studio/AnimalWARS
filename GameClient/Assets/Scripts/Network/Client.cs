using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Network;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;
    private static Dictionary<int, PacketHandler> packetHandlers;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId;


    private bool isConnected;
    public TCP tcp;
    public UDP udp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
                       Destroy(this);
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        tcp = new TCP();
        udp = new UDP();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        InitializeClientData();
        try
        {
            tcp.Connect();
            isConnected = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>
        {
            {(int) ServerPackets.welcome, ClientHandle.Welcome},
            {(int) ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer},
            {(int) ServerPackets.playerPosition, ClientHandle.PlayerPosition},
            {(int) ServerPackets.playerRotation, ClientHandle.PlayerRotation},
            {(int) ServerPackets.playerDisconnected, ClientHandle.PlayerDisconnected},
            {(int) ServerPackets.handleLoginInfo, ClientHandle.HandleLogin},
            {(int) ServerPackets.sendInviteServer, ClientHandle.InvitationReceived},
            {(int) ServerPackets.sendInviteAnswer, ClientHandle.InvitationResponse},
            {(int) ServerPackets.mmOk, ClientHandle.MMState},
            {(int) ServerPackets.removeLFButtons, ClientHandle.InteractableButtons},
            {(int) ServerPackets.matchFound, ClientHandle.MatchFound}
        };
        Debug.Log("Initialized packets.");
    }

    private void Disconnect()
    {
        return;
        if (!isConnected) return;
        isConnected = false;
        tcp.socket.Close();
        if(udp.socket.ToString() != null){
        udp.socket.Close();
        }
        Debug.Log("Disconnected");
    }

    private delegate void PacketHandler(Packet _packet);


    public class TCP
    {
        private bool online;
        private byte[] receiveBuffer;
        private Packet receivedData;
        public TcpClient socket;
        private NetworkStream stream;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            try
            {
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }
            catch (Exception e)
            {
                Debug.Log($"An error occurred during TCP Connection Launch : {e}");
            }
        }


        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected) return;

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null) stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void Disconnect()
        {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                var _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                var _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            var _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0) return true;
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                var _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (var _packet = new Packet(_packetBytes))
                    {
                        var _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() < 4) continue;
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0) return true;
            }

            if (_packetLength <= 1) return true;

            return false;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;
        public UdpClient socket;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (var _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        private void Disconnect()
        {
            instance.Disconnect();
            endPoint = null;
            socket = null;
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null) socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                var _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }

                HandleData(_data);
            }
            catch
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] _data)
        {
            using (var _packet = new Packet(_data))
            {
                var _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (var _packet = new Packet(_data))
                {
                    var _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }
    }
}