using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private bool _isShieldActive;


    private void Start()
    {
        if (!transform.GetChild(0))
        {
            Debug.LogError("No child shield defined. Shields disabled");
            enabled = false;
        }
        else
            transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ActivatePowerup(PowerupType type, int duration)
    {
        if (_isShieldActive)
            StopAllCoroutines();
        StartCoroutine(ManagePowerup(type, duration));
    }

    private IEnumerator ManagePowerup(PowerupType type, int duration)
    {
        //switch (type)
        //{
        //    case PowerupType.Shield:
                GameObject shield = transform.GetChild(0).gameObject;
                shield.SetActive(true);
                yield return new WaitForSeconds(duration);
                shield.SetActive(false);
        //        break;
        // Allowing for different types of shields in future
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Projectile"))
        {
            GameObject shield = transform.GetChild(0).gameObject;
            shield.SetActive(false);
            _isShieldActive = false;
        }
    }
}