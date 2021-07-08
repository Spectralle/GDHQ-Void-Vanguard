using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConclusionHandler : MonoBehaviour
{
    public static GameConclusionHandler i;

    [SerializeField] private GameObject _victoryText;
    [SerializeField] private GameObject _defeatText;
    [SerializeField] private GameObject _gameplayUIContainer;

    private bool _isDefeated;
    private bool _isVictorious;


    private void Awake()
    {
        if (!_victoryText || !_defeatText || !_gameplayUIContainer)
        {
            Debug.LogError("Missing variables on the GameConclusionHandler. Disabling...");
            enabled = false;
            return;
        }

        i = this;
    }

    private void Update()
    {
        if (_isDefeated || _isVictorious)
        {
            if (Input.GetKeyDown(KeyCode.R))
                GameSceneManager.ResetGame();

            if (Input.GetKeyDown(KeyCode.Q))
                GameSceneManager.QuitGame();
        }
    }

    public void Victory()
    {
        _gameplayUIContainer.SetActive(false);
        _victoryText.SetActive(true);
        _defeatText.SetActive(false);
        _isVictorious = true;
    }

    public void Defeat()
    {
        _gameplayUIContainer.SetActive(false);
        _victoryText.SetActive(false);
        _defeatText.SetActive(true);
        _isDefeated = true;
    }

    public void Reset()
    {
        _gameplayUIContainer.SetActive(true);
        _victoryText.SetActive(false);
        _defeatText.SetActive(false);
        _isDefeated = false;
        _isVictorious = false;
    }
}
