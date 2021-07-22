using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] private TextMeshProUGUI _enemiesKilled;
    [Space]
    [SerializeField] private TextMeshProUGUI _playerScore;
    [Space]
    [SerializeField] private TextMeshProUGUI _playerAmmoText;
    [SerializeField] private Image _playerAmmoFill;
    [Space]
    [SerializeField] private Image _playerMagnetFill;
    [Space]
    [SerializeField] private Image _playerThrusterBarImage;
    [Space]
    [SerializeField] private TextMeshProUGUI _playerLives;
    [SerializeField] private Image _playerLivesImage;
    [SerializeField] private Sprite[] LivesSprites;

    private int _kills;
    private int _score;


    private void Awake()
    {
        if (!_enemiesKilled || !_playerScore || !_playerAmmoFill || !_playerThrusterBarImage ||
            !_playerLives || !_playerLivesImage || LivesSprites.Length == 0)
        {
            Debug.Log("No UI assigned. UI updates disabled.");
            enabled = false;
            return;
        }
        
        i = this;

        ChangeScore(0);
        ChangeKills(0);
        ChangeThruster(100);
    }

    public void ChangeKills(int value) => _enemiesKilled.SetText($"Kills: {_kills += value}");

    public void ChangeScore(int value) => _playerScore.SetText($"Score: {_score += value}");

    public void ChangeAmmo(float value, float max)
    {
        _playerAmmoText.SetText($"{value}/{max}");
        StartCoroutine(SmoothChangeAmmoFillAmount((1 / max) * value));
    }

    public void ChangeMagnet(float value, float max) => StartCoroutine(SmoothChangeMagnetFillAmount((1 / max) * value));

    public void ChangeThruster(float value) => _playerThrusterBarImage.fillAmount = value / 100;

    public void ChangeLives(int value)
    {
        _playerLives.SetText($"Lives: {value}");
        _playerLivesImage.sprite = LivesSprites[value];
    }


    private IEnumerator SmoothChangeAmmoFillAmount(float value)
    {
        bool isRaising = _playerAmmoFill.fillAmount < value;

        if (!isRaising)
        {
            while (_playerAmmoFill.fillAmount > value)
            {
                _playerAmmoFill.fillAmount -= (Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_playerAmmoFill.fillAmount < value)
            {
                _playerAmmoFill.fillAmount += (Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator SmoothChangeMagnetFillAmount(float value)
    {
        bool isRaising = _playerMagnetFill.fillAmount < value;

        if (!isRaising)
        {
            while (_playerMagnetFill.fillAmount > value)
            {
                _playerMagnetFill.fillAmount -= (Time.deltaTime * 5);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_playerMagnetFill.fillAmount < value)
            {
                _playerMagnetFill.fillAmount += (Time.deltaTime * 5);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
