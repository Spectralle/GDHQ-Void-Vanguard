using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame() => GameSceneManager.InitGame();

    public void QuitGame() => GameSceneManager.QuitGame();
}
