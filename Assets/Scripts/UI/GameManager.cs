using UnityEngine;

public class GameManager : Singleton<GameManager> {

    //  buttons
    public void roll() {
        //  rolls the dice
    }
    public void keep() {
        float tempScore = 3f;   //  change this to sum of dice when there's dice

        //  asks the player to add their info to the leaderboard if score is good enough
        if(LeaderboardCanvas.I.isScoreRelevant(tempScore))
            NameInputCanvas.I.show();

        //  else just displays leaderboard
        else
            LeaderboardCanvas.I.show();
    }
}
