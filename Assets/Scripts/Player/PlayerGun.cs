using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerGun : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private float _shotCooldown = 0.4f;
    [Space]
    [SerializeField] private Vector2 _shotPoint1 = new Vector2(0, 0.75f);
    [SerializeField] private Vector2 _shotPoint2 = new Vector2(-0.785f, -0.61f);
    [SerializeField] private Vector2 _shotPoint3 = new Vector2(0.785f, -0.61f);
    [Space]
    [Header("Weapons")]
    [SerializeField] private GameObject _pfLaser;
    [SerializeField] private Vector2 _laserSpeed = new Vector2(0, 8f);
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _laserFailedAudioClip;

    #pragma warning disable CS0414
    public int CurrentAmmo => _currentAmmo;
    private int _currentAmmo;
    private bool _canFire = true;
    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private float _cooldownMultiplier = 1;
    private Transform _projectileContainer;
    private AudioSource _audioSource;
    #pragma warning restore CS0414


    private void Awake()
    {
        _projectileContainer = GameObject.Find("Projectile Container").transform;
        if (!_projectileContainer)
            Debug.LogWarning("No container object set for projectiles");
        _audioSource = GetComponent<AudioSource>();

        _currentAmmo = _ammoCount;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _canFire)
            ShootLaser();
    }

    public void ShootLaser()
    {
        if (_currentAmmo == 0)
        {
            _audioSource.PlayOneShot(_laserFailedAudioClip);
            _canFire = false;
            StartCoroutine(ShotCooldown());
            return;
        }

        _canFire = false;
        _currentAmmo--;
        UIManager.i.ChangeAmmo(_currentAmmo);
        if (!_isTripleShotActive)
            Instantiate(_pfLaser, GetShotSpawnPoint(1), Quaternion.identity, _projectileContainer)
                .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);
        else
        {
            Instantiate(_pfLaser, GetShotSpawnPoint(1), Quaternion.identity, _projectileContainer)
                .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);
            Instantiate(_pfLaser, GetShotSpawnPoint(2), Quaternion.identity, _projectileContainer)
                .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);
            Instantiate(_pfLaser, GetShotSpawnPoint(3), Quaternion.identity, _projectileContainer)
                .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);
        }
        if (_audioSource && _laserAudioClip)
            _audioSource.PlayOneShot(_laserAudioClip);
        StartCoroutine(ShotCooldown());
    }

    private Vector3 GetShotSpawnPoint(int pointIndex)
    {
        switch (pointIndex)
        {
            default:
            case 1: return (Vector2)transform.position + _shotPoint1;
            case 2: return (Vector2)transform.position + _shotPoint2;
            case 3: return (Vector2)transform.position + _shotPoint3;
        }
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(_shotCooldown * _cooldownMultiplier);
        _canFire = true;
    }

    public void RefillPrimaryAmmo()
    {
        _currentAmmo = _ammoCount;
        UIManager.i.ChangeAmmo(_currentAmmo);
    }

    public void AddToPrimaryAmmo(int amount)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _ammoCount);
        UIManager.i.ChangeAmmo(_currentAmmo);
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        StopCoroutine(ManagePowerup(type, duration));
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        switch (type)
        {
            case PowerupType.TripleShot:
                _isTripleShotActive = true;
                yield return new WaitForSeconds(duration);
                _isTripleShotActive = false;
                break;
            case PowerupType.SpeedBoost:
                _isSpeedBoostActive = true;
                _cooldownMultiplier = 0.7f;
                yield return new WaitForSeconds(duration);
                _cooldownMultiplier = 1;
                _isSpeedBoostActive = false;
                break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint1, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint2, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint3, 0.02f);
    }
}
