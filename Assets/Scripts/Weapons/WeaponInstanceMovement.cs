using UnityEngine;


public class WeaponInstanceMovement : MonoBehaviour
{
    protected float _lifetime = 2.5f;


    void Awake() => Destroy(gameObject, _lifetime);

    public void SetParameters(float lifetime) => _lifetime = lifetime;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("Enemy Projectile") && other.CompareTag("Player"))
            Destroy(gameObject);
    }
}