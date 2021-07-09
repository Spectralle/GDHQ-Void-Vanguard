using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] private TextMeshProUGUI _enemiesKilled;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerAmmo;
    [SerializeField] private CanvasGroup _playerThrusterImage;
    [SerializeField] private Image _playerThrusterBarImage;
    [SerializeField] private TextMeshProUGUI _playerLives;
    [SerializeField] private Image _playerLivesImage;
    [Space]
    [SerializeField] private Sprite[] LivesSprites;

    private int _kills;
    private int _score;


    private void Awake()
    {
        if (!_enemiesKilled || !_playerScore || !_playerAmmo || !_playerThrusterImage || !_playerThrusterBarImage ||
            !_playerLives || !_playerLivesImage || LivesSprites.Length == 0)
        {
            Debug.Log("No UI assigned. UI updates disabled.");
            enabled = false;
            return;
        }
        
        i = this;
        _playerThrusterImage.alpha = 0;
    }

    public void ChangeKills(int value) => _enemiesKilled.SetText($"Enemies Killed: {_kills += value}");

    public void ChangeScore(int value) => _playerScore.SetText($"Score: {_score += value}");

    public void ChangeAmmo(int value) => _playerAmmo.SetText($"Ammo: {value}");

    public void ChangeThruster(float value) => _playerThrusterBarImage.fillAmount = value / 100;

    public void ChangeThrusterBarVisibility(float value) => StartCoroutine(ChangeThrusterBarVisibilityIEnum(value));

    private IEnumerator ChangeThrusterBarVisibilityIEnum(float value)
    {
        bool isRaising = _playerThrusterImage.alpha < value;

        if (isRaising)
        {
            for (float a = _playerThrusterImage.alpha; a < value; a += Time.deltaTime * 4)
            {
                _playerThrusterImage.alpha = a;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            for (float a = _playerThrusterImage.alpha; a > value; a -= Time.deltaTime * 4)
            {
                _playerThrusterImage.alpha = a;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void ChangeLives(int value)
    {
        _playerLives.SetText($"Lives: {value}");
        _playerLivesImage.sprite = LivesSprites[value];
    }
}
