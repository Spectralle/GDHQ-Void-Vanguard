using System.Collections;
using UnityEngine;

public class UnitShield : MonoBehaviour
{
    [SerializeField] private Color[] _shieldLevelColors =
    {
        Color.red,
        Color.grey,
        Color.white,
    };
    public bool IsPlayerShield => gameObject.CompareTag("Player");
    public bool IsActive => _isShieldActive;

    private int _hitsRemaining = 1;
    private bool _isShieldActive = false;
    private UnitShieldObject _shieldObj;
    private SpriteRenderer _objSpriteRenderer;


    private void Awake()
    {
        _hitsRemaining = _shieldLevelColors.Length;
        _shieldObj = transform.Find("Shield").GetComponent<UnitShieldObject>();
        if (!_shieldObj)
        {
            Debug.LogError($"No shield object found on {name}. Shields disabled", gameObject);
            enabled = false;
        }
        else
        {
            _shieldObj.Initialize(this);
            if (_shieldLevelColors.Length > 0)
            {
                _objSpriteRenderer = _shieldObj.GetComponent<SpriteRenderer>();
                if (!IsPlayerShield)
                    ActivateShield();
                else
                    DeactivateShield();
            }
            else
            {
                _isShieldActive = false;
                _shieldObj.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateShieldAsPowerup(int duration)
    {
        if (_isShieldActive)
            StopAllCoroutines();
        StartCoroutine(ManagePowerup(duration));
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _hitsRemaining = _shieldLevelColors.Length;
        _shieldObj.gameObject.SetActive(true);
        _objSpriteRenderer.color = _shieldLevelColors[_hitsRemaining - 1];
    }

    public void ShieldHit(Collider2D other)
    {
        if (IsPlayerShield)
        {
            if (other.CompareTag("Enemy Projectile") || other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);

                _hitsRemaining--;

                if (_hitsRemaining > 0)
                    _objSpriteRenderer.color = _shieldLevelColors[_hitsRemaining - 1];
                else
                    DeactivateShield();
            }
        }
        else
        {
            if (other.CompareTag("Player Projectile"))
            {
                Destroy(other.gameObject);

                _hitsRemaining--;

                if (_hitsRemaining > 0)
                    _objSpriteRenderer.color = _shieldLevelColors[_hitsRemaining - 1];
                else
                    DeactivateShield();
            }
            else if (other.CompareTag("Player"))
                DeactivateShield();
        }
    }

    public void DeactivateShield()
    {
        StopAllCoroutines();
        _isShieldActive = false;
        _shieldObj.gameObject.SetActive(false);
        _hitsRemaining = _shieldLevelColors.Length;
        _objSpriteRenderer.color = _shieldLevelColors[_hitsRemaining - 1];
    }

    private IEnumerator ManagePowerup(int duration)
    {
        ActivateShield();
        yield return new WaitForSeconds(duration);
        DeactivateShield();
    }
}