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
        if (gameManager.state != gameManager.GAMEEND) {
            if (PhotonNetwork.CurrentRoom == null) {
                SceneManager.LoadScene(0);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount < 2) {
                PhotonNetwork.Disconnect();
                print("Randomly Disconnected");
            }
        }
        else {
            if (PhotonNetwork.CurrentRoom == null) {
                SceneManager.LoadScene(0);
            }
        }
    }
    public void Disconnect() 
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected from opponent");
    }

    public void ExitTutorial()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected from opponent");

        SceneManager.LoadScene(0);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        //we may need to change stuff inside the panels

        //Debug.Log($"Disconnected due to {cause}");
        print($"Disconnected due to {cause}");
    }
}
