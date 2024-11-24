using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public static class TextAnimator {
    static Dictionary<int, Coroutine> running = new Dictionary<int, Coroutine>();

    public delegate void func();

    public static void animateText(MonoBehaviour instance, TextMeshProUGUI t, string words, float timeBtwLetters, ASourceInstance a, AudioClip clip, func runOnDone) {
        if(timeBtwLetters == 0f) {
            t.text = words;
            if(runOnDone != null) runOnDone();
        }
        else
            running.Add(instance.GetInstanceID(), instance.StartCoroutine(textAnim(instance.GetInstanceID(), t, words, timeBtwLetters, a, clip, runOnDone)));
    }

    public static void halt(MonoBehaviour instance) {
        if(running.ContainsKey(instance.GetInstanceID())) {
            instance.StopCoroutine(running[instance.GetInstanceID()]);
            running.Remove(instance.GetInstanceID());
        }
    }

    static IEnumerator textAnim(int id, TextMeshProUGUI t, string words, float timeBtwLetters, ASourceInstance a, AudioClip clip, func runOnDone) {
        //  sets up text for the animation
        t.text = "";
        Dictionary<int, string> specialSubs = new Dictionary<int, string>();
        for(int i = 0; i < words.Length; i++) {
            if(words[i] == '<') {
                int ind = i;
                do {
                    i++;
                } while(words[i] != '>');
                i++;
                specialSubs.Add(ind, words.Substring(ind, i - ind));
            }
            t.text += words[i];
        }
        string oriWords = words;
        words = t.text;
        t.color = Color.clear;
        yield return new WaitForEndOfFrame();
        t.color = Color.white;
        int prevInd = -1;
        for(int i = 0; i < words.Length; i++) {
            while(words[i] == ' ' && !specialSubs.ContainsKey(i)) {
                i++;
                prevInd++;
            }
            if(specialSubs.ContainsKey(i)) {
                words = words.Substring(0, i) + specialSubs[i] + words.Substring(i);
                i += specialSubs[i].Length;
            }



            if(i == 0 || prevInd == -1)
                t.text = words.Substring(0, i) + "<alpha=#00>" + words.Substring(i);
            else
                t.text = words.Substring(0, prevInd) + "<alpha=#35>" + words[prevInd] + "<alpha=#00>" + words.Substring(i);
            if(a != null && clip != null)
                a.playSound(clip, true, true, 1f);
            yield return new WaitForSeconds(timeBtwLetters);
            prevInd = i;
        }
        t.text = oriWords;
        running.Remove(id);

        if(runOnDone != null) runOnDone();
    }

    public static bool isAnimating() {
        return running.Count > 0;
    }
}
