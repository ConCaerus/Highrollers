using System.Collections.Generic;
using UnityEngine;

public class LeaderboardCanvas : Singleton<LeaderboardCanvas> {
    [SerializeField] Transform background;
    [SerializeField] List<LeaderboardSlotInstance> slots = new List<LeaderboardSlotInstance>();

    string slotSaveTag = "SlotSaveTag";

    //  loads names and populates slots with info
    void init() {
        for(int i = 0; i < slots.Count; i++) {
            var info = SaveData.getString(slotSaveTag + i.ToString(), "");
            if(string.IsNullOrEmpty(info)) slots[i].setup(new LeaderboardPlayerInfo());
            else slots[i].setup(JsonUtility.FromJson<LeaderboardPlayerInfo>(info));
        }
    }

    public void show() {
        background.gameObject.SetActive(true);
        init();
    }
    public void addNewInfo(LeaderboardPlayerInfo newInfo) {
        //  checks if score is high enough
        if(newInfo.score <= getLowestBoardScore()) return;
        init(); //  populates slots

        //  places info in leaderboard sorted by new score
        for(int i = 0; i < slots.Count; i++) {
            if(newInfo.score > slots[i].reference.score) {  //  finds the correct spot to be in
                var prevInfo = slots[i].reference;
                slots[i].setup(newInfo);
                SaveData.setString(slotSaveTag + i.ToString(), JsonUtility.ToJson(newInfo));
                newInfo = prevInfo; //  replaces the swaps newInfo and previous held info to continue down list
            }
        }
        init(); //  redisplay info
    }

    public void hide() {
        background.gameObject.SetActive(false);
    }

    public float getLowestBoardScore() {
        var data = SaveData.getString(slotSaveTag + (slots.Count - 1).ToString(), "");
        return string.IsNullOrEmpty(data) ? -1 : JsonUtility.FromJson<LeaderboardPlayerInfo>(data).score;
    }

    public bool isScoreRelevant(float score) {
        return score > getLowestBoardScore();
    }
}

[System.Serializable]
public class LeaderboardPlayerInfo {
    public string username;
    public float score;

    public LeaderboardPlayerInfo(string n, float s) {
        username = n;
        score = s;
    }
    public LeaderboardPlayerInfo() {
        username = "------";
        score = 0;
    }
}
