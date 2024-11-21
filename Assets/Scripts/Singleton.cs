using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Object {
    public static T I { get; private set; }

    protected virtual void Awake() {
        I = FindObjectOfType<T>();
    }
}