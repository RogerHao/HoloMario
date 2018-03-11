using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnetClientBase : MonoBehaviour
{
    private int hostId;
    private int recHostId;
    private int connectionId;
    private int myConnectionId;
    private int channelId;
    private int myReliableChannelId;
    private byte[] recBuffer = new byte[1024];
    private int bufferSize = 1024;
    private int dataSize;
    private byte error;
    // Use this for initialization

    public event EventHandler<UnetConnectionMsg> ConnectionEvent;
    public event EventHandler<UnetConnectionMsg> DisconnectionEvent;
    public event EventHandler<UnetDataMsg> DataEvent;

    void Start () {
	    NetworkTransport.Init();
    }

    public class UnetDataMsg : EventArgs
    {
        public int HostId;
        public int ConnectionId;
        public int ChannelId;
        public string Msg;

        public UnetDataMsg(int hostId, int connectionId, int channelId, string msg)
        {
            HostId = hostId;
            ConnectionId = connectionId;
            ChannelId = channelId;
            Msg = msg;
        }
    }
    public class UnetConnectionMsg : EventArgs
    {
        public int HostId;
        public int ConnectionId;
        public int ChannelId;
        public string ClientIp;
        public int ClientPort;

        public UnetConnectionMsg(int hostId, int connectionId, int channelId, string clientIp, int clientPort)
        {
            HostId = hostId;
            ConnectionId = connectionId;
            ChannelId = channelId;
            ClientIp = clientIp;
            ClientPort = clientPort;
        }
    }

    // Update is called once per frame
    void Update()
    {
        recBuffer = new byte[1024];
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                ConnectionEvent?.Invoke(null,
                    new UnetConnectionMsg(recHostId, connectionId, channelId,"", 0));
                break;
            case NetworkEventType.DataEvent:
                DataEvent?.Invoke(null, new UnetDataMsg(hostId, connectionId, channelId, System.Text.Encoding.UTF8.GetString(recBuffer)));
                break;
            case NetworkEventType.DisconnectEvent:
                DisconnectionEvent?.Invoke(null,
                    new UnetConnectionMsg(recHostId, connectionId, channelId, "", 0));
                break;
        }
    }

    public int ConnectToServer(string ip, int port)
    {
        ConnectionConfig connectionConfig = new ConnectionConfig();
        myReliableChannelId = connectionConfig.AddChannel(QosType.Reliable);
        HostTopology hostTopology = new HostTopology(connectionConfig, 2);
        hostId = NetworkTransport.AddHost(hostTopology);
        return myConnectionId = NetworkTransport.Connect(hostId, ip, port, 0, out error);
    }

    public byte DisconnectToServer()
    {
        NetworkTransport.Disconnect(hostId, myConnectionId, out error);
        return error;
    }

    public byte SendMessageToServer(string msg)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(msg);
        int size = buffer.Length;
        NetworkTransport.Send(hostId, myConnectionId, myReliableChannelId, buffer, size, out error);
        return error;
    }


}
