using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASourceInstance : MonoBehaviour {
    [SerializeField] AudioSource source;
    [SerializeField] bool music = false;

    private void OnEnable() {
        OptionsCanvas.settingsChanged += updateSettings;
        StartCoroutine(initWaiter());
    }

    private void OnDisable() {
        OptionsCanvas.settingsChanged -= updateSettings;
    }

    public void updateSettings() {
        source.volume = music ? OptionsCanvas.I.getMusicVolume() : OptionsCanvas.I.getSFXVolume();
    }

    IEnumerator initWaiter() {
        while(OptionsCanvas.I == null)
            yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        updateSettings();
    }

    public void playSound(AudioClip clip, bool playOnListener, bool randomize, float volMod) {
        source.volume = (music ? OptionsCanvas.I.getMusicVolume() : OptionsCanvas.I.getSFXVolume()) * volMod;
        source.pitch = randomize ? Random.Range(0.75f, 1.25f) : 1f;

        if(playOnListener)
            transform.position = FindFirstObjectByType<AudioListener>().transform.position;
        source.clip = clip;
        source.Play();
    }
    public void modVolume(float volMod) {
        source.volume = (music ? OptionsCanvas.I.getMusicVolume() : OptionsCanvas.I.getSFXVolume()) * volMod;
    }
    public float getCurVolumeMod() {
        return source.volume / (music ? OptionsCanvas.I.getMusicVolume() : OptionsCanvas.I.getSFXVolume());
    }
    public void stopPlaying() {
        source.Stop();
    }

    public bool isPlaying() {
        return source.isPlaying;
    }
    public AudioSource getSource() {
        return source;
    }
}
