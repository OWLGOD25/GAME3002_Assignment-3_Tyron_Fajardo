using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneTransitions : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Loading Game...");
    }

    // Exit Game
    public void Quit()
    {
#if UNITY_EDITOR
        // Stop play mode when testing inside the Unity Editor
        EditorApplication.isPlaying = false;
#else
        // Quit the standalone build
        Application.Quit();
#endif
        Debug.Log("Game Quit...");
    }
}
