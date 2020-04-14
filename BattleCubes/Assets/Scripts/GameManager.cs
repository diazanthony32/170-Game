using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

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
        playerInfo = new List<string>();
        playerInfo.Add(PlayerPrefs.GetString("PlayerName"));

        infoSender.SendPlayerStats(ListToStringArray(playerInfo));

        playerCanvas.transform.Find("PlayerName").gameObject.GetComponent<TextMeshProUGUI>().text = playerInfo[0];

        //roundCountText = gameCanvas.transform.Find("RoundCount").gameObject.GetComponent<TextMeshProUGUI>();

        //StartCoroutine(setupCorrutine());
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
            if (state != SETUP) {
                //todo's
            }
            else {
                GetOutOfSetUp();
            }
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

        //if (setupCorrutineRunning == true) {
        //    StopCoroutine(setupCorrutine());
        //    setupCorrutineRunning = false;
        //}
        print("getting out of setup");
        state = PLAN;
        remainingTime = ROUND_TIME;
        timeStopped = false;
        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);

        //Players[0].turnCube.SetBasePos();
        //playerInfoSender.SendSetBasePos();
        //print("position saved");

        //Players[0].SetAttackSideDefault();
        //Players[0].SetTurnSideDefault();

        //time = 0;
        //readyButton.SetActive(false);
        ////rotByFing.rotAllowed = false;
        //rotByFing.SetRotAllowed(false);
        //ResetReadies();
        //yield return new WaitForSeconds(0.0f);
    }

    private void RunTimer() {
        int intTIME = 0;

        if (remainingTime > 0) {
            remainingTime -= Time.deltaTime;
        }
        else {
            timeStopped = true;

            print(state);
            if (state == PLAN) {
                outtaTimeCanvas.GetComponent<TweenController>().Notify();
                infoSender.SendNotification(TIME_UP);
                StartCoroutine(DoThrowdown());
            }
            else if (state == SETUP) {
                GetOutOfSetUp();
            }
        }
        intTIME = Mathf.CeilToInt(remainingTime);
        remainingTimeText.text = intTIME.ToString();
        infoSender.SendGameStatus(new int[] { intTIME, roundCount, state });
    }

    IEnumerator DoThrowdown() {
        state = THROWDOWN;
        print("inside CORRUTINE");
        yield return new WaitForSeconds(3);
        timeStopped = false;
        state = PLAN;
        remainingTime = ROUND_TIME;
        roundCount++;
        roundCountText.text = ConvertNumToText(roundCount);
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

    //get set
    public GameObject GetEnemyCanvas() {
        return enemyCanvas;
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
