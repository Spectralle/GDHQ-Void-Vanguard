using UnityEngine;


public class WeaponInstanceMovement : MonoBehaviour
{
    protected float _lifetime = 2f;
    protected Vector2 _movementDirection = new Vector2(0, 8f);


    void Awake() => Destroy(gameObject, _lifetime);

    private void Update() => transform.Translate(Vector2.one * _movementDirection * Time.deltaTime);

    public void SetMovementDirection(Vector2 direction) => _movementDirection = direction;
    public void SetMovementDirection(float directionX, float directionY) => _movementDirection = new Vector2(directionX, directionY);

    public void SetLifetime(float lifetime) => _lifetime = lifetime;
}