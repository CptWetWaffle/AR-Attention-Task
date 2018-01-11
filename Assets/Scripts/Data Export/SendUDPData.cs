using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class SendUDPData : MonoBehaviour
{/*

    public static bool isConnected;

    public static string timestamp, timestamp_old = String.Empty;
    //static string date = String.Empty;
    public float uptime;

    //UDP client
    public static string IP = "127.0.0.1";  // define in init
    public static int port = 1204;  // define in init 
    public static bool flag = false;
    // "connection" 
    public static IPEndPoint remoteEndPoint;
    public static UdpClient client;

    public static GameObject IKTarget;//IK target

    void Start()
    {
        //connect client
        init();

        
    }

    void FixedUpdate()
    {
        IKTarget = GameObject.FindGameObjectWithTag("hand");

        timestamp = DateTime.UtcNow.Minute.ToString("00") + DateTime.UtcNow.Second.ToString("00") + DateTime.Now.Millisecond.ToString("0000"); //time in min:sec:usec

        if (Application.loadedLevel == 1)
        {
            sendData();
        }


    }


    public static void init()
    {
        // ----------------------------
        // Send
        // ----------------------------

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

        client = new UdpClient();

        // status
        print("Sending to " + IP + " : " + port);

        flag = true;
    }


    // sendData
    public static void sendString(string message)
    {
        try
        {
            if (message != "")
            {
                // UTF8 encoding to binary format.
                byte[] data = Encoding.UTF8.GetBytes(message);
                //byte[] data = Encoding.ASCII.GetBytes(message); asci

                // Send the message to the remote client.
                client.Send(data, data.Length, remoteEndPoint);
            }
        }

        catch (Exception err)
        {
            UnityEngine.Debug.Log(err.ToString());
        }
    }//end of sendString


    void sendData()
    {

        //inverse kinematics target position
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]iktarget,position," + IKTarget.transform.position.x.ToString() + "," + IKTarget.transform.position.y.ToString() + "," + IKTarget.transform.position.z.ToString() + ";");
        //mouse position
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]mouse,position," + Input.mousePosition.x.ToString() + "," + Input.mousePosition.y.ToString() + ";");
        //fullScore - correct answers
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]fullScore,var," + Scoring.fullScore.ToString() + ";");

        //Wrong
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]wrong,var," + Scoring.GetError().ToString() + ";");

        //MouseClicks
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]mouseClicks,var," + CSVDataSave.mouseClicks.ToString() + ";");

        //LevelNumber
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]levelNumber,var," + SpawnTiles.levelName.ToString() + ";");

        //Score - %
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]score,var," + Scoring.GetScore().ToString() + ";");

        //texture currently being selected
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]texture,var," + Collision_Detection.selectedTexture + ";");


        //Textures of the cubes on the table
        string newLine = SpawnTiles.texturesToExport[0];

        for (int i = 1; i < SpawnTiles.texturesToExport.Count; i++)
        {
            newLine = newLine + "," + SpawnTiles.texturesToExport[i];
        }
        sendString("[$]tracking,[$$]" + "attentiontask" + ",[$$$]texturesOnTheTable,var," + newLine + ";");
    }





    void OnApplicationQuit()
    {
        if (flag)
        {
            client.Close();
        }
    }


*/
}
