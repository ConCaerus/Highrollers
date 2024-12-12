using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiceInstance : MonoBehaviour {
    [SerializeField] Sprite[] faces;
    [SerializeField] Image image;
    [SerializeField] float rollTime;

    Coroutine anim = null;

    [HideInInspector] public int result = 0;

    dieState cs = dieState.Chosen;
    public dieState curState {
        get { return cs; }
        private set {
            cs = value;
            image.color = cs == dieState.Chosen ? Color.white : Color.gray;
        }
    }
    
    //  for rerolling
    public enum dieState {
        None, Idle, Chosen
    }

    public int roll() {
        if(curState != dieState.Chosen) return result;
        if(anim != null) StopCoroutine(anim);
        result = Random.Range(1, 7);
        anim = StartCoroutine(rollAnim());
        return result;
    }

    IEnumerator rollAnim() {
        var changeCount = 50;
        var changeTime = rollTime / changeCount;
        for(int i = 0; i < changeCount; i++) {
            image.sprite = faces[Random.Range(0, faces.Length)];
            var elapsedTime = 0f;
            var startTime = Time.time;
            while(elapsedTime < changeTime) {
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.time - startTime;
                startTime = Time.time;
                var perc = elapsedTime / changeTime;

                transform.eulerAngles = Vector3.forward * 8.5f * perc * (i % 2 == 0 ? 1f : -1f);
            }
        }

        transform.DOPunchScale(Vector2.one * 1.01f, .25f);
        transform.eulerAngles = Vector3.zero;
        image.sprite = faces[result - 1];
        anim = null;
    }

    public void setIdle() {
        curState = dieState.Idle;
    }
    public void toggleState() {
        curState = curState == dieState.Chosen ? dieState.Idle : dieState.Chosen;
    }
    public void setChosen() {
        curState = dieState.Chosen;
    }

    public float getDelay() {
        return rollTime;
    }
}
