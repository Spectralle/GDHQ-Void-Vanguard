using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(LineRenderer))]
public class EnemyGun : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private bool _canShoot = true;
    [SerializeField] private Vector2 _shotCooldown = new Vector2(2.5f, 7);
    [Space]
    [SerializeField] private Vector2 _shotPoint1 = new Vector2(-0.219f, -1.096f);
    [SerializeField] private Vector2 _shotPoint2 = new Vector2(0.219f, -1.096f);
    [Header("Weapons")]
    [SerializeField] private GameObject _pfLaser;
    [SerializeField] private Vector2 _laserSpeed = new Vector2(0, -4f);
    [SerializeField] private AudioClip _laserAudioClip;
    [Header("Anti-Item Laser")]
    public APType AntiPowerupType;
    public enum APType
    {
        Basic,
        Advanced
    }
    [SerializeField, Range(0, 10)] private int _activationChance = 1;
    [SerializeField, Range(0, 1.5f)] private float _shotDelay = 1f;
    [SerializeField, Range(0, 1f)] private float _shotDuration = 0.5f;

    private Transform _projectileContainer;
    private bool _readyToShoot = true;
    private float _cooldownMultiplier = 1;
    private AudioSource _asrc;
    private LineRenderer _antiItemLaserRenderer;
    private float _antiItemTimer;


    private void Awake()
    {
        _projectileContainer = GameObject.Find("Projectile Container").transform;
        if (!_projectileContainer)
            Debug.LogWarning("No container object set for projectiles");
        _asrc = GetComponent<AudioSource>();
        _antiItemLaserRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (_readyToShoot && !GetComponent<EnemyMovement>().IsDestroyed)
            ShootLaser();

        if (AntiPowerupType == APType.Basic)
            ShootBasicAntiItemLaser();
    }

    #region Default Laser
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
        if (!_canShoot)
            return;

        _readyToShoot = false;

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
        _readyToShoot = true;
    }
    #endregion

    #region Anti-Item Laser
    #region Basic
    public void ShootBasicAntiItemLaser()
    {
        if (Random.Range(0, 10) > _activationChance)
            return;

        if (_antiItemTimer < 1f)
            _antiItemTimer += Time.deltaTime;
        else
        {
            _antiItemTimer = 0;

            RaycastHit2D hit1 = Physics2D.Raycast((Vector2)transform.position + _shotPoint1, Vector2.down);
            if (hit1)
            {
                if (hit1.transform.CompareTag("Powerup"))
                {
                    Instantiate(_pfLaser, (Vector2)transform.position + _shotPoint1, Quaternion.identity, _projectileContainer)
                    .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);

                    if (_asrc && _laserAudioClip)
                        _asrc.PlayOneShot(_laserAudioClip);
                }
            }

            RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position + _shotPoint2, Vector2.down);
            if (hit2)
            {
                if (hit2.transform.CompareTag("Powerup"))
                {
                    Instantiate(_pfLaser, (Vector2)transform.position + _shotPoint2, Quaternion.identity, _projectileContainer)
                        .GetComponent<LaserMovement>().SetMovementDirection(_laserSpeed);

                    if (_asrc && _laserAudioClip)
                        _asrc.PlayOneShot(_laserAudioClip);
                }
            }
        }

        Debug.DrawRay((Vector2)transform.position + _shotPoint1, Vector2.down * 15);
        Debug.DrawRay((Vector2)transform.position + _shotPoint2, Vector2.down * 15);
    }
    #endregion

    #region Advanced
    public void ShootAdvAntiItemLaser()
    {
        if (Random.Range(0, 10) <= _activationChance)
            StartCoroutine(ShootAdvancedAntiItemLaser());
    }

    private List<Transform> GetItemsBelowMe()
    {
        if (SpawnManager.ItemsExist)
        {
            List<Transform> _itemsBelowMe = new List<Transform>();

            foreach (Transform t in SpawnManager.ItemList)
            {
                if (t && t.position.y < (transform.position.y - 4))
                    _itemsBelowMe.Add(t);
            }

            return _itemsBelowMe;
        }
        else
            return new List<Transform>();
    }

    private Transform GetClosestItem()
    {
        List<Transform> _itemsBelowMe = GetItemsBelowMe();
        if (_itemsBelowMe.Count == 0)
            return null;

        Transform closestTarget = _itemsBelowMe[0];
        for (int i = 1; i < _itemsBelowMe.Count; i++)
        {
            if (Vector2.Distance(transform.position, _itemsBelowMe[i].position) <
                Vector2.Distance(transform.position, closestTarget.position))
                closestTarget = _itemsBelowMe[i];
        }

        return closestTarget;
    }

    private Vector2 GetClosestGun(Vector2 targetPosition)
    {
        Vector2 activeShotPoint;
        if (Vector2.Distance(_shotPoint1, targetPosition) < Vector2.Distance(_shotPoint2, targetPosition))
            activeShotPoint = _shotPoint1 + new Vector2(0, 0.15f);
        else
            activeShotPoint = _shotPoint2 + new Vector2(0, 0.15f);
        return activeShotPoint;
    }

    private IEnumerator ShootAdvancedAntiItemLaser()
    {
        yield return new WaitForSeconds(_shotDelay);

        Transform target = GetClosestItem();
        if (target == null)
            yield break;
        
        _antiItemLaserRenderer.enabled = true;

        float timer = 0;
        while (timer < _shotDuration)
        {
            if (target == null)
                timer = _shotDuration;
            else
            {
                _antiItemLaserRenderer.SetPosition(0, transform.position + (Vector3)GetClosestGun(target.position));
                _antiItemLaserRenderer.SetPosition(1, target.position);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        _antiItemLaserRenderer.enabled = false;
        if (target != null)
        {
            target.TryGetComponent(out IPickup obj);
            obj.DestroyThis(true);
        }
    }
    #endregion
    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint1, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint2, 0.02f);
    }
}