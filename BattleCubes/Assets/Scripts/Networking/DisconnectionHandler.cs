using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class DisconnectionHandler : MonoBehaviourPunCallbacks {

    GameManager gameManager;

    public void Start() {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
    }

    public void Update() {
        if(gameManager.state != gameManager.GAMEEND){
            if (PhotonNetwork.CurrentRoom == null) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount < 2) { 
                PhotonNetwork.Disconnect();
                print("Randomly Disconnected");
            }
        }
    }
    public void Disconnect() 
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected from opponent");
    }
}
