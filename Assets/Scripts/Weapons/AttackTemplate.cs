using UnityEngine;

[System.Serializable]
public struct AttackTemplate
{
    public GameObject Prefab;
    public int Number;
    public int AmmoCost;
    public float Speed;
    public float Delay;
    public int Degrees;
    public float Radius;
    public AudioClip AudioClip;
    public AudioClip FailedAudioClip;


    public AttackTemplate(
        GameObject projectilePrefab,
        int numberOfProjectiles = 1,
        int ammoCost = 1,
        float projectileSpeed = 10f,
        float spawnDelay = 0f,
        int spawnDegreesIn360 = 360,
        float spawnRadius = 0.2f,
        AudioClip audioClip = null,
        AudioClip failedAudioClip = null)
    {
        Prefab = projectilePrefab;
        Number = numberOfProjectiles;
        AmmoCost = ammoCost;
        Speed = projectileSpeed;
        Delay = spawnDelay;
        Degrees = spawnDegreesIn360;
        Radius = spawnRadius;
        AudioClip = audioClip;
        FailedAudioClip = failedAudioClip;
    }
}