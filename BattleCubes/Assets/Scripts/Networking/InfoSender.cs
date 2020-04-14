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



    //public void SendCubeRotation(float[] content) {
    //    byte evCode = 12;

    //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    //    SendOptions sendOptions = new SendOptions { Reliability = true };
    //    PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
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
