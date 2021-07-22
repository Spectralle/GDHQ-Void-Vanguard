using UnityEngine;


public class Refill : MonoBehaviour
{
    [SerializeField] private RefillType _type;
    [SerializeField] private int _fallSpeed = 3;
    [SerializeField] private AudioClip _powerupAudioClip;

    private Transform _player;
    private float _magnetStrength;


    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        _magnetStrength = PlayerMagnet.MagnetStrength;
    }

    private void Update()
    {
        if (PlayerMagnet.IsMagnetized)
        {
            Vector2 directionToPlayer = (_player.position - transform.position).normalized;
            transform.Translate(directionToPlayer * (_fallSpeed * _magnetStrength) * Time.deltaTime);
        }
        else
            transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);
        
        if (transform.position.y < LevelBoundary.D(-2))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out AudioSource plAS);
            plAS.PlayOneShot(_powerupAudioClip);
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