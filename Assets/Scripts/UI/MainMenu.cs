using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame() => GameSceneManager.ResetGame();

    public void QuitGame() => GameSceneManager.QuitGame();
}
