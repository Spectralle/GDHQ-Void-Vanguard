using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private GameObject _childShield;

    private bool _isShieldActive;


    private void Start()
    {
        if (_childShield == null)
        {
            Debug.LogError("No child shield defined. Shields disabled");
            enabled = false;
        }
        else
            _childShield.SetActive(false);
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        if (_isShieldActive)
            StopAllCoroutines();
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        switch (type)
        {
            case PowerupType.Shield:
                _childShield.SetActive(true);
                yield return new WaitForSeconds(duration);
                _childShield.SetActive(false);
                break;
        }
    }
}