using UnityEngine;


public class WeaponInstanceMovement : MonoBehaviour
{
    protected float _lifetime = 5f;


    void Awake() => Destroy(gameObject, _lifetime);

    public void SetParameters(float lifetime = 5f) => _lifetime = lifetime;
}