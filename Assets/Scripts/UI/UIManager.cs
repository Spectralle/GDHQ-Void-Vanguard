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
    [SerializeField] private TextMeshProUGUI _waveNumber;
    [Space]
    [SerializeField] private Image _playerAmmoFill;
    [SerializeField] private TextMeshProUGUI _playerAmmoText;
    [SerializeField] private RectTransform[] _playerAmmoIcons;
    [Space]
    [SerializeField] private Image _playerMagnetFill;
    [SerializeField] private Image _playerMagnetAvailable;
    [Space]
    [SerializeField] private Image _playerThrusterFill;
    [Space]
    [SerializeField] private TextMeshProUGUI _playerLives;
    [SerializeField] private Image _playerLivesImage;
    [SerializeField] private Sprite[] LivesSprites;

    private int _kills;
    private int _score;


    private void Awake()
    {
        if (!_enemiesKilled || !_playerScore || !_playerAmmoFill || !_playerThrusterFill ||
            !_playerMagnetFill || !_playerLives || !_playerLivesImage || LivesSprites.Length == 0)
        {
            Debug.Log("No UI assigned. UI updates disabled.");
            enabled = false;
            return;
        }
        
        i = this;

        ChangeScore(0);
        ChangeKills(0);
        ChangeWave(0);
    }

    public void ChangeKills(int value) => _enemiesKilled.SetText($"{_kills += value}");

    public void ChangeScore(int value) => _playerScore.SetText($"{_score += value}");

    public void ChangeWave(int value) => _waveNumber.SetText($"{(value == 0 ? "N/A" : value.ToString())}");

    public void ChangeAmmo(float value, float max)
    {
        if (value != -1 && max != -1)
            _playerAmmoText.SetText($"{value}/{max}");
        else
            _playerAmmoText.SetText("∞ / ∞");
        foreach (RectTransform icon in i._playerAmmoIcons)
        {
            float width = (value * (64 - 23.48f) / max) + 23.48f;
            icon.sizeDelta = new Vector2(width, icon.sizeDelta.y);
        }

        StartCoroutine(SmoothChangeAmmoFillAmount((1 / max) * value));
    }

    public void ChangeMagnet(float value)
    {
        _playerMagnetFill.fillAmount = value / 100;
        if (value == 100)
            _playerMagnetAvailable.enabled = false;
        else
            _playerMagnetAvailable.enabled = true;
    }

    public void ChangeThruster(float value) => _playerThrusterFill.fillAmount = value / 100;

    public void ChangeLives(int value)
    {
        _playerLives.SetText($"{value}");
        _playerLivesImage.sprite = LivesSprites[Mathf.Clamp(value, 0, LivesSprites.Length - 1)];
    }

    public void EnableHealthSprites() => _playerLivesImage.enabled = true;

    public void DisableHealthSprites() => _playerLivesImage.enabled = false;


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
}
