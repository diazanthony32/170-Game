using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

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
    [Space(10)]
    [SerializeField] GameObject enemyActionList;
    [SerializeField] GameObject playerActionList;
    [Space(10)]
    [SerializeField] TextMeshProUGUI roundCountText;
    [SerializeField] TextMeshProUGUI remainingTimeText;
    GameObject playerCubePosition;
    GameObject enemyCubePosition;

    List<string> playerInfo;
    bool[] readies = {false, false};

    //states
    const int SETUP = 0;
    const int PLAN = 1;
    const int THROWDOWN = 2;
    const int GAMEEND = 3;
    int state = SETUP;

    //notifications
    const int TIME_UP = 0;
    const int PLAYERS_READY = 1;

    //timeming
    public readonly float ROUND_TIME = 10;
    public readonly float SET_UP_TIME = 20;
    float remainingTime;
    bool timeStopped = false;

    //corrutine stuff
    bool setupCorrutineRunning = false;

    //round management
    int roundCount = 0;

    void Start() {
        string[] cubeInfo = {PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor")};

        playerInfo = new List<string>();
        playerInfo.Add(PlayerPrefs.GetString("PlayerName"));

        infoSender.SendPlayerStats(ListToStringArray(playerInfo));

        playerCanvas.transform.Find("PlayerName").gameObject.GetComponent<TextMeshProUGUI>().text = playerInfo[0];

        SpawnPlayerCube(cubeInfo);
        infoSender.SendCubeInfo(cubeInfo);

        setupCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(true);

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
    }
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

    public void GetOutOfSetUp() {
        print("getting out of setup");
        remainingTime = ROUND_TIME;
        timeStopped = false;
        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);

        setupCanvas.SetActive(false);
        enemyActionList.SetActive(true);
        playerActionList.SetActive(true);
        enemyCanvas.SetActive(true);

        mainScreenCanvas.SetActive(true);

        rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(false);

        ResetRound();
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            infoSender.SendResetRound();
        }
        state = PLAN;
    }

    public IEnumerator StartThrowDown() {
        mainScreenCanvas.SetActive(false);
        attackCanvas.SetActive(false);
        rotationCanvas.SetActive(false);
        rotationCanvas.transform.Find("swiperPannel").gameObject.GetComponent<RotationByFinger>().SetRotAllowed(false);

        yield return new WaitForSeconds(3f);
        //playerActionList.GetComponent<ActionStorage>().PrepareActionListForSend()
        if (playerActionList.GetComponent<ActionStorage>().GetActionListCount() > 0) {
            infoSender.SendActionListArray(playerActionList.GetComponent<ActionStorage>().PrepareActionListForSend());
        }

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
                outtaTimeCanvas.GetComponent<TweenController>().Notify();

                infoSender.SendNotification(TIME_UP);
                infoSender.SendStartThrowDown();
                StartCoroutine(StartThrowDown());
                //StartThrowDown();
            }
            else if (state == SETUP) {
                infoSender.SendGetOutOfSetup();
                GetOutOfSetUp();
            }
        }
        intTIME = Mathf.CeilToInt(remainingTime);
        remainingTimeText.text = intTIME.ToString();
        infoSender.SendGameStatus(new int[] { intTIME, roundCount, state });
    }

    IEnumerator DoThrowdown() {
        state = THROWDOWN;
        //print("inside CORRUTINE");
        yield return new WaitForSeconds(3);

        string[] host;
        string[] client;
        for (int i = 0; i < 5; i++) {
            print(i);

            if (playerActionList.GetComponent<ActionStorage>().GetActionListCount() > i && enemyActionList.GetComponent<ActionStorage>().GetActionListCount() > i) {
                print("both players have actions");
                host = playerActionList.GetComponent<ActionStorage>().GetAt(i);
                client = enemyActionList.GetComponent<ActionStorage>().GetAt(i);

                if (host[0] == client[0]) {
                    if (host[0] == "rotate") {
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
                    }
                    yield return new WaitForSeconds(3);
                }
                else {
                    if (host[0] == "rotate") {
                        //host rotates, client then attacks
                        TranslateRotatePlayerCube(host[1]);
                        infoSender.SendPlayerTurningDir(host[1]);
                        yield return new WaitForSeconds(3);
                    }
                    else if (host[0] == "attack") {
                        //rotate client , host then attacks
                        TranslateRotateEnemyCube(client[1]);
                        infoSender.SendEnemyTurningDir(client[1]);
                        yield return new WaitForSeconds(3);
                    }
                    yield return new WaitForSeconds(3);
                }
            }
            else if (playerActionList.GetComponent<ActionStorage>().GetActionListCount() > i) {
                print("host has actions");
                host = playerActionList.GetComponent<ActionStorage>().GetAt(i);

                if (host[0] == "rotate") {
                    //host rotates
                    TranslateRotatePlayerCube(host[1]);
                    infoSender.SendPlayerTurningDir(host[1]);
                }
                else if (host[0] == "attack") {
                    //host attacks
                }
                yield return new WaitForSeconds(3);
            }
            else if (enemyActionList.GetComponent<ActionStorage>().GetActionListCount() > i) {
                print("client has actions");
                client = enemyActionList.GetComponent<ActionStorage>().GetAt(i);

                if (client[0] == "rotate") {
                    //client rotates
                    TranslateRotateEnemyCube(client[1]);
                    infoSender.SendEnemyTurningDir(client[1]);
                }
                else if (client[0] == "attack") {
                    //client attacks
                }
                yield return new WaitForSeconds(3);
            }
            else {
                print("no more actions");
                break;
            }
            yield return new WaitForSeconds(1);
        }

        timeStopped = false;
        state = PLAN;
        remainingTime = ROUND_TIME;
        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);

        mainScreenCanvas.SetActive(true);

        ResetRound();
        infoSender.SendResetRound();
    }

    public void TranslateRotatePlayerCube(string direction) {
        playerCubePosition.transform.GetChild(0).GetComponent<RotateCube>().AssignDirection(direction);
    }
    public void TranslateRotateEnemyCube(string direction) {
        enemyCubePosition.transform.GetChild(0).GetComponent<RotateCube>().AssignDirection(direction);
    }

    void CompareActions(string[] host, string[] client) {
        if (host[0] == client[0]) {
            if (host[0] == "rotate") {
                //rotate both
            }
            else if (host[0] == "attack") {
                //both attack
            }
        }
        else {
            if (host[0] == "rotate") {
                //host rotates, client then attacks
            }
            else if (host[0] == "attack") {
                //rotate client , host then attacks
            }
        }
    }

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

        if (state != SETUP) {
            playerActionList.GetComponent<ActionStorage>().ClearActionList();
            enemyActionList.GetComponent<ActionStorage>().ClearActionList();
        }
        mainScreenCanvas.SetActive(true);
    }

    void SpawnPlayerCube(string[] cubeInfo) {
        playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        GameObject cube = Instantiate(Resources.Load<GameObject>(cubeInfo[0] + "/" + cubeInfo[1] + "/Cube"));
        cube.transform.position = playerCubePosition.transform.position;
        cube.transform.rotation = playerCubePosition.transform.rotation;
        cube.transform.SetParent(playerCubePosition.transform);

        SpawnCubeTargetingSystem("PlayerCubePosition");
    }
    public void SpawnEnemyCube(string[] cubeInfo) {
        enemyCubePosition = GameObject.FindGameObjectWithTag("EnemyCubePosition");

        GameObject cube = Instantiate(Resources.Load<GameObject>(cubeInfo[0] + "/" + cubeInfo[1] + "/Cube"));
        cube.transform.position = enemyCubePosition.transform.position;
        cube.transform.rotation = enemyCubePosition.transform.rotation;
        cube.transform.SetParent(enemyCubePosition.transform);

        SpawnCubeTargetingSystem("EnemyCubePosition");
    }
    void SpawnCubeTargetingSystem(string tag){
        GameObject playerCubePosition = GameObject.FindGameObjectWithTag(tag);

        GameObject cubeTargeting = Instantiate(Resources.Load<GameObject>("BaseCube/CubeTargeting"));
        cubeTargeting.transform.position = playerCubePosition.transform.position;
        cubeTargeting.transform.rotation = playerCubePosition.transform.rotation;
        cubeTargeting.transform.SetParent(playerCubePosition.transform);
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
