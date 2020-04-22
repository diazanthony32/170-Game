﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] InfoSender infoSender;
    [SerializeField] InfoReceiver infoReceiver;
    [Space(10)]
    [SerializeField] GameObject enemyCanvas;
    [SerializeField] GameObject playerCanvas;
    [SerializeField] GameObject setupCanvas;
    [SerializeField] GameObject mainScreenCanvas;
    [SerializeField] GameObject rotationCanvas;
    [SerializeField] GameObject attackCanvas;
    [SerializeField] GameObject outtaTimeCanvas;
    [SerializeField] GameObject outtaSetupCanvas;
    [Space(10)]
    [SerializeField] GameObject enemyActionList;
    [SerializeField] GameObject playerActionList;
    [Space(10)]
    [SerializeField] TextMeshProUGUI roundCountText;
    [SerializeField] TextMeshProUGUI remainingTimeText;
    [Space(10)]
    [SerializeField] GameObject preventClick;
    [SerializeField] AttackHandler attackHandler;

    public GameObject playerCubePosition;
    public GameObject enemyCubePosition;

    public GameObject playerTargettingSystem;
    public GameObject enemyTargettingSystem;

    List<string> playerInfo;
    bool[] readies = {false, false};

    //states
    public readonly int SETUP = 0;
    public readonly int PLAN = 1;
    public readonly int THROWDOWN = 2;
    public readonly int GAMEEND = 3;
    int state;

    //notifications
    const int TIME_UP = 0;
    const int PLAYERS_READY = 1;

    //timeming
    public readonly float ROUND_TIME = 30;
    public readonly float SET_UP_TIME = 150;
    float remainingTime;
    bool timeStopped = false;

    //corrutine stuff
    bool setupCorrutineRunning = false;

    //round management
    int roundCount = 0;

    //player points
    int roundsForPointIncrease = 5;
    int pointsPerRound = 3;
    int actionPoints = 100;

    //unit points
    public int totalUnitPoints = 12;
    public int remainingUnitPoints = 12;

    public int towerCount = 0;
    public int unitCount = 0;

    public string[] enemyCubeInfo;
    public string[] cubeInfo;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;

    void Start() {
        cubeInfo = new string[] {PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor")};

        if(PlayerPrefs.HasKey("MusicVolume")){
            audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        state = SETUP;

        playerInfo = new List<string>();
        playerInfo.Add(PlayerPrefs.GetString("PlayerName"));

        infoSender.SendPlayerStats(ListToStringArray(playerInfo));
        //infoSender.SendCubePrefs(ListToStringArray(playerInfo));

        playerCanvas.transform.Find("PlayerName").gameObject.GetComponent<TextMeshProUGUI>().text = playerInfo[0];
        playerCanvas.transform.Find("PlayerUnitCount").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();

        SpawnPlayerCube(cubeInfo);
        infoSender.SendCubeInfo(cubeInfo);

        setupCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(true);

        playerTargettingSystem = GameObject.FindGameObjectWithTag("PlayerCubePosition").transform.GetChild(1).gameObject;
        // playerTargettingSystem.SetActive(false);
        // attackHandler.TurnOffTargetting();


        remainingTime = SET_UP_TIME;
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            if (!timeStopped) {
                CheckForReady();
                RunTimer();
            }
            //else {
            //    if (throwDownStart) {
            //        ResetReadies();
            //        if (roundCount > 0) {
            //            DoThrowDown();
            //        }
            //        throwDownStart = false;
            //    }
            //    if (throwDownStopped) {
            //        RestartTime();
            //        nextRound();
            //        throwDownStopped = false;
            //        throwDownStart = true;
            //    }
            //}
        }
        //if (roundCount == 0) {
        //    roundText.text = "Setup";
        //}
        //else if (showEnemy == false && roundCount != 0) {
        //    ShowEnemyStuff();
        //}
        //else {
        //    roundText.text = "Round " + ConvertNumToText(roundCount);
        //}
        // UpdateUI();
    }

    // void UpdateUnitCount(){
    //     playerCanvas.transform.Find("PlayerUnitCount").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();
    // }

    public void CheckForReady() {
        if (readies[0] && readies[1]) {
            remainingTime = 0;
            readies[0] = false;
            readies[1] = false;
            //ResetRound();
            //infoSender.SendResetRound();
            //if (state != SETUP) {
            //    remainingTime = 0;
            //}
            //else {
            //    GetOutOfSetUp();
            //}
        }
    }

    //public IEnumerator setupCorrutine() {
    //    setupCorrutineRunning = true;
    //    remainingTime = SET_UP_TIME;
    //    yield return new WaitForSeconds(SET_UP_TIME);
    //    setupCorrutineRunning = false;

    //    GetOutOfSetUp();
    //}

    public void TurnOnTargetting(){
        //print("Turning on targets");

        for(int j = 0; j < enemyCubePosition.transform.GetChild(1).childCount-1; j++){
            enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
            // targetsystems.SetActive(false);
            //print("turning enemy targets on");
        }

        for(int i = 0; i < playerCubePosition.transform.GetChild(1).childCount-1; i++){
            playerCubePosition.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
            // targetsystems.SetActive(false);
            //print("turning player targets on");

        }

        //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

    }
    public void SetEnemyCubePrefs(string[] cubeInfo){
        enemyCubeInfo = cubeInfo;
    }
    public void TurnOffTargetting(){
        //print("Turning off targets");


        for(int j = 0; j < enemyCubePosition.transform.GetChild(1).childCount-1; j++){
            enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(false);
            //print("turning enemy targets off");
            // targetsystems.SetActive(false);
        }

        for(int i = 0; i < playerCubePosition.transform.GetChild(1).childCount-1; i++){
            playerCubePosition.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            // targetsystems.SetActive(false);
            //print("turning player targets off");

        }

        //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

    }

    public IEnumerator GetOutOfSetUp() {
        print("getting out of setup");

        setupCanvas.SetActive(false);
        enemyActionList.SetActive(true);
        playerActionList.SetActive(true);
        enemyCanvas.SetActive(true);

        rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(false);

        outtaSetupCanvas.GetComponent<TweenController>().Notify();

        yield return new WaitForSeconds(3f);
        infoSender.SendCubeRotation(GetQuatComponentAry(playerCubePosition.transform.GetChild(0)));
        
        mainScreenCanvas.SetActive(true);

        remainingTime = ROUND_TIME;
        timeStopped = false;

        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);

        //infoSender.SendCubePrefs(cubeInfo);

        ResetRound();
    }

    public IEnumerator StartThrowDown() {
        outtaTimeCanvas.GetComponent<TweenController>().Notify();

        preventClick.SetActive(true);
        state = THROWDOWN;
        mainScreenCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        rotationCanvas.SetActive(false);
        rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(false);

        yield return new WaitForSeconds(1.5f);
        playerCubePosition.transform.GetChild(0).GetComponent<RotateCube>().SnapToBaseRotation();
        yield return new WaitForSeconds(1.5f);
        TurnOnTargetting();

        //playerActionList.GetComponent<ActionStorage>().PrepareActionListForSend()
        if (playerActionList.GetComponent<ActionStorage>().GetActionListCount() > 0) {
            infoSender.SendActionListArray(playerActionList.GetComponent<ActionStorage>().PrepareActionListForSend());
        }
        
        yield return new WaitForSeconds(0.5f);

        playerActionList.GetComponent<ActionStorage>().resetUIPulses();
        enemyActionList.GetComponent<ActionStorage>().resetUIPulses();

        playerActionList.GetComponent<TweenController>().slideXOver((Screen.width * -0.034f));
        enemyActionList.GetComponent<TweenController>().slideXOver((Screen.width * 0.034f));

        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            StartCoroutine(DoThrowdown());
        }
    }
    
    private void RunTimer() {
        int intTIME = 0;

        if (remainingTime >= 0) {
            remainingTime -= Time.deltaTime;
        }
        else {
            timeStopped = true;

            //print(state);
            if (state == PLAN) {
                infoSender.SendNotification(TIME_UP);
                infoSender.SendStartThrowDown();
                StartCoroutine(StartThrowDown());
                //StartThrowDown();
            }
            else if (state == SETUP) {
                infoSender.SendGetOutOfSetup();
                StartCoroutine(GetOutOfSetUp());
            }
        }
        intTIME = Mathf.CeilToInt(remainingTime);
        remainingTimeText.text = intTIME.ToString();
        infoSender.SendGameStatus(new int[] { intTIME, roundCount, state });
    }

    public void HighlightThrowdownAction(int i){
        playerActionList.GetComponent<ActionStorage>().resetUIPulses();
        enemyActionList.GetComponent<ActionStorage>().resetUIPulses();

        // playerActionList.GetComponent<ActionStorage>().resetUIPulses();
        playerActionList.transform.GetChild(i).GetComponent<TweenController>().PulseHighlight();
        enemyActionList.transform.GetChild(i).GetComponent<TweenController>().PulseHighlight();
    }

    IEnumerator DoThrowdown() {
        //print("inside CORRUTINE");
        // attackHandler.TurnOnTargetting();
        // playerActionList.GetComponent<TweenController>().slideXOver(-65.0f);
        // enemyActionList.GetComponent<TweenController>().slideXOver(65.0f);


        yield return new WaitForSeconds(3.5f);

        // attackHandler.TurnOnTargetting();

        string[] host;
        string[] client;
        for (int i = 0; i < 5; i++) {
            HighlightThrowdownAction(i);
            infoSender.SendThrowdownHighlight(i);
            // playerActionList.GetComponent<ActionStorage>().resetUIPulses();
            // enemyActionList.GetComponent<ActionStorage>().resetUIPulses();

            // // playerActionList.GetComponent<ActionStorage>().resetUIPulses();
            // playerActionList.transform.GetChild(i).GetComponent<TweenController>().PulseHighlight();
            // enemyActionList.transform.GetChild(i).GetComponent<TweenController>().PulseHighlight();
            // transform.GetChild(actionList.Count - 1).GetComponent<TweenController>().PulseHighlight();
            // enemyActionList.GetComponent<ActionStorage>().resetUIPulses();
            //print(i);

            //both players have actions
            if (playerActionList.GetComponent<ActionStorage>().GetActionListCount() > i && enemyActionList.GetComponent<ActionStorage>().GetActionListCount() > i) {
                yield return new WaitForSeconds(0.5f);
                print("both players have actions");
                host = playerActionList.GetComponent<ActionStorage>().GetAt(i);
                client = enemyActionList.GetComponent<ActionStorage>().GetAt(i);

                if (host[0] == client[0]) {
                    if (host[0] == "rotate") {
                        print("Players both rotate");

                        //rotate both
                        TranslateRotatePlayerCube(host[1]);
                        TranslateRotateEnemyCube(client[1]);

                        infoSender.SendPlayerTurningDir(host[1]);
                        infoSender.SendEnemyTurningDir(client[1]);

                        //playerCubePosition.transform.GetChild(0).GetComponent<RotateCube>().AssignDirection(host[1]);
                        //enemyCubePosition.transform.GetChild(0).GetComponent<RotateCube>().AssignDirection(client[1]);
                    }
                    else if (host[0] == "attack") {
                        //both attack
                        print("Players both attack");

                        attackHandler.DoAttack("Host", host);
                        attackHandler.DoAttack("Client", client);

                        infoSender.SendPlayerAttackInfo(host);
                        infoSender.SendEnemyAttackInfo(client);
                    }
                    yield return new WaitForSeconds(3);
                }
                else {
                    yield return new WaitForSeconds(0.5f);
                    if (host[0] == "rotate") {
                        print("Host rotate, client attack");

                        //host rotates, client then attacks
                        TranslateRotatePlayerCube(host[1]);
                        infoSender.SendPlayerTurningDir(host[1]);
                        
                        yield return new WaitForSeconds(3);

                        attackHandler.DoAttack("Client", client);
                        infoSender.SendEnemyAttackInfo(client);
                        //infosender
                        yield return new WaitForSeconds(3);

                    }
                    else if (host[0] == "attack") {
                        yield return new WaitForSeconds(0.5f);
                        print("Client rotate, Host attack");

                        //rotate client , host then attacks
                        TranslateRotateEnemyCube(client[1]);
                        infoSender.SendEnemyTurningDir(client[1]);
                        
                        yield return new WaitForSeconds(3);

                        attackHandler.DoAttack("Host", host);
                        infoSender.SendPlayerAttackInfo(host);

                        //infosender
                        yield return new WaitForSeconds(3);

                    }
                    yield return new WaitForSeconds(3);
                }
            }
            //only host has actions
            else if (playerActionList.GetComponent<ActionStorage>().GetActionListCount() > i) {
                yield return new WaitForSeconds(0.5f);
                print("host has actions");
                host = playerActionList.GetComponent<ActionStorage>().GetAt(i);

                if (host[0] == "rotate") {
                    print("Host rotate");

                    //host rotates
                    TranslateRotatePlayerCube(host[1]);
                    infoSender.SendPlayerTurningDir(host[1]);
                }
                else if (host[0] == "attack") {
                    print("Host attack");
                    attackHandler.DoAttack("Host", host);
                    infoSender.SendPlayerAttackInfo(host);

                    //infosender
                }
                yield return new WaitForSeconds(3);
            }
            //only client has actions
            else if (enemyActionList.GetComponent<ActionStorage>().GetActionListCount() > i) {
                yield return new WaitForSeconds(0.5f);
                print("client has actions");
                client = enemyActionList.GetComponent<ActionStorage>().GetAt(i);

                if (client[0] == "rotate") {
                    print("client rotate");

                    //client rotates
                    TranslateRotateEnemyCube(client[1]);
                    infoSender.SendEnemyTurningDir(client[1]);
                }
                else if (client[0] == "attack") {
                    print("client attack");
                    //print(client[1] + " "+ client[2] + " "+ client[3]);

                    attackHandler.DoAttack("Client", client);
                    infoSender.SendEnemyAttackInfo(client);

                    //infosender
                }
                yield return new WaitForSeconds(3);
            }
            //no more actions
            else {
                print("no more actions");
                break;
            }
            yield return new WaitForSeconds(1);
        }

        // attackHandler.TurnOffTargetting();

        timeStopped = false;
        remainingTime = ROUND_TIME;
        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);

        mainScreenCanvas.SetActive(true);

        ResetRound();
        infoSender.SendResetRound();
    }

    public void TranslateRotatePlayerCube(string direction) {
        playerCubePosition.transform.GetChild(0).GetComponent<RotateCube>().RequestRotation(direction);
    }
    public void TranslateRotateEnemyCube(string direction) {
        enemyCubePosition.transform.GetChild(0).GetComponent<RotateCube>().RequestRotation(direction);
    }


    public void TranslateAttackOnEnemyCube(string[] array) {
        attackHandler.DoAttack("Client", array);
    }
    public void TranslateAttackOnPlayerCube(string[] array) {
        attackHandler.DoAttack("Host", array);
    }

    // void CompareActions(string[] host, string[] client) {
    //     if (host[0] == client[0]) {
    //         if (host[0] == "rotate") {
    //             //rotate both
    //         }
    //         else if (host[0] == "attack") {
    //             //both attack
    //         }
    //     }
    //     else {
    //         if (host[0] == "rotate") {
    //             //host rotates, client then attacks
    //         }
    //         else if (host[0] == "attack") {
    //             //rotate client , host then attacks
    //         }
    //     }
    // }

    string[] ListToStringArray(List<string> l) {
        string[] tmp;

        if (l != null) {
            tmp = new string[l.Count];
            for (int i = 0; i < l.Count; i++) {
                tmp[i] = l[i];
            }
            return tmp;
        }
        return null;
    }

    public void ReadNotification(int notification) {
        if (notification == TIME_UP) {
            outtaTimeCanvas.GetComponent<TweenController>().Notify();
        }
    }

    public void ReadyUp() {
        readies[0] = true;
        infoSender.SendReady();
    }

    public void ResetRound() {
        readies[0] = false;
        readies[1] = false;

        print("Reseting round!!!");

        playerCubePosition.transform.GetChild(0).GetComponent<RotateCube>().SetBasePos();

        //print(roundCount % roundsForPointIncrease);
        AddActionPoints(pointsPerRound);

        if (state != SETUP) {
            playerActionList.GetComponent<TweenController>().slideXOver((Screen.width * 0.034f));
            enemyActionList.GetComponent<TweenController>().slideXOver((Screen.width * -0.034f));

            playerActionList.GetComponent<ActionStorage>().ClearActionList();
            enemyActionList.GetComponent<ActionStorage>().ClearActionList();
            TurnOffTargetting();
        }
        mainScreenCanvas.SetActive(true);
        
        preventClick.SetActive(false);

        state = PLAN;
    }

    void SpawnPlayerCube(string[] cubeInfo) {
        playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        GameObject cube = Instantiate(Resources.Load<GameObject>(cubeInfo[0] + "/" + cubeInfo[1] + "/Cube"));
        cube.transform.position = playerCubePosition.transform.position;
        cube.transform.rotation = playerCubePosition.transform.rotation;
        cube.transform.SetParent(playerCubePosition.transform);

        SpawnCubeTargetingSystem("PlayerCubePosition");

        playerTargettingSystem = GameObject.FindGameObjectWithTag("PlayerCubePosition").transform.GetChild(1).gameObject;
        //playerTargettingSystem.SetActive(false);
    }
    public void SpawnEnemyCube(string[] cubeInfo) {
        enemyCubePosition = GameObject.FindGameObjectWithTag("EnemyCubePosition");
        enemyCubeInfo = cubeInfo;

        GameObject cube = Instantiate(Resources.Load<GameObject>(cubeInfo[0] + "/" + cubeInfo[1] + "/Cube"));
        cube.transform.position = enemyCubePosition.transform.position;
        cube.transform.rotation = enemyCubePosition.transform.rotation;
        cube.transform.SetParent(enemyCubePosition.transform);

        SpawnCubeTargetingSystem("EnemyCubePosition");
        // enemyCubeInfo = cubeInfo;

        enemyTargettingSystem = GameObject.FindGameObjectWithTag("EnemyCubePosition").transform.GetChild(1).gameObject;
        //enemyTargettingSystem.SetActive(false);
    }
    void SpawnCubeTargetingSystem(string tag){
        GameObject playerCubePosition = GameObject.FindGameObjectWithTag(tag);

        GameObject cubeTargeting = Instantiate(Resources.Load<GameObject>("MainCubePrefab/CubeTargeting"));
        cubeTargeting.transform.position = playerCubePosition.transform.position;
        cubeTargeting.transform.rotation = playerCubePosition.transform.rotation;
        cubeTargeting.transform.SetParent(playerCubePosition.transform);
    }
    public void CheckSync(float[] comp, Transform cube) {
        if (cube.rotation.w != comp[0] || cube.rotation.x != comp[1] ||
            cube.rotation.y != comp[2] || cube.rotation.z != comp[3]) {

            cube.rotation = new Quaternion(comp[1], comp[2], comp[3], comp[0]);
        }
        
    }
    public float[] GetQuatComponentAry(Transform cube) {
        float[] components = {cube.rotation.w, cube.rotation.x,
            cube.rotation.y, cube.rotation.z};

        return components;
    }
    public void AddAction(string[] actionArray, int cost) {
        playerActionList.GetComponent<ActionStorage>().StoreAction(actionArray);
        AddActionPoints(-cost);
    }

    //set
    public void SetRoundCountText(int val) {
        roundCountText.text =  ConvertNumToText(val);
    }
    public void SetRemainingTimeText(int val) {
        remainingTimeText.text = val.ToString();
    }
    public void SetState(int val) {
        state = val;
    }
    public void SetEnemyReady() {
        readies[1] = true;
    }
    public void SetEnemyActionList(string[][] array) {
        enemyActionList.GetComponent<ActionStorage>().SetActionListArray(array);
    }
    public void AddActionPoints(int val) {
        actionPoints += val;
        playerCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = actionPoints.ToString();
    }

    public void AddUnitCount(int val) {
        unitCount += val;
        print(unitCount);
        playerCanvas.transform.Find("PlayerUnitCount").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();
    }

    public void SpawnEnemyUnit(string[] unitArray){
        GameObject plane = enemyCubePosition.transform.GetChild(0).Find(unitArray[1]).Find(unitArray[2]).gameObject;
        GameObject unit = Instantiate(Resources.Load<GameObject>(enemyCubeInfo[0] + "/" + enemyCubeInfo[1] + "/Units/" + unitArray[0] + "/Prefab"));

        unit.transform.position = plane.transform.position;
        unit.transform.rotation = plane.transform.rotation;
        unit.transform.SetParent(plane.transform);
    }

    public void RemoveEnemyUnit(string[] unitArray){
        GameObject plane = enemyCubePosition.transform.GetChild(0).Find(unitArray[0]).Find(unitArray[1]).gameObject;
        Destroy(plane.transform.GetChild(0).gameObject);
    }

    //get
    public GameObject GetEnemyCanvas() {
        return enemyCanvas;
    }
    public GameObject GetMainCanvas() {
        return mainScreenCanvas;
    }
    public TextMeshProUGUI GetRoundCountText() {
        return roundCountText;
    }
    public TextMeshProUGUI GetRemainingTimeText() {
        return remainingTimeText;
    }
    public Transform GetEnemyCube() {
        return enemyCubePosition.transform.GetChild(0);
    }

    public int GetState() {
        return state;
    }
    public int GetRoundCount() {
        return roundCount;
    }
    public int GetActionPoints() {
        return actionPoints;
    }


    public string ConvertNumToText(int num) {
        if (num == 0) {
            return "Setup";
        }
        else if (num == 1) {
            return "Round One";
        }
        else if (num == 2) {
            return "Round Two";
        }
        else if (num == 3) {
            return "Round Three";
        }
        else if (num == 4) {
            return "Round Four";
        }
        else if (num == 5) {
            return "Round Five";
        }
        else if (num == 6) {
            return "Round Six";
        }
        else if (num == 7) {
            return "Round Seven";
        }
        else if (num == 8) {
            return "Round Eight";
        }
        else if (num == 9) {
            return "Round Nine";
        }
        else if (num == 10) {
            return "Round Ten";
        }
        else if (num == 11) {
            return "Round Eleven";
        }
        else if (num == 12) {
            return "Round Twelve";
        }
        else if (num == 13) {
            return "Round Thirteen";
        }
        else if (num == 14) {
            return "Round Fourteen";
        }
        else if (num == 15) {
            return "Round Fifteen";
        }
        else if (num == 16) {
            return "Round Sixteen";
        }
        else if (num == 17) {
            return "Round Seventeen";
        }
        else if (num == 18) {
            return "Round Eighteen";
        }
        else if (num == 19) {
            return "Round Nineteen";
        }
        else if (num == 20) {
            return "Round Twenty";
        }
        else {
            return "RCAP";
        }
    }
}
