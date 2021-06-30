using System.Collections;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _shotCooldown = 0.4f;
    [Space]
    [SerializeField] private Vector2 _shotPoint1 = new Vector2(0, 0.75f);
    [SerializeField] private Vector2 _shotPoint2 = new Vector2(-0.785f, -0.61f);
    [SerializeField] private Vector2 _shotPoint3 = new Vector2(0.785f, -0.61f);
    [Space]
    [Header("Weapons")]
    [SerializeField] private GameObject _pfLaser;
    [Space]
    [SerializeField] private Transform _projectileContainer;

    private bool _canFire = true;
    private bool _isTripleShotActive;


    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _canFire)
            ShootLaser();
    }

    private Vector3 GetShotSpawnPoint(int pointIndex)
    {
        switch (pointIndex)
        {
            default:
            case 1: return (Vector2)transform.position + _shotPoint1 + new Vector2(0, _pfLaser.transform.lossyScale.y);
            case 2: return (Vector2)transform.position + _shotPoint2 + new Vector2(0, _pfLaser.transform.lossyScale.y);
            case 3: return (Vector2)transform.position + _shotPoint3 + new Vector2(0, _pfLaser.transform.lossyScale.y);
        }
    }

    public void ShootLaser()
    {
        if (!_isTripleShotActive)
            Instantiate(_pfLaser, GetShotSpawnPoint(1), Quaternion.identity, _projectileContainer);
        else
        {
            Instantiate(_pfLaser, GetShotSpawnPoint(1), Quaternion.identity, _projectileContainer);
            Instantiate(_pfLaser, GetShotSpawnPoint(2), Quaternion.identity, _projectileContainer);
            Instantiate(_pfLaser, GetShotSpawnPoint(3), Quaternion.identity, _projectileContainer);
        }
        StartCoroutine(ShotCooldown());
    }

    private IEnumerator ShotCooldown()
    {
        _canFire = false;
        yield return new WaitForSeconds(_shotCooldown);
        _canFire = true;
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        if (_isTripleShotActive)
            StopAllCoroutines();
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        switch (type)
        {
            case PowerupType.TripleShot:
                _isTripleShotActive = true;
                yield return new WaitForSeconds(duration);
                _isTripleShotActive = false;
                break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint1, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint2, 0.02f);
        Gizmos.DrawSphere(transform.position + (Vector3)_shotPoint3, 0.02f);
    }
}
