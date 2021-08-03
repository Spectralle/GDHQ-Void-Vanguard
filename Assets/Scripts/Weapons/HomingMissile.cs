using System.Collections;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float _moveSpeed = 3f;
    [SerializeField, Range(0, 10)] private float _turnSpeed = 3f;
    [SerializeField, Range(1, 10)] private float _lifetime = 5f;
    [SerializeField] private GameObject _explosionPrefab;

    private Transform _target;
    private float _retargetTimer;
    private Rigidbody2D _rb;
    private Vector3 directionToTarget;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _retargetTimer = 0;

        _target = SpawnManager.GetClosestEnemy(transform);

        StartCoroutine(Explode(_lifetime));
    }

    private void Update()
    {
        if (_retargetTimer > 0.4f)
        {
            _target = SpawnManager.GetClosestEnemy(transform);
            _retargetTimer = 0;
        }
        else
            _retargetTimer += Time.deltaTime;

        if (_target != null)
        {
            directionToTarget = (_target.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * _turnSpeed);

            _rb.velocity = Vector2.Lerp(_rb.velocity, transform.up * _moveSpeed, Time.deltaTime);

            Debug.DrawRay(transform.position, (_target.position - transform.position), Color.red);
        }
        else
        {
            directionToTarget = transform.up;
            _rb.velocity = Vector2.Lerp(_rb.velocity, directionToTarget * (_moveSpeed * 0.75f), Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyMovement>().MakeExplode();
            Destroy(gameObject);
        }
    }

    private IEnumerator Explode(float delay)
    {
        yield return new WaitForSeconds(delay);

        bool isWithinScreenBounds =
            (transform.position.x > LevelBoundary.L()) && (transform.position.x < LevelBoundary.R()) &&
            (transform.position.y < LevelBoundary.U()) && (transform.position.y > LevelBoundary.D());

        if (isWithinScreenBounds)
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameObject.Find("Game Handler/Scene").transform);

        Destroy(gameObject);
    }
}
