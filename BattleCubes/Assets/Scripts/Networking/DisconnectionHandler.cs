using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class DisconnectionHandler : MonoBehaviourPunCallbacks {

    public void Update() {
        if (PhotonNetwork.CurrentRoom == null) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount < 2) { 
            PhotonNetwork.Disconnect();
            print("Randomly Disconnected");
        }
    }
    public void Disconnect() 
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected from opponent");
    }
}
