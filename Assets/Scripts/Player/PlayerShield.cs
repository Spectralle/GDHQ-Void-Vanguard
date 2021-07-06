using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private Color _fullShield = Color.white;
    [SerializeField] private Color _twoThirdsShield = Color.grey;
    [SerializeField] private Color _oneThirdShield = Color.red;

    private int _hitsRemaining = 3;
    private bool _isShieldActive = false;
    public bool IsActive => _isShieldActive;
    private GameObject _shieldObj;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _shieldObj = transform.Find("Player Shield").gameObject;
        if (!_shieldObj)
        {
            Debug.LogError("No child shield defined. Shields disabled");
            enabled = false;
        }
        else
        {
            _spriteRenderer = _shieldObj.GetComponent<SpriteRenderer>();
            _shieldObj.SetActive(false);
        }
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        if (_isShieldActive)
            StopAllCoroutines();
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        _isShieldActive = true;
        _hitsRemaining = 3;
        _shieldObj.SetActive(true);
        _spriteRenderer.color = _fullShield;
        yield return new WaitForSeconds(duration);
        _shieldObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Projectile") || other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);

            _hitsRemaining--;

            switch (_hitsRemaining)
            {
                case 3:
                    _spriteRenderer.color = _fullShield;
                    break;
                case 2:
                    _spriteRenderer.color = _twoThirdsShield;
                    break;
                case 1:
                    _spriteRenderer.color = _oneThirdShield;
                    break;
                case 0:
                    _hitsRemaining = 3;
                    _isShieldActive = false;
                    StopAllCoroutines();
                    _shieldObj.SetActive(false);
                    break;
            }
        }
    }
}