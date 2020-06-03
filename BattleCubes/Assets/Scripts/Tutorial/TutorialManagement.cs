using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TutorialManagement : MonoBehaviour
{
    //[SerializeField] InfoSender infoSender;
    //[SerializeField] InfoReceiver infoReceiver;
    //[Space(10)]
    [SerializeField] GameObject enemyCanvas;
    [SerializeField] GameObject playerCanvas;
    [SerializeField] GameObject setupCanvas;
    [SerializeField] GameObject mainScreenCanvas;
    [SerializeField] GameObject rotationCanvas;
    [SerializeField] GameObject attackCanvas;
    [SerializeField] GameObject outtaTimeCanvas;
    [SerializeField] GameObject outtaSetupCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject playerApCounter;
    [Space(10)]
    [SerializeField] GameObject enemyActionList;
    [SerializeField] GameObject playerActionList;
    [Space(10)]
    [SerializeField] TextMeshProUGUI roundCountText;
    [SerializeField] TextMeshProUGUI remainingTimeText;
    [Space(10)]
    [SerializeField] GameObject preventClick;
    [SerializeField] TutorialAttackHandler attackHandler;
    [SerializeField] infoMenu infoMenu;

    public GameObject playerCubePosition;
    public GameObject enemyCubePosition;

    public GameObject playerTargettingSystem;
    public GameObject enemyTargettingSystem;

    public List<string[]> attackList = new List<string[]>();

    List<string> playerInfo;
    public bool[] readies = {true, true};

    //states
    public readonly int SETUP = 0;
    public readonly int PLAN = 1;
    public readonly int THROWDOWN = 2;
    public readonly int GAMEEND = 3;
    public int state;

    //notifications
    const int TIME_UP = 0;
    const int PLAYERS_READY = 1;

    //timeming
    public readonly float ROUND_TIME = 30;
    public readonly float SET_UP_TIME = 150;
    public float remainingTime;
    public bool timeStopped = true;

    //corrutine stuff
    bool setupCorrutineRunning = false;

    //round management
    int roundCount = 0;

    //player points
    int roundsForPointIncrease = 5;
    int pointsPerRound = 3;
    int actionPoints = 0;

    //unit points
    public int totalUnitPoints = 12;
    public int remainingUnitPoints = 12;

    public int towerCount = 0;
    public int unitCount = 0;

    public int enemyTowerCount = 0;
    public int enemyUnitCount = 0;

    public string[] enemyCubeInfo;
    public string[] cubeInfo;

    int tmpTime;
    bool tmpTimeChange = false;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start() {
        cubeInfo = new string[] {PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor")};

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume"));
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }

        state = SETUP;

        playerInfo = new List<string>();
        playerInfo.Add(PlayerPrefs.GetString("PlayerName"));

        //infoSender.SendPlayerStats(ListToStringArray(playerInfo));
        //infoSender.SendCubePrefs(ListToStringArray(playerInfo));

        playerCanvas.transform.Find("PlayerName").gameObject.GetComponent<TextMeshProUGUI>().text = playerInfo[0];
        playerCanvas.transform.Find("PlayerUnitCounter").Find("Count").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();

        SpawnPlayerCube();
        SpawnEnemyCube();

        SpawnEnemyUnits();
        //AssignUnitPlacementLocations();

        //infoSender.SendCubeInfo(cubeInfo);

        setupCanvas.transform.Find("swiperPannel").gameObject.GetComponent<TutorialRotationByFinger>().SetRotAllowed(true);

        //playerTargettingSystem = GameObject.FindGameObjectWithTag("PlayerCubePosition").transform.GetChild(1).gameObject;
        // playerTargettingSystem.SetActive(false);
        // attackHandler.TurnOffTargetting();

        FillAttackList();

        playerCanvas.transform.Find("PlayerAPCounter").Find("Count").GetComponent<TextMeshProUGUI>().text = actionPoints.ToString();

        remainingTime = SET_UP_TIME;

        //infoMenu.Pause();
        infoMenu.Welcome();
    }

    void Update()
    {
        //if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            if (!timeStopped) {
                CheckForReady();
                RunTimer();
            }
        //print(enemyUnitCount + ": " + enemyTowerCount);

        //if (infoMenu.isPaused) {
            //timeStopped = true;
        //}
        //}
    }

    // void UpdateUnitCount(){
    //     playerCanvas.transform.Find("PlayerUnitCount").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();
    // }

    public void CheckForReady() {
        if (readies[0] && readies[1]) {
            remainingTime = 0;
            readies[0] = false;
            readies[1] = true;
            //ResetRound();
            //infoSender.SendResetRound();
            //if (state == SETUP) {
                //    remainingTime = 0;
                //playerApCounter.SetActive(true);
                
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

        //if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            CheckWinCondition();
        //}

        if(state != GAMEEND){

            playerApCounter.SetActive(true);

            setupCanvas.SetActive(false);
            enemyActionList.SetActive(true);
            playerActionList.SetActive(true);
            enemyCanvas.SetActive(true);

            rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<TutorialRotationByFinger>().SetRotAllowed(false);

            outtaSetupCanvas.GetComponent<TweenController>().Notify();
            yield return new WaitForSeconds(1.0f);
           // infoSender.SendCubeRotation(GetQuatComponentAry(playerCubePosition.transform.GetChild(0)));
            yield return new WaitForSeconds(0.25f);
            enemyCubePosition.GetComponent<TweenController>().slideEnemyUp();
            
            yield return new WaitForSeconds(3.0f);
            //infoSender.SendCubeRotation(GetQuatComponentAry(playerCubePosition.transform.GetChild(0)));
            
            //mainScreenCanvas.SetActive(true);

            //remainingTime = ROUND_TIME;
            //timeStopped = false;

            //roundCount++;
            //roundCountText.text = ConvertNumToText(roundCount);

            ResetRound();
            preventClick.SetActive(false);
        }

        // setupCanvas.SetActive(false);
        // enemyActionList.SetActive(true);
        // playerActionList.SetActive(true);
        // enemyCanvas.SetActive(true);

        // rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(false);

        // outtaSetupCanvas.GetComponent<TweenController>().Notify();

        // yield return new WaitForSeconds(3f);
        // infoSender.SendCubeRotation(GetQuatComponentAry(playerCubePosition.transform.GetChild(0)));
        
        // mainScreenCanvas.SetActive(true);

        // remainingTime = ROUND_TIME;
        // timeStopped = false;

        // roundCount++;
        // roundCountText.text = ConvertNumToText(roundCount);

        //infoSender.SendCubePrefs(cubeInfo);

        // if (PhotonNetwork.LocalPlayer.IsMasterClient) {
        //     CheckWinCondition();
        // }

        // ResetRound();
    }

    public IEnumerator StartThrowDown() {
        outtaTimeCanvas.GetComponent<TweenController>().Notify();

        preventClick.SetActive(true);
        state = THROWDOWN;

        attackHandler.ResetChooseAttackHandlers();
        attackHandler.ResetAttackBack();

        mainScreenCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        rotationCanvas.SetActive(false);
        rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<TutorialRotationByFinger>().SetRotAllowed(false);

        yield return new WaitForSeconds(1.5f);
        playerCubePosition.transform.GetChild(0).GetComponent<TutorialRotateCube>().SnapToBaseRotation();
        yield return new WaitForSeconds(1.5f);
        TurnOnTargetting();

        //playerActionList.GetComponent<ActionStorage>().PrepareActionListForSend()
        if (playerActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > 0) {
            //infoSender.SendActionListArray(playerActionList.GetComponent<ActionStorage>().PrepareActionListForSend());
        }
        
        yield return new WaitForSeconds(0.5f);

        playerActionList.GetComponent<TutorialActionStorage>().resetUIPulses();
        enemyActionList.GetComponent<TutorialActionStorage>().resetUIPulses();

        playerActionList.GetComponent<TweenController>().slideXOver((Screen.width * -0.034f));
        enemyActionList.GetComponent<TweenController>().slideXOver((Screen.width * 0.034f));

        //if () {
            StartCoroutine(DoThrowdown());
        //}
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
                //infoSender.SendNotification(TIME_UP);
                //infoSender.SendStartThrowDown();
                StartCoroutine(StartThrowDown());
                //StartThrowDown();
            }
            else if (state == SETUP) {
                //infoSender.SendGetOutOfSetup();
                StartCoroutine(GetOutOfSetUp());
            }
        }
        intTIME = Mathf.CeilToInt(remainingTime);

        if (!tmpTimeChange) {
            tmpTime = intTIME;

            remainingTimeText.text = intTIME.ToString();
            //infoSender.SendGameStatus(new int[] { intTIME, roundCount, state });

            tmpTimeChange = true;
        }
        else {
            if (tmpTime != intTIME) {
                tmpTime = intTIME;

                remainingTimeText.text = intTIME.ToString();
                //infoSender.SendGameStatus(new int[] { intTIME, roundCount, state });

                tmpTimeChange = false;
            }
        }
    }

    public void HighlightThrowdownAction(int i){
        playerActionList.GetComponent<TutorialActionStorage>().resetUIPulses();
        enemyActionList.GetComponent<TutorialActionStorage>().resetUIPulses();

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
        for (int i = 0; i < 5 && state == THROWDOWN; i++) {

            if (playerActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > i || enemyActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > i) {
                HighlightThrowdownAction(i);
                //infoSender.SendThrowdownHighlight(i);
            }
            // playerActionList.GetComponent<ActionStorage>().resetUIPulses();
            // enemyActionList.GetComponent<ActionStorage>().resetUIPulses();

            // // playerActionList.GetComponent<ActionStorage>().resetUIPulses();
            // playerActionList.transform.GetChild(i).GetComponent<TweenController>().PulseHighlight();
            // enemyActionList.transform.GetChild(i).GetComponent<TweenController>().PulseHighlight();
            // transform.GetChild(actionList.Count - 1).GetComponent<TweenController>().PulseHighlight();
            // enemyActionList.GetComponent<ActionStorage>().resetUIPulses();
            //print(i);

            //CheckWinCondition();
            yield return new WaitForSeconds(0.25f);

            //both players have actions
            if (playerActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > i && enemyActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > i) {
                yield return new WaitForSeconds(0.5f);
                print("both players have actions");
                host = playerActionList.GetComponent<TutorialActionStorage>().GetAt(i);
                client = enemyActionList.GetComponent<TutorialActionStorage>().GetAt(i);

                if (host[0] == client[0]) {
                    yield return new WaitForSeconds(0.5f);

                    if (host[0] == "rotate") {
                        print("Players both rotate");

                        //rotate both
                        TranslateRotatePlayerCube(host[1]);
                        TranslateRotateEnemyCube(client[1]);

                        //infoSender.SendPlayerTurningDir(host[1]);
                        //infoSender.SendEnemyTurningDir(client[1]);

                        //playerCubePosition.transform.GetChild(0).GetComponent<RotateCube>().AssignDirection(host[1]);
                        //enemyCubePosition.transform.GetChild(0).GetComponent<RotateCube>().AssignDirection(client[1]);
                    }
                    else if (host[0] == "attack") {
                        //both attack
                        print("Players both attack");

                        StartCoroutine(attackHandler.DoAttack("Host", host));
                        StartCoroutine(attackHandler.DoAttack("Client", client));


                        //infoSender.SendPlayerAttackInfo(host);
                        //infoSender.SendEnemyAttackInfo(client);
                    }
                    yield return new WaitForSeconds(3.5f);
                }
                else {
                    yield return new WaitForSeconds(0.5f);
                    if (host[0] == "rotate") {
                        print("Host rotate, client attack");

                        //host rotates, client then attacks
                        TranslateRotatePlayerCube(host[1]);
                        //infoSender.SendPlayerTurningDir(host[1]);
                        
                        yield return new WaitForSeconds(3.5f);

                        StartCoroutine(attackHandler.DoAttack("Client", client));
                        //infoSender.SendEnemyAttackInfo(client);
                        //infosender
                        //yield return new WaitForSeconds(3);

                    }
                    else if (host[0] == "attack") {
                        yield return new WaitForSeconds(0.5f);
                        print("Client rotate, Host attack");

                        //rotate client , host then attacks
                        TranslateRotateEnemyCube(client[1]);
                        //infoSender.SendEnemyTurningDir(client[1]);
                        
                        yield return new WaitForSeconds(3.5f);

                        StartCoroutine(attackHandler.DoAttack("Host", host));
                        //infoSender.SendPlayerAttackInfo(host);

                        //infosender
                        //yield return new WaitForSeconds(3);

                    }
                    yield return new WaitForSeconds(3.5f);
                }
            }
            //only host has actions
            else if (playerActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > i) {
                yield return new WaitForSeconds(0.5f);
                print("host has actions");
                host = playerActionList.GetComponent<TutorialActionStorage>().GetAt(i);

                if (host[0] == "rotate") {
                    print("Host rotate");

                    //host rotates
                    TranslateRotatePlayerCube(host[1]);
                    //infoSender.SendPlayerTurningDir(host[1]);
                }
                else if (host[0] == "attack") {
                    print("Host attack");
                    StartCoroutine(attackHandler.DoAttack("Host", host));
                    //infoSender.SendPlayerAttackInfo(host);

                    //infosender
                }
                yield return new WaitForSeconds(3.5f);
            }
            //only client has actions
            else if (enemyActionList.GetComponent<TutorialActionStorage>().GetActionListCount() > i) {
                yield return new WaitForSeconds(0.5f);
                print("client has actions");
                client = enemyActionList.GetComponent<TutorialActionStorage>().GetAt(i);

                if (client[0] == "rotate") {
                    print("client rotate");

                    //client rotates
                    TranslateRotateEnemyCube(client[1]);
                    //infoSender.SendEnemyTurningDir(client[1]);
                }
                else if (client[0] == "attack") {
                    print("client attack");
                    //print(client[1] + " "+ client[2] + " "+ client[3]);

                    StartCoroutine(attackHandler.DoAttack("Client", client));
                    //infoSender.SendEnemyAttackInfo(client);

                    //infosender
                }
                yield return new WaitForSeconds(3.5f);
            }
            //no more actions
            else {
                print("no more actions");
                break;
            }

            yield return new WaitForSeconds(0.25f);
            CheckWinCondition();
            yield return new WaitForSeconds(1f);
        }

        CheckWinCondition();
        yield return new WaitForSeconds(0.25f);

        // attackHandler.TurnOffTargetting();
        if (state == THROWDOWN){
            //timeStopped = false;
            //remainingTime = ROUND_TIME;
            //roundCount++;
            //roundCountText.text = ConvertNumToText(roundCount);

            //mainScreenCanvas.SetActive(true);

            ResetRound();
            //infoSender.SendResetRound();
        }

        yield return new WaitForSeconds(0.25f);
        CheckWinCondition();
    }

    public void TranslateRotatePlayerCube(string direction) {
        playerCubePosition.transform.GetChild(0).GetComponent<TutorialRotateCube>().RequestRotation(direction);
    }
    public void TranslateRotateEnemyCube(string direction) {
        enemyCubePosition.transform.GetChild(0).GetComponent<TutorialRotateCube>().RequestRotation(direction);
    }

    public void CreateFloatingText(string text, Transform location, string type)
    {
        FloatingText instance = Instantiate(Resources.Load<FloatingText>("Prefabs/PopupTextParent"));
        instance.transform.SetParent(GameObject.Find("GameCanvas").transform, true);

        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x/*+ Random.Range(-0.5f, 0.5f)*/, location.position.y/*+ Random.Range(-0.5f, 0.5f)*/));
        //Vector3 sc = Camera.main.WorldToViewportPoint(location.position);

        //instance.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
        //print(sc.x + " " + sc.y + " " + sc.z);
        //Vector3 tmp = instance.transform.position;
        //tmp.x = (sc.x * Screen.width);
        //tmp.y = (sc.y * Screen.height);

        if (type == "attack")
        {
            Vector3 sc = Camera.main.WorldToViewportPoint(location.position);

            //instance.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            //print(sc.x + " " + sc.y + " " + sc.z);
            Vector3 tmp = instance.transform.position;
            tmp.x = (sc.x * Screen.width);
            tmp.y = (sc.y * Screen.height);

            instance.transform.position = tmp;
        }

        else if (type == "points") {
            instance.transform.position = location.position;
        }

        instance.SetText(text);
    }

    public void TranslateAttackOnEnemyCube(string[] array) {
        StartCoroutine(attackHandler.DoAttack("Client", array));
    }
    public void TranslateAttackOnPlayerCube(string[] array) {
        StartCoroutine(attackHandler.DoAttack("Host", array));
    }

    public void CheckWinCondition(){

        if(roundCount > 20 || unitCount <= 0 || towerCount < 3 || enemyUnitCount <= 0 || enemyTowerCount < 3){
            
            timeStopped = true;
            // state = GAMEEND;

            //mainScreenCanvas.SetActive(false);

            // pauseCanvas.SetActive(false);
            // gameOverCanvas.SetActive(true);

            if(state != SETUP){

                if(unitCount <= 0 || towerCount < 3){
                    GameEndPopUp("You Lose: All Units or Tower Destroyed");
                    //infoSender.SendGameOver("You Win!: Opponent Units or Tower Destroyed");
                    // gameCanvas.transform.GetChild(1).GetChild(0).Find("GameOverText").GetComponent<TextMeshProUGUI>().text;
                }
                else if(enemyUnitCount <= 0 || enemyTowerCount < 3){
                    GameEndPopUp("You Win!: Opponent Units or Tower Destroyed");
                    //infoSender.SendGameOver("You Lose: All Units or Tower Destroyed");
                }
                else if(roundCount >= 20){
                    if(unitCount > enemyUnitCount){
                        GameEndPopUp("You Win!: More units");
                        //infoSender.SendGameOver("You Lose: Less units");
                    }
                    else if(unitCount < enemyUnitCount){
                        GameEndPopUp("You Lose: Less units");
                        //infoSender.SendGameOver("You Win!: More units");
                    }
                    else if(unitCount == enemyUnitCount){
                        GameEndPopUp("Game Tie");
                        //infoSender.SendGameOver("Game Tie");
                    }
                    else{
                        GameEndPopUp("ERROR 1: Unknown game end condition");
                        //infoSender.SendGameOver("ERROR 1: Unknown game end condition");
                    }
                }
            }
            else{
                if((unitCount <= 0 || towerCount < 3) && (enemyUnitCount <= 0 || enemyTowerCount < 3)){
                    GameEndPopUp("Both players did not finish setup in time...");
                    //infoSender.SendGameOver("Both players did not finish setup in time...");
                }
                else if(unitCount <= 0 || towerCount < 3){
                    GameEndPopUp("You did not finish setup in time...");
                    //infoSender.SendGameOver("Opponent did not finish setup in time...");
                }
                else if(enemyUnitCount <= 0 || enemyTowerCount < 3){
                    GameEndPopUp("Opponent did not finish setup in time...");
                    //infoSender.SendGameOver("You did not finish setup in time...");
                }
                else{
                    GameEndPopUp("ERROR 2: Unknown game end condition");
                    //infoSender.SendGameOver("ERROR 2: Unknown game end condition");
                }
            }

        }
    }

    public void GameEndPopUp(string condition){
        //mainScreenCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        state = GAMEEND;

        TextMeshProUGUI gameOverText = gameOverCanvas.transform.GetChild(1).GetChild(0).Find("GameOverText").GetComponent<TextMeshProUGUI>();
        gameOverText.text = condition;

        state = GAMEEND;

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
        readies[1] = true;

        remainingTime = 0;
        timeStopped = false;

        //infoSender.SendReady();
    }

    public void ResetRound() {
        readies[0] = false;
        readies[1] = false;

        print("Reseting round!!!");
        // CheckWinCondition();

        playerCubePosition.transform.GetChild(0).GetComponent<TutorialRotateCube>().SetBasePos();

        //int extraPoints = ((roundCount - (roundCount % roundsForPointIncrease))/roundsForPointIncrease);
        //print("Extra points: " + extraPoints);

        //AddActionPoints(pointsPerRound + extraPoints);

        if (state != SETUP && roundCount >= 1) {
            playerActionList.GetComponent<TweenController>().slideXOver((Screen.width * 0.034f));
            enemyActionList.GetComponent<TweenController>().slideXOver((Screen.width * -0.034f));

            playerActionList.GetComponent<TutorialActionStorage>().ClearActionList();
            enemyActionList.GetComponent<TutorialActionStorage>().ClearActionList();
            TurnOffTargetting();
        }

        remainingTime = ROUND_TIME;
        timeStopped = false;

        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);

        int extraPoints = ((roundCount - (roundCount % roundsForPointIncrease)) / roundsForPointIncrease);
        print("Extra points: " + extraPoints);

        AddActionPoints(pointsPerRound + extraPoints);

        mainScreenCanvas.SetActive(true);

        preventClick.SetActive(false);

        state = PLAN;
    }

    void FillAttackList() {
        for (int i = 1; i <= 3; i++) {
            GameObject unitPrefab = Resources.Load<GameObject>("Themes/Tutorial/Units/"+ i + "/Prefab");
            if (unitPrefab) {
                TutorialUnitInformation unitInformation = unitPrefab.GetComponent<TutorialUnitInformation>();
                attackList.Add(new string[] { unitInformation.attackName, "false" });
                print(attackList[i - 1][0] + " : " + attackList[i - 1][1]);
            }
        }
    }

    void SpawnPlayerCube() {
        playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        GameObject cube = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Cube/Prefab"));
        cube.transform.position = playerCubePosition.transform.position;
        cube.transform.rotation = playerCubePosition.transform.rotation;
        cube.transform.SetParent(playerCubePosition.transform);

        SpawnCubeTargetingSystem("PlayerCubePosition");
        SpawnAttackChecker("PlayerCubePosition");
        SpawnHideUnit("PlayerCubePosition");

        playerTargettingSystem = GameObject.FindGameObjectWithTag("PlayerCubePosition").transform.GetChild(1).gameObject;
        //playerTargettingSystem.SetActive(false);
    }
    public void SpawnEnemyCube() {
        enemyCubePosition = GameObject.FindGameObjectWithTag("EnemyCubePosition");

        GameObject cube = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Cube/Prefab"));
        cube.transform.position = enemyCubePosition.transform.position;
        cube.transform.rotation = enemyCubePosition.transform.rotation;
        cube.transform.SetParent(enemyCubePosition.transform);

        SpawnCubeTargetingSystem("EnemyCubePosition");
        SpawnAttackChecker("EnemyCubePosition");
        SpawnHideUnit("EnemyCubePosition");
        // enemyCubeInfo = cubeInfo;

        enemyTargettingSystem = GameObject.FindGameObjectWithTag("EnemyCubePosition").transform.GetChild(1).gameObject;
        //enemyTargettingSystem.SetActive(false);
    }
    void SpawnCubeTargetingSystem(string tag){
        GameObject CubePosition = GameObject.FindGameObjectWithTag(tag);

        GameObject cubeTargeting = Instantiate(Resources.Load<GameObject>("MainCubePrefab/CubeTargeting"));
        cubeTargeting.transform.position = CubePosition.transform.position;
        cubeTargeting.transform.rotation = CubePosition.transform.rotation;
        cubeTargeting.transform.SetParent(CubePosition.transform);
    }
    void SpawnAttackChecker(string tag)
    {
        GameObject CubePosition = GameObject.FindGameObjectWithTag(tag);

        GameObject attackChecker = Instantiate(Resources.Load<GameObject>("MainCubePrefab/AttackChecker"));
        attackChecker.transform.position = CubePosition.transform.position;
        attackChecker.transform.rotation = CubePosition.transform.rotation;
        attackChecker.transform.SetParent(CubePosition.transform);
    }
    void SpawnHideUnit(string tag)
    {
        GameObject CubePosition = GameObject.FindGameObjectWithTag(tag);

        GameObject attackChecker = Instantiate(Resources.Load<GameObject>("MainCubePrefab/HideUnits"));
        attackChecker.transform.position = CubePosition.transform.position;
        attackChecker.transform.rotation = CubePosition.transform.rotation;
        attackChecker.transform.SetParent(CubePosition.transform);
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
        playerActionList.GetComponent<TutorialActionStorage>().StoreAction(actionArray);
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
        playerCanvas.transform.Find("PlayerAPCounter").Find("Count").GetComponent<TextMeshProUGUI>().text = actionPoints.ToString();
        //CreateFloatingText();
        if (val > 0) {
            CreateFloatingText("+ "+ val.ToString(), GameObject.Find("PlayerAPCounter").transform, "points");
        }
        else{ 
            CreateFloatingText(val.ToString(), GameObject.Find("PlayerAPCounter").transform, "points");
        }
    }

    public void AddUnitCount(int val) {
        unitCount += val;
        print("Unit Count: " + unitCount);

        for (int i = 0; i < playerCanvas.transform.Find("PlayerUnitCount").childCount; i++)
        {
            playerCanvas.transform.Find("PlayerUnitCount").GetChild(i).gameObject.SetActive(false);
        }

        for (int j = 0; j < unitCount; j++) {
            playerCanvas.transform.Find("PlayerUnitCount").GetChild(j).gameObject.SetActive(true);
        }

        //playerCanvas.transform.Find("PlayerUnitCount").Find("Count").GetComponent<TextMeshProUGUI>().text = unitCount.ToString();

    }
    public void AddTowerCount(int val) {
        towerCount += val;
        print("Tower Count: " + towerCount);
        //playerCanvas.transform.Find("PlayerUnitCount").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();
    }
    public void AddUnitPoints(int val)
    {
        remainingUnitPoints += val;
        print("Unit Points Remaining: " + towerCount);
        
        infoMenu.transform.Find("DragNDropSide").Find("Background").Find("UnitIPlacement").Find("Background").Find("PlayerUnitPointCounter").Find("Count").gameObject.GetComponent<TextMeshProUGUI>().text = remainingUnitPoints.ToString();

        //setupCanvas.transform.Find("DragNDropSide").Find("PlayerUnitPointCounter").Find("Count").gameObject.GetComponent<TextMeshProUGUI>().text = remainingUnitPoints.ToString();
        //setupCanvas.transform.Find("DragNDropSide").Find("UnitPoints").gameObject.GetComponent<TextMeshProUGUI>().text = ("Unit Points Left: <color=yellow>" + remainingUnitPoints.ToString());
    }

    public void AddEnemyUnitCount(int val) {
        enemyUnitCount += val;
        print("Enemy Unit Count: " + enemyUnitCount);

        for (int i = 0; i < enemyCanvas.transform.Find("EnemyUnitCount").childCount; i++)
        {
            enemyCanvas.transform.Find("EnemyUnitCount").GetChild(i).gameObject.SetActive(false);
        }

        for (int j = 0; j < enemyUnitCount; j++)
        {
            enemyCanvas.transform.Find("EnemyUnitCount").GetChild(j).gameObject.SetActive(true);
        }

        //enemyCanvas.transform.Find("EnemyUnitCounter").Find("Count").GetComponent<TextMeshProUGUI>().text = enemyUnitCount.ToString();
    }
    public void AddEnemyTowerCount(int val) {
        enemyTowerCount += val;
        print("Enemy Tower Count: " + enemyTowerCount);
        //playerCanvas.transform.Find("PlayerUnitCount").gameObject.GetComponent<TextMeshProUGUI>().text = unitCount.ToString();
    }

    public void SpawnEnemyUnit(string[] unitArray){
        GameObject plane = enemyCubePosition.transform.GetChild(0).Find(unitArray[1]).Find(unitArray[2]).gameObject;
        GameObject unit = Instantiate(Resources.Load<GameObject>("Themes/" + enemyCubeInfo[0] + "/Units/" + unitArray[0] + "/Prefab"));

        unit.transform.position = plane.transform.position;
        unit.transform.rotation = plane.transform.rotation;
        unit.transform.SetParent(plane.transform);

        if(unit.GetComponent<UnitInformation>().isTower){
            AddEnemyTowerCount(1);
        }
        else{
            AddEnemyUnitCount(1);
        }
        // AddEnemyUnitCount(1);
    }

    public void SpawnEnemyUnits() {

        List<string[]> tutorialSpawns = new List<string[]>();
        
        tutorialSpawns.Add(new string[] { "Face_2", "2", "unit"});
        tutorialSpawns.Add(new string[] { "Face_2", "4", "unit"});
        tutorialSpawns.Add(new string[] { "Face_5", "8", "unit"});
        tutorialSpawns.Add(new string[] { "Face_6", "2", "unit" });

        tutorialSpawns.Add(new string[] { "Face_2", "6", "tower" });
        tutorialSpawns.Add(new string[] { "Face_4", "5", "tower" });
        tutorialSpawns.Add(new string[] { "Face_3", "8", "tower" });



        //goes everytime through to spawn each unit
        foreach (string[] unit in tutorialSpawns) {

            //gets every face of the cube
            for (int x = 0; x < enemyCubePosition.transform.GetChild(0).childCount; x++)
            {
                // gets every square unit on a face
                for (int i = 0; i < enemyCubePosition.transform.GetChild(0).GetChild(x).childCount; i++)
                {
                    //int randomIndex = Random.Range(0, 8);
                    Transform face = enemyCubePosition.transform.GetChild(0).GetChild(x).GetChild(i);
                    //print(face.tag);
                    //checks for the correct unit placement
                    if (face.tag == "unitSquare" && face.childCount < 1 && face.parent.name == unit[0] && face.name == unit[1])
                    {
                        //print("im in");

                        GameObject unitPrefab;

                        if (unit[2] == "tower") {
                            unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Units/Tower/Prefab"));
                            enemyTowerCount++;
                        }
                        else { 
                            unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Units/1/Prefab"));
                            enemyUnitCount++;
                        }
                        //GameObject unitPrefab = Instantiate(unitPrefab)

                        //var unit = Instantiate(unitPrefab);

                        unitPrefab.transform.position = face.transform.position;
                        unitPrefab.transform.rotation = face.transform.rotation;

                        var rand = Random.Range(0, 4);

                        //unit.transform.Translate(0.0f, 0.0f, 0.0f);
                        unitPrefab.transform.Rotate(0.0f, (rand * 90.0f), 0.0f);
                        // Set unit as a child of the unitPlane
                        unitPrefab.transform.SetParent(face.transform);
                    }
                }

            }

        }

    }

    public void AssignPlacementLocations()
    {

        List<string[]> tutorialSpawns = new List<string[]>();

        //tutorialSpawns.Add(new string[] { "Face_2", "2", "unit" });
        //tutorialSpawns.Add(new string[] { "Face_2", "4", "unit" });
        //tutorialSpawns.Add(new string[] { "Face_5", "8", "unit" });
        //tutorialSpawns.Add(new string[] { "Face_6", "2", "unit" });

        tutorialSpawns.Add(new string[] { "Face_3", "2", "tower" });
        tutorialSpawns.Add(new string[] { "Face_4", "5", "tower" });
        tutorialSpawns.Add(new string[] { "Face_5", "5", "tower" });



        //goes everytime through to spawn each unit
        foreach (string[] unit in tutorialSpawns)
        {

            //gets every face of the cube
            for (int x = 0; x < playerCubePosition.transform.GetChild(0).childCount; x++)
            {
                // gets every square unit on a face
                for (int i = 0; i < playerCubePosition.transform.GetChild(0).GetChild(x).childCount; i++)
                {
                    //int randomIndex = Random.Range(0, 8);
                    Transform face = playerCubePosition.transform.GetChild(0).GetChild(x).GetChild(i);
                    //print(face.tag);
                    //checks for the correct unit placement
                    if (face.tag == "unitSquare" && face.childCount < 1 && face.parent.name == unit[0] && face.name == unit[1])
                    {
                        //print("im in");
                        face.GetComponent<TweenController>().HighlightPlacementTargets();

                        //GameObject unitPrefab;

                        //if (unit[2] == "tower")
                        //{
                            //unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Units/Tower/Prefab"));
                        //}
                        //else
                        //{
                            //unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Units/1/Prefab"));
                        //}
                        //GameObject unitPrefab = Instantiate(unitPrefab)

                        //var unit = Instantiate(unitPrefab);

                        //unitPrefab.transform.position = face.transform.position;
                        //unitPrefab.transform.rotation = face.transform.rotation;

                        //var rand = Random.Range(0, 4);

                        //unit.transform.Translate(0.0f, 0.0f, 0.0f);
                        //unitPrefab.transform.Rotate(0.0f, (rand * 90.0f), 0.0f);
                        // Set unit as a child of the unitPlane
                        //unitPrefab.transform.SetParent(face.transform);
                    }
                }

            }

        }

    }

    public void AssignUnitPlacementLocations()
    {

        List<string[]> tutorialSpawns = new List<string[]>();

        tutorialSpawns.Add(new string[] { "Face_2", "5", "unit" });
        tutorialSpawns.Add(new string[] { "Face_4", "3", "unit" });
        tutorialSpawns.Add(new string[] { "Face_5", "2", "unit" });
        tutorialSpawns.Add(new string[] { "Face_6", "6", "unit" });

        //tutorialSpawns.Add(new string[] { "Face_3", "2", "tower" });
        //tutorialSpawns.Add(new string[] { "Face_4", "5", "tower" });
        //tutorialSpawns.Add(new string[] { "Face_5", "5", "tower" });



        //goes everytime through to spawn each unit
        foreach (string[] unit in tutorialSpawns)
        {

            //gets every face of the cube
            for (int x = 0; x < playerCubePosition.transform.GetChild(0).childCount; x++)
            {
                // gets every square unit on a face
                for (int i = 0; i < playerCubePosition.transform.GetChild(0).GetChild(x).childCount; i++)
                {
                    //int randomIndex = Random.Range(0, 8);
                    Transform face = playerCubePosition.transform.GetChild(0).GetChild(x).GetChild(i);
                    //print(face.tag);
                    //checks for the correct unit placement
                    if (face.tag == "unitSquare" && face.childCount < 1 && face.parent.name == unit[0] && face.name == unit[1])
                    {
                        //print("im in");
                        face.GetComponent<TweenController>().HighlightPlacementTargets();

                        //GameObject unitPrefab;

                        //if (unit[2] == "tower")
                        //{
                        //unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Units/Tower/Prefab"));
                        //}
                        //else
                        //{
                        //unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/Tutorial/Units/1/Prefab"));
                        //}
                        //GameObject unitPrefab = Instantiate(unitPrefab)

                        //var unit = Instantiate(unitPrefab);

                        //unitPrefab.transform.position = face.transform.position;
                        //unitPrefab.transform.rotation = face.transform.rotation;

                        //var rand = Random.Range(0, 4);

                        //unit.transform.Translate(0.0f, 0.0f, 0.0f);
                        //unitPrefab.transform.Rotate(0.0f, (rand * 90.0f), 0.0f);
                        // Set unit as a child of the unitPlane
                        //unitPrefab.transform.SetParent(face.transform);
                    }
                }

            }

        }

    }


    public void RemoveEnemyUnit(string[] unitArray) {
        GameObject plane = enemyCubePosition.transform.GetChild(0).Find(unitArray[0]).Find(unitArray[1]).gameObject;
        if (plane.transform.GetChild(0).GetComponent<UnitInformation>().isTower) {
            AddEnemyTowerCount(-1);
        }
        else {
            AddEnemyUnitCount(-1);
        }

        Destroy(plane.transform.GetChild(0).gameObject);
    }
    public bool IsCubeTweening() {
        return LeanTween.isTweening(playerCubePosition.transform.GetChild(0).gameObject); 
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
            return "Setup Phase";
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
            return "Game Over";
        }
    }
}
