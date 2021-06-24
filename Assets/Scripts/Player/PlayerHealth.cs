using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _currentLives = 3;


    public void Damage() => Damage(1);

    public void Damage(int livesLost)
    {
        if (_currentLives > 0)
            _currentLives--;

        if (_currentLives <= 0)
            Die();
    }

    public void Die() => Destroy(gameObject);
}
