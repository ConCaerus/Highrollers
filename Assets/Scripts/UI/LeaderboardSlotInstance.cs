using TMPro;
using UnityEngine;

public class LeaderboardSlotInstance : MonoBehaviour {
    [SerializeField] TextMeshProUGUI nameText, scoreText;
    public LeaderboardPlayerInfo reference;

    public void setup(LeaderboardPlayerInfo info) {
        reference = info;
        nameText.text = info.username;
        scoreText.text = info.score.ToString("0");
    }
}
