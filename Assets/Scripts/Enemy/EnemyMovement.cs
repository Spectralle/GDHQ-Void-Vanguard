using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;

    private void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (transform.position.y < LevelBoundary.D(-2))
        {
            if (SpawnManager.CanSpawn)
                transform.position = SpawnManager.GetSpawnPosition();
            else
                Destroy(gameObject);
        }
    }

    private void OnDestroy() => SpawnManager.EnemiesAlive--;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            UIManager.i.AddToEnemiesKilled(1);
            UIManager.i.AddToScore(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerHealth playerHealth);
            if (playerHealth)
                playerHealth.Damage(1);
            Destroy(gameObject);
        }

        if (other.CompareTag("Shield"))
        {
            other.GetComponentInParent<PlayerShield>().StopAllCoroutines();
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
