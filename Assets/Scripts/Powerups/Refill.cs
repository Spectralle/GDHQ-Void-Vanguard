using System.Collections;
using UnityEngine;

public class Refill : MonoBehaviour, IPickup
{
    [SerializeField] private RefillType _type;
    [SerializeField] private float _fallSpeed = 3;
    [SerializeField] private AudioClip _powerupAudioClip;
    [SerializeField] private GameObject _explosionPrefab;

    private Transform _player;
    private float _magnetStrength;
    private LineRenderer _magnetLineRenderer;
    private bool _isDestroyed;


    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        _magnetStrength = PlayerMagnet.MagnetStrength;
        _magnetLineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (PlayerMagnet.IsMagnetized)
        {
            _magnetLineRenderer.enabled = true;
            _magnetLineRenderer.SetPosition(0, transform.position);
            _magnetLineRenderer.SetPosition(1, _player.position);
            Vector2 directionToPlayer = (_player.position - transform.position).normalized;
            transform.Translate(directionToPlayer * (_fallSpeed * _magnetStrength) * Time.deltaTime);
        }
        else
        {
            _magnetLineRenderer.enabled = false;
            transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);
        }
        
        if (transform.position.y < LevelBoundary.D(-3) && !_isDestroyed)
            DestroyThis(false);
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
                    break;
                case RefillType.Ammo:
                    collision.gameObject.TryGetComponent(out PlayerGun plGun);
                    if (plGun)
                        plGun.RefillPrimaryAmmo();
                    break;
                case RefillType.None:
                    break;
            }

            DestroyThis(false);
        }

        else if (collision.CompareTag("Enemy Projectile"))
        {
            Destroy(collision.gameObject);
            DestroyThis(true);
        }
    }

    public void DestroyThis(bool willExplode)
    {
        SpawnManager.ChangeItemsExist(transform);
        if (!willExplode)
            Destroy(gameObject);
        else
            StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        _isDestroyed = true;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity, transform);

        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject, 2.6f);

        while (_fallSpeed > 0.00f)
        {
            _fallSpeed = Mathf.Lerp(_fallSpeed, 0, Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
    }
}