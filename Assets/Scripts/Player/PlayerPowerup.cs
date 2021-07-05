using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    private AudioSource audioSource;


    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void ActivatePowerup(PowerupType type, int duration, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
        switch (type)
        {
            case PowerupType.TripleShot:
                TryGetComponent(out PlayerGun playerGun1);
                if (playerGun1)
                    playerGun1.ActivatePowerup(type, duration);
                break;

            case PowerupType.SpeedBoost:
                TryGetComponent(out PlayerMovement playerMovement);
                if (playerMovement)
                    playerMovement.ActivatePowerup(type, duration);
                TryGetComponent(out PlayerGun playerGun2);
                if (playerGun2)
                    playerGun2.ActivatePowerup(type, duration);
                break;

            case PowerupType.Shield:
                TryGetComponent(out PlayerShield playerShield);
                if (playerShield)
                    playerShield.ActivatePowerup(type, duration);
                break;
        }
    }
}