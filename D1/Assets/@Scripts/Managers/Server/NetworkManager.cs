using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using static Define;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Online : Connected to Master Server");
    }

      public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to Random Room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Offline : Connection Disabled - Try reconnecting...}");
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("There is no empty room, Creating new Room.");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected with Room");
        // 호스트가 방을 참가했던 참가자들도 같은 씬으로 로드됌
        // 참가자들의 씬이 동기화 됌 
        PhotonNetwork.LoadLevel(EScene.GameScene.ToString());
    }

}
