using UnityEngine;


public static class AttackLibrary
{
    #region Laser Attacks
    public static AttackTemplate OneForward() => new AttackTemplate(
        projectilePrefab:       _defaultProjectilePrefab,
        numberOfProjectiles:    1,
        ammoCost:               1,
        projectileSpeed:        9f,
        spawnDelay:             0f,
        spawnDegreesIn360:      360,
        spawnRadius:            1f,
        audioClip:              _defaultProjectileAudio,
        failedAudioClip:        _defaultProjectileFailedAudio
    );

    public static AttackTemplate Three360() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 3,
        ammoCost: 1,
        projectileSpeed: 9f,
        spawnDelay: 0f,
        spawnDegreesIn360: 360,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );

    public static AttackTemplate ThreeForward() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 3,
        ammoCost: 1,
        projectileSpeed: 9f,
        spawnDelay: 0f,
        spawnDegreesIn360: 30,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );

    public static AttackTemplate ThreeForward20() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 3,
        ammoCost: 1,
        projectileSpeed: 9f,
        spawnDelay: 0f,
        spawnDegreesIn360: 20,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );

    public static AttackTemplate ThreeForward_Free() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 3,
        ammoCost: 0,
        projectileSpeed: 9f,
        spawnDelay: 0f,
        spawnDegreesIn360: 30,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );

    public static AttackTemplate FiveForward30() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 5,
        ammoCost: 1,
        projectileSpeed: 9f,
        spawnDelay: 0f,
        spawnDegreesIn360: 30,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );

    public static AttackTemplate Ten90_Free() => new AttackTemplate(
        projectilePrefab:       _defaultProjectilePrefab,
        numberOfProjectiles:    10,
        ammoCost:               0,
        projectileSpeed:        9f,
        spawnDelay:             0f,
        spawnDegreesIn360:      90,
        spawnRadius:            0.2f,
        audioClip:              _defaultProjectileAudio,
        failedAudioClip:        _defaultProjectileFailedAudio
    );

    public static AttackTemplate Twenty360() => new AttackTemplate(
        projectilePrefab:       _defaultProjectilePrefab,
        numberOfProjectiles:    20,
        ammoCost:               1,
        projectileSpeed:        10f,
        spawnDelay:             0f,
        spawnDegreesIn360:      360,
        spawnRadius:            1f,
        audioClip:              _defaultProjectileAudio,
        failedAudioClip:        _defaultProjectileFailedAudio
    );

    public static AttackTemplate Thirty360Delayed_Free() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 30,
        ammoCost: 0,
        projectileSpeed: 10f,
        spawnDelay: 0.07f,
        spawnDegreesIn360: 360,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );

    public static AttackTemplate Sixty360_Free() => new AttackTemplate(
        projectilePrefab: _defaultProjectilePrefab,
        numberOfProjectiles: 60,
        ammoCost: 0,
        projectileSpeed: 10f,
        spawnDelay: 0f,
        spawnDegreesIn360: 360,
        spawnRadius: 1f,
        audioClip: _defaultProjectileAudio,
        failedAudioClip: _defaultProjectileFailedAudio
    );
    #endregion

    #region Missile Attacks
    public static AttackTemplate OneMissileForward() => new AttackTemplate(
        projectilePrefab:       _missileProjectilePrefab,
        numberOfProjectiles:    1,
        ammoCost:               1,
        projectileSpeed:        9f,
        spawnDelay:             0f,
        spawnDegreesIn360:      360,
        spawnRadius:            1f,
        audioClip:              _missileProjectileAudio,
        failedAudioClip:        _missileProjectileFailedAudio
    );

    public static AttackTemplate ThreeMissileForward() => new AttackTemplate(
        projectilePrefab: _missileProjectilePrefab,
        numberOfProjectiles: 10,
        ammoCost: 1,
        projectileSpeed: 9f,
        spawnDelay: 0f,
        spawnDegreesIn360: 50,
        spawnRadius: 1f,
        audioClip: _missileProjectileAudio,
        failedAudioClip: _missileProjectileFailedAudio
    );
    #endregion


    private static GameObject _defaultProjectilePrefab;
    private static GameObject _missileProjectilePrefab;
    private static AudioClip _defaultProjectileAudio;
    private static AudioClip _defaultProjectileFailedAudio;
    private static AudioClip _missileProjectileAudio;
    private static AudioClip _missileProjectileFailedAudio;


    public static void Initialize(
        GameObject defaultProjectilePrefab, GameObject missileProjectilePrefab,
        AudioClip defaultProjectileAudio, AudioClip defaultProjectileFailedAudio,
        AudioClip missileProjectileAudio, AudioClip missileProjectileFailedAudio)
    {
        _defaultProjectilePrefab = defaultProjectilePrefab;
        _missileProjectilePrefab = missileProjectilePrefab;
        _defaultProjectileAudio = defaultProjectileAudio;
        _defaultProjectileFailedAudio = defaultProjectileFailedAudio;
        _missileProjectileAudio = missileProjectileAudio;
        _missileProjectileFailedAudio = missileProjectileFailedAudio;
    }
}