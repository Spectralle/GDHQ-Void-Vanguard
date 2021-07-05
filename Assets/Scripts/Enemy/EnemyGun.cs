using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyGun : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Vector2 _shotCooldown = new Vector2(2.5f, 7);
    [Space]
    [SerializeField] private Vector2 _shotPoint1 = new Vector2(-0.219f, -1.096f);
    [SerializeField] private Vector2 _shotPoint2 = new Vector2(0.219f, -1.096f);
    [Space]
    [Header("Weapons")]
    [SerializeField] private GameObject _pfLaser;
    [SerializeField] private Vector2 _laserSpeed = new Vector2(0, -4f);
    [SerializeField] private AudioClip _laserAudioClip;

    private Transform _projectileContainer;
    private bool _canFire = true;
    private float _cooldownMultiplier = 1;
    private AudioSource _asrc;


    private void Awake()
    {
        _projectileContainer = GameObject.Find("Projectile Container").transform;
        if (!_projectileContainer)
            Debug.LogWarning("No container object set for projectiles");
        _asrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_canFire && !GetComponent<EnemyMovement>().IsDestroyed)
            ShootLaser();
    }

    private Vector3 GetShotSpawnPoint(int pointIndex)
    {
        switch (pointIndex)
        {
            default:
            case 1: return (Vector2)transform.position + _shotPoint1;
            case 2: return (Vector2)transform.position + _shotPoint2;
        }
    }

    public void ShootLaser()
    {
        _canFire = false;

        Instantiate(_pfLaser, GetShotSpawnPoint(1), Quaternion.identity, _projectileContainer)
                .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);
        Instantiate(_pfLaser, GetShotSpawnPoint(2), Quaternion.identity, _projectileContainer)
                .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);

        if (_asrc && _laserAudioClip)
            _asrc.PlayOneShot(_laserAudioClip);

        StartCoroutine(ShotCooldown());
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(Random.Range(_shotCooldown.x, _shotCooldown.y) * _cooldownMultiplier);
        _canFire = true;
    }


    private void OnDrawGizmosSelected()
    {
        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint1, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint2, 0.02f);
    }
}