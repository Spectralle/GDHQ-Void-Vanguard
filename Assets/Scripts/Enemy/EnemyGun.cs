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
    [Space]
    [Header("Weapons")]
    [SerializeField] private GameObject _pfLaser;
    [SerializeField] private Vector2 _laserSpeed = new Vector2(0, -4f);
    [SerializeField] private AudioClip _laserAudioClip;
    [Space]
    [Header("Anti-Item Laser")]
    [SerializeField, Range(0, 10)] private int _activationChance = 1;
    [SerializeField, Range(0, 1.5f)] private float _shotDelay = 1f;
    [SerializeField, Range(0, 1f)] private float _shotDuration = 0.5f;

    private Transform _projectileContainer;
    private bool _readyToShoot = true;
    private float _cooldownMultiplier = 1;
    private AudioSource _asrc;
    private LineRenderer _antiItemLaserRenderer;


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

    #region Anti-Item Laser
    private List<Transform> GetItemsBelowMe()
    {
        if (SpawnManager.ItemsExist)
        {
            List<Transform> _itemsBelowMe = new List<Transform>();

            foreach (Transform t in SpawnManager.ItemList)
            {
                if (t.position.y < (transform.position.y - 2))
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

    private IEnumerator ShootAILaser()
    {
        yield return new WaitForSeconds(_shotDelay);

        Transform target = GetClosestItem();
        if (target == null)
            yield return null;

        Vector2 activeShotPoint =
            Vector2.Distance(_shotPoint1, target.position) < Vector2.Distance(_shotPoint2, target.position) ?
            _shotPoint1 : _shotPoint2;
        _antiItemLaserRenderer.enabled = true;

        float timer = 0;
        while (timer < _shotDuration)
        {
            if (target == null)
                timer = _shotDuration;
            else
            {
                _antiItemLaserRenderer.SetPosition(0, transform.position + (Vector3)activeShotPoint);
                _antiItemLaserRenderer.SetPosition(1, target.position);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        _antiItemLaserRenderer.enabled = false;
        if (target != null)
            Destroy(target.gameObject);
    }
    #endregion

    public void ShootAntiItemLaser() => StartCoroutine(ShootAILaser());


    private void OnDrawGizmosSelected()
    {
        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint1, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint2, 0.02f);
    }
}