using UnityEngine;


public class WeaponInstanceMovement : MonoBehaviour
{
    [SerializeField] protected float Speed = 8f;
    [SerializeField] protected float Lifetime = 2f;

    void Awake() => Destroy(gameObject, Lifetime);

    private void Update() => transform.Translate(Vector3.up * Speed * Time.deltaTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}