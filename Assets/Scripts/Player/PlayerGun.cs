using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerGun : MonoBehaviour
{
    #pragma warning disable CS0414
    [Header("Variables")]
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private float _shotCooldown = 0.4f;
    [Space]
    [Header("Weapons")]
    [SerializeField] private GameObject _pfLaser;
    [SerializeField] private GameObject _pfMissile;
    [SerializeField] private float _laserSpeed = 20;
    [SerializeField] private Vector2 _laserDirection = new Vector2(0, 1f);
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _laserFailedAudioClip;
    [SerializeField] private DynamicLaser _dynalaser;
    [SerializeField] private AudioClip _dynaLaserAudioClip;
    [SerializeField] private AudioClip _missileAudioClip;

    public int CurrentAmmo => _currentAmmo;
    private int _currentAmmo;
    private bool _canFire = true;
    private float _cooldownMultiplier = 1;
    private Transform _projectileContainer;
    private AudioSource _audioSource;

    // Powerup checks
    private bool _isAnyPowerupActive;
    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isDynaLaserActive;
    private bool _isHomingMissileActive;
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
        {
            if (_isDynaLaserActive)
                ShootDynaLaser();
            else if (_isHomingMissileActive)
                StartCoroutine(MakeAnAttack(AttackLibrary.Missile.OneForward()));
            else if (_isTripleShotActive)
                StartCoroutine(MakeAnAttack(AttackLibrary.Laser.Free.ThreeForward30()));
            else
                StartCoroutine(MakeAnAttack(AttackLibrary.Ricochet.OneForward()));
        }
    }

    private IEnumerator MakeAnAttack(AttackTemplate attackData)
    {
        if (attackData.AmmoCost > 0)
        {
            if (_currentAmmo < attackData.AmmoCost)
            {
                _audioSource.PlayOneShot(attackData.FailedAudioClip);
                _canFire = false;
                StartCoroutine(ShotCooldown());
                yield break;
            }
        }

        _canFire = false;
        _currentAmmo -= attackData.AmmoCost;
        UIManager.i.ChangeAmmo(_currentAmmo, _ammoCount);

        float angleStep = attackData.Degrees / attackData.Number;
        float angle = attackData.Degrees != 360 ? -(attackData.Degrees - angleStep) / 2 : 0f;
        float transformUpAngle = Mathf.Atan2(transform.up.x, transform.up.y);
        float PIx2 = Mathf.PI * 2;
        Vector3 originPoint = transform.position;

        for (int i = 0; i < attackData.Number; i++)
        {
            Vector2 startPosition = new Vector2(
                Mathf.Sin(((angle * Mathf.PI) / 180) + transformUpAngle),
                Mathf.Cos(((angle * Mathf.PI) / 180) + transformUpAngle)
            );

            Vector2 relativeStartPosition = (Vector2)originPoint + startPosition * attackData.Radius;
            float rotationZ = (360 - angle) - (angle * PIx2 + transformUpAngle) * Mathf.Rad2Deg;
            Vector2 shotMovementVector = (relativeStartPosition - (Vector2)originPoint).normalized * attackData.Speed;
            
            Vector2 relativeCurrentStartPosition = (Vector2)transform.position + startPosition * attackData.Radius;
            Vector2 shotCurrentMovementVector = (relativeCurrentStartPosition - (Vector2)transform.position).normalized * attackData.Speed;

            GameObject shot = Instantiate(
                attackData.Prefab,
                attackData.Delay == 0 ? relativeStartPosition : relativeCurrentStartPosition,
                Quaternion.Euler(0, 0, rotationZ),
                _projectileContainer);
            shot.tag = "Player Projectile";
            shot.GetComponent<Rigidbody2D>().velocity = attackData.Delay == 0 ? shotMovementVector : shotCurrentMovementVector;
            shot.TryGetComponent(out RichochetLaserMovement ricochet);
            ricochet.SetSpeed(attackData.Speed);

            angle += angleStep;

            if (attackData.Delay > 0)
            {
                if (attackData.Delay >= 0.1f && _audioSource && attackData.AudioClip)
                    _audioSource.PlayOneShot(attackData.AudioClip);
                yield return new WaitForSeconds(attackData.Delay);
            }
        }

        if (attackData.Delay < 0.1f && _audioSource && attackData.AudioClip)
            _audioSource.PlayOneShot(attackData.AudioClip);
        StartCoroutine(ShotCooldown());
    }

    public void ShootDynaLaser()
    {
        _canFire = false;
        _dynalaser.ActivateLaser();
        if (_audioSource && _laserAudioClip)
            _audioSource.PlayOneShot(_dynaLaserAudioClip);
        StartCoroutine(ShotCooldown());
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(_shotCooldown * _cooldownMultiplier);
        _canFire = true;
    }

    public void RefillPrimaryAmmo()
    {
        _currentAmmo = _ammoCount;
        UIManager.i.ChangeAmmo(_currentAmmo, _ammoCount);
    }

    public void AddToPrimaryAmmo(int amount)
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo + amount, 0, _ammoCount);
        UIManager.i.ChangeAmmo(_currentAmmo, _ammoCount);
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        StopCoroutine(ManagePowerup(type, duration));
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        if (_isAnyPowerupActive)
            yield break;

        switch (type)
        {
            case PowerupType.TripleShot:
                _isAnyPowerupActive = true;
                _isTripleShotActive = true;
                yield return new WaitForSeconds(duration);
                _isTripleShotActive = false;
                _isAnyPowerupActive = false;
                break;

            case PowerupType.SpeedBoost:
                _isAnyPowerupActive = true;
                _isSpeedBoostActive = true;
                _cooldownMultiplier = 0.7f;
                yield return new WaitForSeconds(duration);
                _cooldownMultiplier = 1;
                _isSpeedBoostActive = false;
                _isAnyPowerupActive = false;
                break;

            case PowerupType.DynamicLaser:
                _isAnyPowerupActive = true;
                _isDynaLaserActive = true;
                _cooldownMultiplier = 8f;
                yield return new WaitForSeconds(duration);
                _cooldownMultiplier = 1;
                _isDynaLaserActive = false;
                _isAnyPowerupActive = false;
                break;

            case PowerupType.HomingMissile:
                _isAnyPowerupActive = true;
                _isHomingMissileActive = true;
                _cooldownMultiplier = 3.5f;
                yield return new WaitForSeconds(duration);
                _cooldownMultiplier = 1;
                _isHomingMissileActive = false;
                _isAnyPowerupActive = false;
                break;
        }
    }
}
