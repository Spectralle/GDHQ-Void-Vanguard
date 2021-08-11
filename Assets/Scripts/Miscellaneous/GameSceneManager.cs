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

    public static void ResetGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Game restarted");

        if (SpawnManager.i)
            SpawnManager.i.CanSpawn = true;

        SpawnManager.Reset();
    }

    public static void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
}