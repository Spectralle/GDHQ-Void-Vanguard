using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(LineRenderer))]
public class EnemyGun : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private bool _canShoot = true;
    [SerializeField] private bool _canShootBackwards = true;
    [SerializeField] private Vector2 _shotCooldown = new Vector2(2.5f, 7);
    [Space]
    [SerializeField] private AttackStyle _attackStyle;
    public enum AttackStyle
    {
        OneForward,
        ThreeForward20,
        FiveForward30
    }
    [Space]
    [Header("Anti-Item Laser")]
    public AILType AILaserType;
    public enum AILType
    {
        None,
        Basic,
        Advanced
    }
    [SerializeField, Range(0, 10)] private int _AAIActivationChance = 1;
    [SerializeField, Range(0, 1.5f)] private float _AAIShotDelay = 0.5f;
    [SerializeField, Range(0, 1f)] private float _AAIShotDuration = 0.5f;
    [SerializeField] private AudioClip _AAIAudioClip;

    private EnemyMovement _enemyMovement;
    private static Transform _player;
    private static Transform _projectileContainer;
    private bool _readyToShoot = true;
    private float _cooldownMultiplier = 1;
    private AudioSource _audioSource;
    private LineRenderer _antiItemLaserRenderer;
    private float _antiItemTimer;
    private float _backwardsShotTimer;


    private void Awake()
    {
        if (!_projectileContainer)
            _projectileContainer = GameObject.Find("Projectile Container").transform;
        if (!_player)
            _player = GameObject.Find("Player").transform;
        if (!_projectileContainer)
            Debug.LogWarning("No container object set for projectiles");
        _enemyMovement = GetComponent<EnemyMovement>();
        _audioSource = GetComponent<AudioSource>();
        _antiItemLaserRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (_player && _canShoot && _canShootBackwards)
            CheckShootBackwards();

        if (_canShoot && _readyToShoot && !_enemyMovement.IsDestroyed)
        {
            switch (_attackStyle)
            {
                case AttackStyle.OneForward:
                    StartCoroutine(MakeAnAttack(AttackLibrary.OneForward()));
                    break;
                case AttackStyle.ThreeForward20:
                    StartCoroutine(MakeAnAttack(AttackLibrary.ThreeForward20()));
                    break;
                case AttackStyle.FiveForward30:
                    StartCoroutine(MakeAnAttack(AttackLibrary.FiveForward30()));
                    break;
            }
        }

        if (AILaserType == AILType.Basic)
            ShootBasicAntiItemLaser();
    }

    #region Default Laser
    private IEnumerator MakeAnAttack(AttackTemplate attackData, bool fireBackwards = false)
    {
        _readyToShoot = false;

        float angleStep = attackData.Degrees / attackData.Number;
        float angle = attackData.Degrees != 360 ? (fireBackwards ? 0f : 180f) - (attackData.Degrees - angleStep) / 2 : (fireBackwards ? 0f : 180f);
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
            shot.tag = "Enemy Projectile";
            shot.GetComponent<Rigidbody2D>().velocity = attackData.Delay == 0 ? shotMovementVector : shotCurrentMovementVector;

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

    private void CheckShootBackwards()
    {
        if (_backwardsShotTimer <= 0)
        {
            bool isPlayerAboveThis =
                transform.position.y < _player.position.y &&
                _player.transform.position.x > transform.position.x - 1 &&
                _player.transform.position.x < transform.position.x + 1;

            if (isPlayerAboveThis)
                StartCoroutine(MakeAnAttack(AttackLibrary.OneForward(), true));

            _backwardsShotTimer = 1f;
        }
        else
            _backwardsShotTimer -= Time.deltaTime;
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(Random.Range(_shotCooldown.x, _shotCooldown.y) * _cooldownMultiplier);
        _readyToShoot = true;
    }
    #endregion

    #region Basic Anti-Item Laser
    public void ShootBasicAntiItemLaser()
    {
        if (Random.Range(0, 10) > _AAIActivationChance)
            return;

        if (_antiItemTimer < 1f)
            _antiItemTimer += Time.deltaTime;
        else
        {
            _antiItemTimer = 0;

            RaycastHit2D hit1 = Physics2D.Raycast((Vector2)transform.position, Vector2.down);
            if (hit1)
            {
                if (hit1.transform.CompareTag("Powerup"))
                {
                    StartCoroutine(MakeAnAttack(AttackLibrary.OneForward()));

                    if (_audioSource && _AAIAudioClip)
                        _audioSource.PlayOneShot(_AAIAudioClip);
                }
            }

            RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position, Vector2.down);
            if (hit2)
            {
                if (hit2.transform.CompareTag("Powerup"))
                {
                    StartCoroutine(MakeAnAttack(AttackLibrary.OneForward()));

                    if (_audioSource && _AAIAudioClip)
                        _audioSource.PlayOneShot(_AAIAudioClip);
                }
            }
        }
    }
    #endregion

    #region Advanced Anti-Item Laser
    public void ShootAdvAntiItemLaser()
    {
        if (Random.Range(0, 10) <= _AAIActivationChance)
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

    private IEnumerator ShootAdvancedAntiItemLaser()
    {
        yield return new WaitForSeconds(_AAIShotDelay);

        Transform target = GetClosestItem();
        if (target == null)
            yield break;
        
        _antiItemLaserRenderer.enabled = true;

        float timer = 0;
        while (timer < _AAIShotDuration)
        {
            if (target == null)
                timer = _AAIShotDuration;
            else
            {
                _antiItemLaserRenderer.SetPosition(0, transform.position - (transform.up * 0.5f));
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
}