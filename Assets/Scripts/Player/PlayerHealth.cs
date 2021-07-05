using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _currentLives = 3;
    public int CurrentLives => _currentLives;
    [SerializeField] private GameObject _damage2LivesLeft;
    [SerializeField] private GameObject _damage1LifeLeft;
    [Space]
    [SerializeField] private GameObject _explosionPrefab;


    private void Awake()
    {
        _damage2LivesLeft.SetActive(false);
        _damage1LifeLeft.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && _currentLives > 0)
            Die();
    }

    public void Damage() => Damage(1);

    public void Damage(int livesLost)
    {
        if (_currentLives > 0)
            _currentLives -= livesLost;

        switch (_currentLives)
        {
            case 3:
                UIManager.i.ChangePlayerLives(_currentLives);
                _damage2LivesLeft.SetActive(false);
                _damage1LifeLeft.SetActive(false);
                break;
            case 2:
                UIManager.i.ChangePlayerLives(_currentLives);
                _damage2LivesLeft.SetActive(true);
                _damage1LifeLeft.SetActive(false);
                break;
            case 1:
                UIManager.i.ChangePlayerLives(_currentLives);
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

        GameConclusionHandler.i.Defeat();

        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Projectile"))
        {
            Destroy(other.gameObject);
            Damage();
        }
    }
}
