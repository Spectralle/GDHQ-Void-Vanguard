using UnityEngine;

public class UnitShieldObject : MonoBehaviour
{
    private UnitShield _shieldController;


    public void Initialize(UnitShield controller) => _shieldController = controller;

    private void OnTriggerEnter2D(Collider2D other) => _shieldController.ShieldHit(other);
}