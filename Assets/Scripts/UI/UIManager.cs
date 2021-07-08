using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] private TextMeshProUGUI _enemiesKilled;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerAmmo;
    [SerializeField] private TextMeshProUGUI _playerLives;
    [SerializeField] private Image _playerLivesImage;
    [Space]
    [SerializeField] private Sprite[] LivesSprites;

    private int _kills;
    private int _score;


    private void Awake()
    {
        if (!_enemiesKilled || !_playerScore || !_playerLives || !_playerLivesImage)
        {
            Debug.Log("No UI assigned. UI updates disabled.");
            enabled = false;
            return;
        }
        
        i = this;
    }

    public void ChangeKills(int value) => _enemiesKilled.SetText($"Enemies Killed: {_kills += value}");

    public void ChangeScore(int value) => _playerScore.SetText($"Score: {_score += value}");

    public void ChangeAmmo(int value) => _playerAmmo.SetText($"Ammo: {value}");

    public void ChangeLives(int value)
    {
        _playerLives.SetText($"Lives: {value}");
        _playerLivesImage.sprite = LivesSprites[value];
    }
}
