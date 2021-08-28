#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            QuitGame();
    }

    public static void InitGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("<color=orange>Game started</color>");

        SpawnManager.Init();
    }

    public static void ResetGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("<color=orange>Game restarted</color>");

        SpawnManager.Reset();
    }

    public static void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}