using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField, Range(0, 12)] private float _moveSpeed = 5;
    [SerializeField, Range(0, 2)] private float _sineXMoveScale = 0.5f;
    [SerializeField, Range(0, 2)] private float _sineYMoveScale = 0.5f;
    [Header("Evasion:")]
    [SerializeField, Range(0, 10)] private int _evadeChance = 3;
    [SerializeField, Range(0, 10)] private float _evadeDistance = 2.5f;
    [Header("Ramming:")]
    [SerializeField] private bool _canRamPlayer;
    [SerializeField, Range(1, 4)] private float _ramRadius = 3;
    [SerializeField, Range(1, 10)] private float _ramSpeed = 8;
    [SerializeField, Range(0.2f, 5)] private float _ramRotateSpeed;
    [SerializeField] private GameObject _trails;
    [Header("Other:")]
    [SerializeField] private AudioClip _explosionAudioClip;
    public bool IsDestroyed => _isDestroyed;

    private Transform _player;
    private Animator _anim;
    private EnemyGun _gun;
    private bool _isDestroyed;
    private Vector2 _originalPosition = Vector2.zero;
    private Quaternion _originalRotation = Quaternion.identity;
    private Vector3 _baseMovementDirection = Vector3.down;
    private int _damageAmount = 1;
    private bool _isShootingAsteroid;
    private int _sineDirection = 1;
    private int _randomSineStarter;
    private const float _ramMinDistance = 1f;


    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        TryGetComponent(out _gun);
        SetOrigins();
        if (_trails)
            _trails.SetActive(true);

        if (Random.Range(0, 2) == 0)
            _sineDirection = -1;

        _randomSineStarter = Random.Range(1, 500);
    }

    private void SetOrigins()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public void SetMovementDirection(Vector3 dir) => _baseMovementDirection = dir;
    public void SetDamageAmount(int amount) => _damageAmount = amount;
    public void SetAsAsteroid() => _isShootingAsteroid = true;

    private void Update()
    {
        if (!_isShootingAsteroid)
        {
            DoShipMovement();

            if (_canRamPlayer && !_isDestroyed)
                CheckForRamChance();
        }
        else
            DoAsteroidMovement();

        CheckForReposition();
    }

    private void DoShipMovement()
    {
        _originalPosition += (Vector2)_baseMovementDirection * _moveSpeed * Time.deltaTime;

        float X = 0;
        if (_sineXMoveScale > 0)
            X = Mathf.Sin((Time.timeSinceLevelLoad + _randomSineStarter) * _sineDirection * 5) * _sineXMoveScale;
        float Y = 0;
        if (_sineYMoveScale > 0)
            Y = Mathf.Cos((Time.timeSinceLevelLoad + _randomSineStarter) * _sineDirection * 5) * _sineYMoveScale;

        transform.position = _originalPosition + new Vector2(X, Y);
    }

    private void DoAsteroidMovement()
    {
        transform.Translate(_baseMovementDirection * _moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * 4 * Time.deltaTime);
    }

    private void CheckForRamChance()
    {
        float distance = Vector2.Distance(transform.position, _player.position);
        if (distance < _ramRadius && distance > 0.6f)
            RamInPlayersDirection();
        else
        {
            if (transform.rotation != _originalRotation)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * _ramRotateSpeed);
        }
    }

    private void RamInPlayersDirection()
    {
        Vector3 directionToPlayer = -(_player.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90;
        Quaternion rotateToPlayer = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToPlayer, Time.deltaTime * _ramRotateSpeed);

        _originalPosition += -(Vector2)transform.up * _ramSpeed * Time.deltaTime;
    }

    private void CheckForReposition()
    {
        if (!_isDestroyed && transform.position.y < LevelBoundary.D(-2))
        {
            if (SpawnManager.i.CanSpawn && !_isShootingAsteroid)
            {
                if (_trails)
                    _trails.SetActive(false);
                StopCoroutine(Evade());
                transform.position = SpawnManager.GetEnemySpawnPosition();
                transform.rotation = Quaternion.identity;
                SetOrigins();
                if (_gun.AILaserType == EnemyGun.AILType.Advanced)
                    _gun.ShootAdvAntiItemLaser();
                if (_trails)
                    _trails.SetActive(true);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!_isShootingAsteroid && SpawnManager.EnemyList.Contains(transform))
            SpawnManager.EnemyList.Remove(transform);
    }

    #region Collide
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile") && !_isShootingAsteroid)
        {
            int evadeChance = Random.Range(1, 10);
            if (evadeChance <= _evadeChance)
                StartCoroutine(Evade());
            else
            {
                Destroy(other.gameObject);
                StartCoroutine(Explode());
            }
        }

        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerHealth playerHealth);
            if (playerHealth)
                playerHealth.Damage(_damageAmount);
            StartCoroutine(Explode());
        }
    }
    #endregion

    #region Evade
    private IEnumerator Evade()
    {
        float _sineX = _sineXMoveScale;
        _sineXMoveScale = 0;
        float _sineY = _sineYMoveScale;
        _sineYMoveScale = 0;

        float X = _evadeDistance;
        int leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0)
            X = -X;

        _originalPosition.x += X;

        if (_originalPosition.x < LevelBoundary.L(2) || _originalPosition.x > LevelBoundary.R(2))
        {
            X = -X;
            _originalPosition.x = transform.position.x + X;
        }

        while (leftOrRight == 0 ? transform.position.x > X : transform.position.x < X)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, Time.deltaTime * 4.5f);
            yield return new WaitForEndOfFrame();
        }

        _sineXMoveScale = _sineX;
        _sineYMoveScale = _sineY;
    }
    #endregion

    #region Explode
    public void MakeExplode() => StartCoroutine(Explode());

    private IEnumerator Explode()
    {
        if (_isDestroyed || _isShootingAsteroid)
            yield break;

        _isDestroyed = true;

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child != transform)
                child.gameObject.SetActive(false);
        }

        SpawnManager.ChangeEnemiesAlive(transform);

        UIManager.i.ChangeKills(1);
        UIManager.i.ChangeScore(10);

        TryGetComponent(out _anim);
        if (_anim)
            _anim.SetTrigger("OnEnemyDeath");

        TryGetComponent(out AudioSource _asrc);
        if (_asrc)
            _asrc.PlayOneShot(_explosionAudioClip, 0.9f);

        TryGetComponent(out Collider2D _c2d);
        if (_c2d)
            _c2d.enabled = false;

        Destroy(gameObject, 2.5f);

        while (_moveSpeed > 0.00f)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, 0, Time.deltaTime * 1.5f);
            _sineXMoveScale = Mathf.Lerp(_sineXMoveScale, 0, Time.deltaTime * 1.5f);
            _sineYMoveScale = Mathf.Lerp(_sineYMoveScale, 0, Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
