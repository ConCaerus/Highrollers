using System.Collections;
using TMPro;
using UnityEngine;

public class NameInputCanvas : Singleton<NameInputCanvas> {
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Transform background;
    [SerializeField] int maxNameLength;
    string input;

    Coroutine anim = null;

    [SerializeField] float tempScore;

    bool s = false;
    bool shown {
        get { return s; }
        set {
            if(s == value) return;
            s = value;
            background.gameObject.SetActive(s);
            if(s) {
                input = "";
                LeaderboardCanvas.I.hide();
                if(anim == null) anim = StartCoroutine(animWaiter());
            }
            else {
                if(anim != null) {
                    StopCoroutine(anim);
                    anim = null;
                }
            }
        }
    }

    private void Update() {
        if(shown && Input.anyKeyDown) {
            if(Input.GetKeyDown(KeyCode.Backspace)) {
                if(input.Length == 0) return;
                input = input.Substring(0, input.Length - 1);
                nameText.text = nameText.text.Length == input.Length ? input + "I" : input + "<alpha=#00>I";
            }
            else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                if(input.Length == 0) return;
                LeaderboardCanvas.I.addNewInfo(new LeaderboardPlayerInfo(endInput(), tempScore));
                LeaderboardCanvas.I.show();
            }
            else if(input.Length < maxNameLength) {
                for(int i = 97; i < 123; i++) {
                    if(Input.GetKeyDown((KeyCode)i)) {
                        input += ((KeyCode)i).ToString();
                        nameText.text = nameText.text.Length == input.Length ? input + "I" : input + "<alpha=#00>I";
                    }
                }
            }
        }
    }

    public void show() {
        shown = true;
    }

    public string endInput() {
        shown = false;
        return input;
    }

    IEnumerator animWaiter() {
        float flickerTime = .25f;
        while(true) {
            nameText.text = input + "I";
            yield return new WaitForSeconds(flickerTime);
            nameText.text = input + "<alpha=#00>I";
            yield return new WaitForSeconds(flickerTime);
        }
    }
}
