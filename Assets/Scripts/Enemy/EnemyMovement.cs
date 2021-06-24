using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;

    private void Update()
    {
        transform.Translate(-Vector3.up * _moveSpeed * Time.deltaTime);

        if (transform.position.y < LevelBoundary.D(-2))
        {
            if (EnemySpawnManager.CanSpawn)
                transform.position = EnemySpawnManager.GetSpawnPosition();
            else
                Destroy(gameObject);
        }
    }

    private void OnDestroy() => EnemySpawnManager.EnemiesAlive--;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerHealth playerHealth);
            playerHealth?.Damage(1);
            Destroy(gameObject);
        }
    }
}
