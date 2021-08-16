using UnityEngine;


public class WeaponInstanceMovement : MonoBehaviour
{
    protected float _lifetime = 5f;


    void Awake() => Destroy(gameObject, _lifetime);

    public void SetParameters(float lifetime = 5f) => _lifetime = lifetime;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && CompareTag("Player Projectile"))
            Destroy(gameObject);
        else if (other.CompareTag("Player") && CompareTag("Enemy Projectile"))
            Destroy(gameObject);
    }
}