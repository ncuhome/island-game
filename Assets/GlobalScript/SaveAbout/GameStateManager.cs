using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ±£¥Ê”Œœ∑¥Êµµ∞¢∞Õ∞¢∞Õ
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance = null;
    public bool isLoadData { get; private set; } = false;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        isLoadData = Saver.LoadJsonSave();
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationPause(bool pause) {
        if (pause) {
            Saver.SaveJson();
        }
    }

    private void OnApplicationQuit() {
        Saver.SaveJson();
    }
}
