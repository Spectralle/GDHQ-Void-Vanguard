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
        SpawnManager.i.CanSpawn = true;
    }

    public static void QuitGame() => Application.Quit();
}