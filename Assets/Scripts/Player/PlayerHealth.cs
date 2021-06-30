using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _currentLives = 3;
    public int CurrentLives => _currentLives;


    public void Damage() => Damage(1);

    public void Damage(int livesLost)
    {
        if (_currentLives > 0)
            _currentLives--;

        UIManager.i.ChangePlayerLives(_currentLives);

        if (_currentLives <= 0)
            Die();
    }

    public void Die() => Destroy(gameObject);
}
