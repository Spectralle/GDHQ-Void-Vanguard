using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    public void ActivatePowerup(PowerupType type, int duration)
    {
        switch (type)
        {
            case PowerupType.TripleShot:
                TryGetComponent(out PlayerGun playerGun);
                if (playerGun)
                    playerGun.ActivatePowerup(type, duration);
                break;

            case PowerupType.SpeedBoost:
                TryGetComponent(out PlayerMovement playerMovement);
                if (playerMovement)
                    playerMovement.ActivatePowerup(type, duration);
                break;

            case PowerupType.Shield:
                TryGetComponent(out PlayerShield playerShield);
                if (playerShield)
                    playerShield.ActivatePowerup(type, duration);
                break;
        }
    }
}