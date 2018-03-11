using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UNETClient : MonoBehaviour {

    //    public InputField IpInput;
    //    public InputField PortInput;
    //    private int port;
    //    public InputField MsgInput;
    //    public Text MsgRevText;
    //
    //    public Text Log;

    public GameObject Mario;
    public GameObject ArImage;
    private float _speed = 1.0f;
    private float _scale = 1.0f;

//    public Image[] GestImages;

    private string LogString { get; set; }
    private UnetClientBase UnetClientBase;


    // Use this for initialization
    void Start () {
        UnetClientBase = gameObject.GetComponent<UnetClientBase>();

        UnetClientBase.ConnectionEvent += UnetClientBase_ConnectionEvent;
        UnetClientBase.DisconnectionEvent += UnetClientBase_DisconnectionEvent;
        UnetClientBase.DataEvent += UnetClientBase_DataEvent;

        StartCoroutine(connectToServer());
    }

    private IEnumerator connectToServer()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("start Connect");
        ConnectedToServer();
    }

    private void UnetClientBase_DataEvent(object sender, UnetClientBase.UnetDataMsg e)
    {
        
        if (string.IsNullOrEmpty(e.Msg)) return;
//        if (e.Msg.Equals("0"))
//        {
//            Debug.Log($"0");
//        }
//        else if (e.Msg.Equals("w"))
//        {
//            Debug.Log($"w");
//        }
//        else if (e.Msg.Equals("s"))
//        {
//            Debug.Log($"s");
//        }
//        else
//        {
//            Debug.Log($"default");
//        }
        int commandNum;
        int.TryParse(e.Msg, out commandNum);
        switch (commandNum)
        {
            case 0:
                Mario.transform.parent = Mario.transform.parent == null ? ArImage.transform:null; 
                break;
            case 1:
                Mario.transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * _speed);
                break;
            case 2:
                Mario.transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * _speed);
                break;
            case 3:
                Mario.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * _speed);
                break;
            case 4:
                Mario.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * _speed);
                break;
            case 5:
//                float.TryParse(e.Msg, out _scale);
                Mario.transform.localScale = Mario.transform.localScale * 1.1f;
                break;
            case 6:
//                float.TryParse(e.Msg, out _scale);
                Mario.transform.localScale = Mario.transform.localScale * 0.9f;
                break;
            default:
                float.TryParse(e.Msg, out _speed);
                break;
        }
    }

    private void UnetClientBase_DisconnectionEvent(object sender, UnetClientBase.UnetConnectionMsg e)
    {
//        LogString = $"Server {e.ConnectionId} Disconnect!";
    }

    private void UnetClientBase_ConnectionEvent(object sender, UnetClientBase.UnetConnectionMsg e)
    {
//        LogString = $"Server {e.ConnectionId} Connect!";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ConnectedToServer()
    {
//        UnetClientBase.ConnectToServer(IpInput.text,port);
        UnetClientBase.ConnectToServer("192.168.0.146",9696);
    }

    public void DisConnectedToServer()
    {
        UnetClientBase.DisconnectToServer();
    }

    public void SendMessageToServer()
    {
//        UnetClientBase.SendMessageToServer(MsgInput.text);
    }

//    private string GetIp()
//    {
//        string name = Dns.GetHostName();
//        IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
//        foreach (IPAddress ipa in ipadrlist)
//        {
//            if (ipa.AddressFamily == AddressFamily.InterNetwork)
//            {
//                return ipa.ToString();
//            }
//        }
//        return "";
//    }
}
