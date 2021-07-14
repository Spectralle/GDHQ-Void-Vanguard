using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupType _type;
    [SerializeField] private int _fallSpeed = 3;
    [SerializeField] private int _duration = 5;
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
            collision.gameObject.TryGetComponent(out AudioSource plAS);
            plAS.PlayOneShot(_powerupAudioClip);
            switch (_type)
            {
                case PowerupType.TripleShot:
                    collision.TryGetComponent(out PlayerGun playerGun1);
                    if (playerGun1)
                        playerGun1.ActivatePowerup(_type, _duration);
                    break;
                case PowerupType.SpeedBoost:
                    collision.TryGetComponent(out PlayerMovement playerMovement);
                    if (playerMovement)
                        playerMovement.ActivatePowerup(_type, _duration);
                    collision.TryGetComponent(out PlayerGun playerGun2);
                    if (playerGun2)
                        playerGun2.ActivatePowerup(_type, _duration);
                    break;
                case PowerupType.Shield:
                    collision.TryGetComponent(out PlayerShield playerShield);
                    if (playerShield)
                        playerShield.ActivatePowerup(_type, _duration);
                    break;
                case PowerupType.DynamicLaser:
                    collision.TryGetComponent(out PlayerGun playerGun3);
                    if (playerGun3)
                        playerGun3.ActivatePowerup(_type, _duration);
                    break;
                case PowerupType.None:
                    break;
            }

            SpawnManager.i.ItemsInLevel--;
            Destroy(gameObject);
        }
    }
}
