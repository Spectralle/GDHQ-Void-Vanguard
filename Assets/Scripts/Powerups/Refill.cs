using UnityEngine;


public class Refill : MonoBehaviour
{
    [SerializeField] private RefillType _type;
    [SerializeField] private int _fallSpeed = 3;
    [SerializeField] private AudioClip _powerupAudioClip;


    private void Update()
    {
        transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);
        
        if (transform.position.y < LevelBoundary.D(-2))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (_type)
            {
                case RefillType.Health:
                    collision.gameObject.TryGetComponent(out PlayerHealth plHth);
                    if (plHth)
                        plHth.Heal(1);
                    SpawnManager.i.ItemsInLevel--;
                    Destroy(gameObject);
                    break;
                case RefillType.Ammo:
                    collision.gameObject.TryGetComponent(out PlayerGun plGun);
                    if (plGun)
                        plGun.RefillPrimaryAmmo();
                    SpawnManager.i.ItemsInLevel--;
                    Destroy(gameObject);
                    break;
                case RefillType.None:
                    break;
            }
        }
    }
}