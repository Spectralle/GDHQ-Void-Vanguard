using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] private TextMeshProUGUI _enemiesKilled;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerLives;
    [SerializeField] private Image _playerLivesImage;
    [Space]
    [SerializeField] private Sprite[] LivesSprites;

    private int _enemiesKilledValue;
    private int _playerScoreValue;
    private int _playerLivesValue;



    private void Awake()
    {
        if (!_enemiesKilled || !_playerScore || !_playerLives || !_playerLivesImage)
        {
            Debug.Log("No UI assigned. UI updates disabled.");
            enabled = false;
            return;
        }
        
        i = this;
        _playerLivesValue = FindObjectOfType<PlayerHealth>().CurrentLives;
        ChangePlayerLives(_playerLivesValue);
    }

    public void AddToEnemiesKilled(int amount) => _enemiesKilled.SetText($"Enemies Killed: {_enemiesKilledValue += amount}");

    public void AddToScore(int amount) => _playerScore.SetText($"Score: {_playerScoreValue += amount}");

    public void ChangePlayerLives(int currentLives)
    {
        _playerLives.SetText($"Score: {_playerLivesValue += currentLives}");
        _playerLivesImage.sprite = LivesSprites[currentLives];
    }
}
