using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class InfoSender : MonoBehaviourPun {
    //public GameManager gameManager;

    public void SendPlayerStats(string[] content) {
        byte evCode = 1;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void SendGameStatus(int[] content) {
        byte evCode = 2;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void SendNotification(int content) {
        byte evCode = 3;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void SendStartThrowDown() {
        byte evCode = 4;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, true, raiseEventOptions, sendOptions);
    }
    public void SendGetOutOfSetup() {
        byte evCode = 5;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, true, raiseEventOptions, sendOptions);
    }
    public void SendReady() {
        byte evCode = 6;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, true, raiseEventOptions, sendOptions);
    }
    public void SendResetRound() {
        byte evCode = 7;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, true, raiseEventOptions, sendOptions);
    }
    //public void SendTurnOnMainScreen() {
    //    byte evCode = 8;

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, true, raiseEventOptions, sendOptions);
    //}
    //public void SendTurnDirection(string host, string client) {
    //    byte evCode = 8;

    //    string[] content = new string[] {host, client};

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    //}
    public void SendCubeInfo(string[] content) {
        byte evCode = 9;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void SendActionListArray(string[][] content) {
        byte evCode = 10;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void SendPlayerTurningDir(string content) {
        byte evCode = 11;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void SendEnemyTurningDir(string content) {
        byte evCode = 12;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void SendCubeRotation(float[] content) {
        byte evCode = 13;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void SendUnitPlacement(string[] content) {
        byte evCode = 14;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void RemoveUnitPlacement(string[] content) {
        byte evCode = 15;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    //not used
    public void SendCubePrefs(string[] content) {
        byte evCode = 16;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void SendPlayerAttackInfo(string[] content) {
        byte evCode = 17;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void SendEnemyAttackInfo(string[] content) {
        byte evCode = 18;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }
    public void SendThrowdownHighlight(int content) {
        byte evCode = 19;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void SendGameOver(string content) {
        byte evCode = 20;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }


    //public void SendTurningInfo(string content) {

    //    //sets event code; 1 means turnCube
    //    //Debug.Log("sending turning info...");
    //    byte evCode = 1;
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);

    //    //Debug.Log("turning Info sent");
    //}

    //public void SendUnitInfo(string[] content) {

    //    //sets event code; 2 means unit info changes
    //    //Debug.Log("sending unit placement info...");
    //    byte evCode = 2;
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);

    //    //Debug.Log("unit placement Info sent");
    //}

    //public void SendAttackInfo(string[] content) {

    //    //sets event code; 2 means unit info changes
    //    //Debug.Log("sending attack info...");
    //    byte evCode = 3;
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);

    //    //Debug.Log("attack Info sent");
    //}

    //public void RemoveActionInfo(int content) {

    //    //sets event code; 2 means unit info changes
    //    //Debug.Log("sending attack info...");
    //    byte evCode = 4;
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);

    //    //Debug.Log("attack Info sent");
    //}





    //public void SendSetBasePos() {
    //    byte evCode = 13;
    //    bool content = true;

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    //}

    ////this is not the full array, just a piece of it
    //public void SendActionAry(string[] content) {
    //    byte evCode = 20;

    //    //Debug.Log("sending array piece of info...");
    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);

    //    //Debug.Log("array piece of Info sent");
    //}
    //public void SendNextRound() {
    //    byte evCode = 21;
    //    bool content = true;

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    //}
    //public void SendThrowDown() {
    //    byte evCode = 22;
    //    bool content = true;

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    //}
    //public void SendReadyUpSat() { //only used by non-master clients
    //    byte evCode = 23;
    //    bool content = true;

    //    if (!PhotonNetwork.LocalPlayer.IsMasterClient) {
    //        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //        SendOptions sendOptions = new SendOptions { Reliability = true };
    //        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    //    }
    //}
    //public void SendResetReadies() {
    //    byte evCode = 24;
    //    bool content = true;

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    //}


}
