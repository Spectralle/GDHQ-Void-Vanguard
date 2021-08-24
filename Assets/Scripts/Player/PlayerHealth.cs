using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int CurrentLives => _currentLives;
    public void OverrideLives(int amount) => _currentLives = amount;
    #pragma warning disable CS0649
    [SerializeField] private int _currentLives = 3;
    [Space]
    [SerializeField] private GameObject _damage2LivesLeft;
    [SerializeField] private GameObject _damage1LifeLeft;
    [Space]
    [SerializeField] private GameObject _explosionPrefab;
    #pragma warning restore CS0649

    private UnitShield _shield;


    private void Awake()
    {
        _damage2LivesLeft.SetActive(false);
        _damage1LifeLeft.SetActive(false);

        TryGetComponent(out _shield);
    }

    private void Start() => UIManager.i.ChangeLives(_currentLives);

    public void Damage() => Damage(1);

    public void Damage(int livesLost)
    {
        if (_shield)
        {
            if (_shield.IsActive)
                return;
        }

        if (_currentLives > 0)
            _currentLives -= livesLost;

        if (_currentLives < 0)
            _currentLives = 0;

        switch (_currentLives)
        {
            case 3:
                CameraShaker.StartShaking(.7f, .3f);
                UIManager.i.ChangeLives(_currentLives);
                _damage2LivesLeft.SetActive(false);
                _damage1LifeLeft.SetActive(false);
                break;
            case 2:
                CameraShaker.StartShaking(.7f, .3f);
                UIManager.i.ChangeLives(_currentLives);
                _damage2LivesLeft.SetActive(true);
                _damage1LifeLeft.SetActive(false);
                break;
            case 1:
                CameraShaker.StartShaking(.7f, .3f);
                UIManager.i.ChangeLives(_currentLives);
                _damage2LivesLeft.SetActive(false);
                _damage1LifeLeft.SetActive(true);
                break;
            case 0:
                CameraShaker.StartShaking(1.5f, .6f);
                Die();
                break;
        }
    }

    public void Heal() => Heal(3 - _currentLives);

    public void Heal(int livesToHeal)
    {
        if (_currentLives < 3)
            _currentLives += livesToHeal;

        switch (_currentLives)
        {
            case 3:
                UIManager.i.ChangeLives(_currentLives);
                _damage2LivesLeft.SetActive(false);
                _damage1LifeLeft.SetActive(false);
                break;
            case 2:
                UIManager.i.ChangeLives(_currentLives);
                _damage2LivesLeft.SetActive(true);
                _damage1LifeLeft.SetActive(false);
                break;
            case 1:
                UIManager.i.ChangeLives(_currentLives);
                _damage2LivesLeft.SetActive(false);
                _damage1LifeLeft.SetActive(true);
                break;
            case 0:
                Die();
                break;
        }
    }

    public void Die()
    {
        _currentLives = 0;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameObject.Find("Game Handler/Scene").transform);

        GetComponent<PlayerMovement>().enabled = false;

        GameConclusionHandler.i.Defeat();

        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Projectile"))
        {
            CameraShaker.StartShaking(.8f, .3f);
            Damage();
        }
    }
}
