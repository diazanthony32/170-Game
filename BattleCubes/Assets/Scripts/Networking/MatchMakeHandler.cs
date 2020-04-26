﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MatchMakeHandler : MonoBehaviourPunCallbacks 
{
    [SerializeField] private TMP_InputField roomNameInputField = null;
    [SerializeField] TextMeshProUGUI searchText;
    private bool isConnecting = false;
    private const string GameVersion = "0.1.4"; //Change with the gameVersion
    private const int MaxPlayersPerRoom = 2;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;

        searchText = GameObject.FindGameObjectWithTag("searchText").GetComponent<TextMeshProUGUI>();
    }

    public void FindOpponent() 
    {
        isConnecting = true;
        searchText.text = "Searching...";

        if (PhotonNetwork.IsConnected) 
        {
            //PhotonNetwork.JoinRandomRoom();
            //RoomOptions roomOptions = new RoomOptions();
            //roomOptions.IsVisible = false;

            //PhotonNetwork.JoinOrCreateRoom("nose", roomOptions, TypedLobby.Default);
            if (roomNameInputField.text != null) {
                PhotonNetwork.JoinRoom(roomNameInputField.text, null);
            }
            else {
                PhotonNetwork.JoinRandomRoom();
            }

        }
        else 
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster() {
        Debug.Log("connected to master");
        if (isConnecting) {
            //PhotonNetwork.JoinRandomRoom();
            if (roomNameInputField.text != null) {
                PhotonNetwork.JoinRoom(roomNameInputField.text, null);
            }
            else {
                PhotonNetwork.JoinRandomRoom();
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        //we may need to change stuff inside the panels

        Debug.Log($"Disconnected due to {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("No clients waiting, creating new room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log("No clients waiting, creating new room");

        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    //this what player 2 does
    public override void OnJoinedRoom() {
        Debug.Log("client joined the room succesfully");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount != MaxPlayersPerRoom) {
            searchText.text = "Waiting for Opponent...";

            Debug.Log("client is waiting for opponent");
        }
        else {
            searchText.text = "Opponent found!";
            Debug.Log("match is ready to begin");

            // Loading Levels
            //LevelLoader levelLoader = GameObject.FindGameObjectWithTag("levelLoader").GetComponent<LevelLoader>();
            //levelLoader.FadeOutLevel("MainScene");
            //PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //this what player 1 does
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom) {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            searchText.text = "Opponent found!";
            Debug.Log("Match is ready to begin");

            // Loading Levels
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    //public void ChangeRoomName(string rName) {
    //    roomName = rName;
    //}

    public void GoToTutorialScene() {
        PhotonNetwork.LoadLevel("Tutorial");
    }

    public void GoToMenuScene() {
        PhotonNetwork.LoadLevel("MenuScene");
    }

    public void CancelSearch() {
        PhotonNetwork.Disconnect();
    }

}
