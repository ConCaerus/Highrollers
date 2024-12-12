using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [SerializeField] List<DiceInstance> dice = new List<DiceInstance>();
    [SerializeField] TextMeshProUGUI score;

    [SerializeField] GameObject buttons1, buttons2, rerollText;
    [SerializeField] Button rerollButton;
    [SerializeField] int maxRerollCount;

    [HideInInspector] public int result;

    Coroutine scoreShower = null;

    bool hasRerolled = false;

    private void Start() {
        score.text = "Test Your Luck!";
        rerollText.SetActive(false);
        buttons1.SetActive(true);
        buttons2.SetActive(false);
    }

    //  buttons
    public void roll() {
        if(scoreShower != null) return;
        bool hasChosen = false; //  checks if there are dice to roll
        foreach(var i in dice) {
            if(i.curState == DiceInstance.dieState.Chosen) {
                hasChosen = true;
                break;
            }
        }
        if(!hasChosen) return;

        score.text = "";
        rerollText.SetActive(false);
        result = 0;
        var showTime = 0f;
        foreach(var i in dice) {
            result += i.roll();
            if(i.curState == DiceInstance.dieState.Chosen && i.getDelay() > showTime)
                showTime = i.getDelay();
        }
        scoreShower = StartCoroutine(showScore(showTime + .5f));
    }
    public void keep() {
        //  asks the player to add their info to the leaderboard if score is good enough
        if(LeaderboardCanvas.I.isScoreRelevant(result))
            NameInputCanvas.I.show();

        //  else just displays leaderboard
        else
            LeaderboardCanvas.I.show();
    }

    public void reset() {
        score.text = "Test Your Luck!";
        buttons1.SetActive(true);
        buttons2.SetActive(false);
        rerollText.SetActive(false);
        hasRerolled = false;
        rerollButton.interactable = true;

        foreach(var i in dice) i.setChosen();
    }

    IEnumerator showScore(float scoreShowDelay) {
        yield return new WaitForSeconds(scoreShowDelay);
        score.text = "You Rolled: <color=yellow>" + result.ToString() + "<color=white>!";
        foreach(var i in dice) i.setIdle();
        buttons1.SetActive(false);
        buttons2.SetActive(true);
        rerollText.SetActive(!hasRerolled);
        rerollButton.interactable = !hasRerolled;
        hasRerolled = true;
        scoreShower = null;
    }

    public bool canChooseDie() {
        var cCount = 0;
        foreach(var i in dice) {
            if(i.curState == DiceInstance.dieState.Chosen) {
                cCount++;
                if(cCount >= maxRerollCount) return false;
            }
        }
        return true;
    }
}
