using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupType _type;
    [SerializeField] private int _fallSpeed = 3;
    [SerializeField] private int _duration = 5;


    private void Update()
    {
        transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);

        if (transform.position.y < LevelBoundary.D(-2))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out PlayerGun playerGun);
            if (playerGun)
                playerGun.ActivatePowerup(_type, _duration);
            Destroy(gameObject);
        }
    }
}
