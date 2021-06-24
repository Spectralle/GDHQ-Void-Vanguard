using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;

    private void Update()
    {
        transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime);

        if (transform.position.y < LevelBoundary.D(-2))
            transform.position = new Vector3(Random.Range(LevelBoundary.L(2), LevelBoundary.R(-2)), LevelBoundary.U(2), transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
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
