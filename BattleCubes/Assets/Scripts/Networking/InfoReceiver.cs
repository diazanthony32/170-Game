using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;


public class InfoReceiver : MonoBehaviourPun {

    [SerializeField] GameManager gameManager;

    public void OnEnable() => PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    public void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    public void Start() {
        //oc = GetComponent<objectClicker>();
        //gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        //enemyCanvas = gameCanvas.transform.Find("Enemy").gameObject;
    }

    public void OnEvent(EventData photonEvent) {
        byte eventCode = photonEvent.Code;

        //checks for enemy status
        if (eventCode == 1) {
            string[] content = (string[])photonEvent.CustomData;

            gameManager.GetEnemyCanvas().transform.Find("EnemyName").gameObject.GetComponent<TextMeshProUGUI>().text = content[0];

            //var enemyUnitDisplay = oc.enemyStatDisplay.GetChild(3);
            //int unitCount;
            //int.TryParse(content[2], out unitCount);

            //oc.currentNumUnits = unitCount;

            //for (int i = 0; i < unitCount; i++) {

            //    enemyUnitDisplay.GetChild(i).GetComponent<Image>().enabled = true;
            //}
            //for (int y = (11 - unitCount); y >= 0; y--) {
            //    enemyUnitDisplay.GetChild(11 - y).GetComponent<Image>().enabled = false;
            //}
            //}
        }

        //changeTimer for timer
        else if (eventCode == 2) {
            int[] content = (int[])photonEvent.CustomData;

            gameManager.SetRemainingTimeText(content[0]);
            gameManager.SetRoundCountText(content[1]);
            gameManager.SetState(content[2]);
        }
        //notifications
        else if (eventCode == 3) {
            int content = (int)photonEvent.CustomData;

            gameManager.ReadNotification(content);
        }
        //do throwdown
        else if (eventCode == 4) {
            gameManager.StartThrowDown();
        }
        else if (eventCode == 5) {
            gameManager.GetOutOfSetUp();
        }
        else if (eventCode == 6) {
            gameManager.SetEnemyReady();
        }
        else if (eventCode == 7) {
            gameManager.ResetReadies();
        }
        else {
            print("No notification");
        }
    }
    //GameManager gameManager;
    //objectClicker oc;
    //public int counter = 0;

    //public void OnEnable() => PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    //public void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    //public void Start() {
    //    oc = GetComponent<objectClicker>();
    //    gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    //}

    //public void OnEvent(EventData photonEvent) {
    //    byte eventCode = photonEvent.Code;

    //    //Debug.Log("info received");

    //    // checks if the cube wants to be moved
    //    if (eventCode == 1) {
    //        Debug.Log("Turning Info");
    //        string content = (string)photonEvent.CustomData;

    //        oc.turnCube.AssignArrow(content);
    //    }

    //    //checks for changes in units
    //    if (eventCode == 2) {
    //        Debug.Log("Unit Placement Info");
    //        string[] content = (string[])photonEvent.CustomData;

    //        oc.unitHandler.AdjustEnemyUnits(content);
    //    }

    //    //checks for attacks
    //    if (eventCode == 3) {
    //        Debug.Log("Attack Info");
    //        string[] content = (string[])photonEvent.CustomData;

    //        oc.attackHandler.TranslateAttack(content);
    //    }

    //    //checks for attacks
    //    if (eventCode == 4) {
    //        Debug.Log("Removing Action");
    //        int actionNum = (int)photonEvent.CustomData;

    //        //oc.attackHandler.TranslateAttack(content);

    //        if(oc.actionList != null){

    //            int actionCount = -1;
    //            foreach (List<string> action in oc.actionList) {

    //                actionCount++;
    //                //p1.Add(action);

    //                if(actionCount == actionNum){

    //                    print("Reseting Action");

    //                    oc.actionList.Remove(oc.actionList[actionNum]);

    //                    //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
    //                    // var sprite = Resources.Load<Sprite>("BC_UI_Blank");
    //                    // oc.actionStorageUI[actionNum].GetComponent<Image>().sprite = sprite;

    //                    break;

    //                }
    //                else{
    //                    print("Nothing to Reset");
    //                }

    //            }
    //        }
    //        else{
    //            print("ERROR");
    //        }
    //    }

    //    //checks for player status
    //    if (eventCode == 10) {
    //        //Debug.Log("Player stats info received");
    //        string[] content = (string[])photonEvent.CustomData;

    //        //for (int i = 0; i < 3; i++) { 
    //            //oc.enemyStatDisplay.GetChild(i).GetComponent<TextMeshProUGUI>().text = content[i]; 
    //            //actionPointText.text = "Points: " + actionPoints.ToString();
    //        oc.enemyStatDisplay.GetChild(1).GetComponent<TextMeshProUGUI>().text = (content[0]);
    //        //unitText.text = "Units: " + currentNumUnits.ToString();

    //        //oc.enemyStatDisplay.GetChild(2).GetComponent<TextMeshProUGUI>().text = (content[2] + " :Units");
    //        var enemyUnitDisplay = oc.enemyStatDisplay.GetChild(3);
    //        int unitCount;
    //        int.TryParse(content[2], out unitCount);

    //        oc.currentNumUnits = unitCount;

    //        for (int i = 0; i < unitCount; i++){

    //            enemyUnitDisplay.GetChild(i).GetComponent<Image>().enabled = true;
    //        }
    //        for(int y = (11 - unitCount); y >= 0 ; y--){
    //            enemyUnitDisplay.GetChild(11-y).GetComponent<Image>().enabled = false;
    //        }
    //        //}
    //    }

    //    

    //    //gets cube rotation
    //    if (eventCode == 12) {
    //        float[] content = (float[])photonEvent.CustomData;

    //        oc.gameManager.CheckSync(content, oc.transform.GetChild(0).transform);
    //    }

    //    //gets setBasePos
    //    if (eventCode == 13) {
    //        bool content = (bool)photonEvent.CustomData;

    //        gameManager.Players[0].turnCube.SetBasePos();
    //    }

    //    //actionAry received
    //    if (eventCode == 20) {
    //        //Debug.LogError("ActionAry Info");
    //        string[] content = (string[])photonEvent.CustomData;

    //        //putting actions on enemy player
    //        List<string> action = new List<string>();
    //        action.Add(content[0]);
    //        if (content.Length > 1) {
    //            action.Add(content[1]);
    //            action.Add(content[2]);
    //        }

    //        //oc.gameManager.Players[1].actionList.Add(action);
    //        oc.actionList.Add(action);
    //    }
    //    //nextRound
    //    if (eventCode == 21) {
    //        if (!PhotonNetwork.LocalPlayer.IsMasterClient) {
    //            gameManager.nextRound();
    //        }
    //    }
    //    //doThrowDown
    //    if (eventCode == 22) {
    //        if (!PhotonNetwork.LocalPlayer.IsMasterClient) {
    //            print("running throwDown from eventCode---------------------------");
    //            gameManager.DoThrowDown();
    //        }
    //    }
    //    //A player readyed up
    //    if (eventCode == 23) {
    //        oc.ready = true;
    //    }
    //    //Reset Readies
    //    if (eventCode == 24) {
    //        if (!PhotonNetwork.LocalPlayer.IsMasterClient) {
    //            gameManager.ResetReadies();
    //        }
    //    }
    //}
}
