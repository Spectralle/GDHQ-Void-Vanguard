using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RichochetLaserMovement : LaserMovement
{
    public void SetSpeed(float speed) => _speed = speed;
    private Rigidbody2D _rb;
    private float _speed;
    private float _turnSpeed = 65f;
    private Quaternion _intendedRotation;

    private void Awake()
    {
        TryGetComponent(out _rb);
        _intendedRotation = transform.rotation;
    }

    private void Update() => transform.rotation = Quaternion.Lerp(transform.rotation, _intendedRotation, Time.deltaTime * _turnSpeed);

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 24f);
            Collider2D nearest = FindNearestEnemy(objects, other);

            if (nearest == other || nearest == null)
                Destroy(gameObject);
            else if (nearest != null)
            {
                Vector2 distance = nearest.transform.position - transform.position;
                float transformUpAngle = Mathf.Atan2(distance.x, distance.y);
                float rotationZ = 360 - ((Mathf.PI * 2) + transformUpAngle) * Mathf.Rad2Deg;
                _intendedRotation = Quaternion.Euler(new Vector3(0, 0, rotationZ));
                _rb.velocity = (nearest.transform.position - transform.position).normalized * (_speed * 3);
            }
        }
    }

    private Collider2D FindNearestEnemy(Collider2D[] objects, Collider2D initialEnemy)
    {
        Collider2D nearest = null;

        foreach (Collider2D obj in objects)
        {
            if (obj.gameObject.CompareTag("Enemy") && obj != initialEnemy)
            {
                if (nearest == null)
                    nearest = obj;
                else if (Vector2.Distance(obj.transform.position, transform.position) <
                    Vector2.Distance(nearest.transform.position, transform.position))
                    nearest = obj;
            }
        }
        return nearest;
    }
}
