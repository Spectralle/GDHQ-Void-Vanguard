using UnityEngine;


public static class AttackLibrary
{
    // =========================================

    public struct Laser
    {
        public struct Free
        {
            public static AttackTemplate OneForward() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 1,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate TwoForward20() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 2,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 20,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate TwoForward65() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 2,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 65,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Three360() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate ThreeForward20() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 20,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate ThreeForward30() => new AttackTemplate(
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

            public static AttackTemplate ThreeForward100() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 100,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate FiveForward30() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 5,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 30,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate FiveForward100() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 5,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 100,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate EightForward100() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 8,
                ammoCost: 0,
                projectileSpeed: 6f,
                spawnDelay: 0f,
                spawnDegreesIn360: 100,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Ten90() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 90,
                spawnRadius: 0.2f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Ten360() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Ten360Delayed() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0.2f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Twenty360() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 20,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Twenty360Delayed() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 20,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0.07f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Thirty360Delayed() => new AttackTemplate(
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

            public static AttackTemplate Sixty360() => new AttackTemplate(
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
        }

        public struct Boss
        {
            public static AttackTemplate OneForward() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 1,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate TwoForward20() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 2,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 20,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate TwoForward65() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 2,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 65,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Three360() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate ThreeForward20() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 20,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate ThreeForward30() => new AttackTemplate(
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

            public static AttackTemplate ThreeForward100() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 100,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate FiveForward30() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 5,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 30,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate FiveForward100() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 5,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 100,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate EightForward100() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 8,
                ammoCost: 0,
                projectileSpeed: 6f,
                spawnDelay: 0f,
                spawnDegreesIn360: 100,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Ten90() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 90,
                spawnRadius: 0.2f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Ten360() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Ten360Delayed() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0.1f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Twenty360() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 20,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Twenty360Delayed() => new AttackTemplate(
                projectilePrefab: _defaultProjectilePrefab,
                numberOfProjectiles: 20,
                ammoCost: 0,
                projectileSpeed: 10f,
                spawnDelay: 0.07f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate Thirty360Delayed() => new AttackTemplate(
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

            public static AttackTemplate Sixty360() => new AttackTemplate(
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
        }

        public static AttackTemplate OneForward() => new AttackTemplate(
            projectilePrefab: _defaultProjectilePrefab,
            numberOfProjectiles: 1,
            ammoCost: 1,
            projectileSpeed: 9f,
            spawnDelay: 0f,
            spawnDegreesIn360: 360,
            spawnRadius: 1f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
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

        public static AttackTemplate ThreeForward30() => new AttackTemplate(
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

        public static AttackTemplate FiveForward20() => new AttackTemplate(
            projectilePrefab: _defaultProjectilePrefab,
            numberOfProjectiles: 5,
            ammoCost: 1,
            projectileSpeed: 9f,
            spawnDelay: 0f,
            spawnDegreesIn360: 20,
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

        public static AttackTemplate Ten90() => new AttackTemplate(
            projectilePrefab: _defaultProjectilePrefab,
            numberOfProjectiles: 10,
            ammoCost: 1,
            projectileSpeed: 9f,
            spawnDelay: 0f,
            spawnDegreesIn360: 90,
            spawnRadius: 0.2f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
        );

        public static AttackTemplate Twenty360() => new AttackTemplate(
            projectilePrefab: _defaultProjectilePrefab,
            numberOfProjectiles: 20,
            ammoCost: 1,
            projectileSpeed: 10f,
            spawnDelay: 0f,
            spawnDegreesIn360: 360,
            spawnRadius: 1f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
        );

        public static AttackTemplate Thirty360Delayed() => new AttackTemplate(
            projectilePrefab: _defaultProjectilePrefab,
            numberOfProjectiles: 30,
            ammoCost: 1,
            projectileSpeed: 10f,
            spawnDelay: 0.07f,
            spawnDegreesIn360: 360,
            spawnRadius: 1f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
        );

        public static AttackTemplate Sixty360() => new AttackTemplate(
            projectilePrefab: _defaultProjectilePrefab,
            numberOfProjectiles: 60,
            ammoCost: 1,
            projectileSpeed: 10f,
            spawnDelay: 0f,
            spawnDegreesIn360: 360,
            spawnRadius: 1f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
        );
    }

    // =========================================

    public struct Ricochet
    {
        public struct Free
        {
            public static AttackTemplate OneForward() => new AttackTemplate(
                projectilePrefab: _ricochetProjectilePrefab,
                numberOfProjectiles: 1,
                ammoCost: 0,
                projectileSpeed: 13f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );

            public static AttackTemplate ThreeForward() => new AttackTemplate(
                projectilePrefab: _ricochetProjectilePrefab,
                numberOfProjectiles: 3,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 30,
                spawnRadius: 1f,
                audioClip: _defaultProjectileAudio,
                failedAudioClip: _defaultProjectileFailedAudio
            );
        }

        public static AttackTemplate OneForward() => new AttackTemplate(
            projectilePrefab: _ricochetProjectilePrefab,
            numberOfProjectiles: 1,
            ammoCost: 1,
            projectileSpeed: 13f,
            spawnDelay: 0f,
            spawnDegreesIn360: 360,
            spawnRadius: 1f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
        );

        public static AttackTemplate ThreeForward() => new AttackTemplate(
            projectilePrefab: _ricochetProjectilePrefab,
            numberOfProjectiles: 3,
            ammoCost: 1,
            projectileSpeed: 9f,
            spawnDelay: 0f,
            spawnDegreesIn360: 30,
            spawnRadius: 1f,
            audioClip: _defaultProjectileAudio,
            failedAudioClip: _defaultProjectileFailedAudio
        );
    }

    // =========================================

    public struct Missile
    {
        public struct Free
        {
            public static AttackTemplate OneForward() => new AttackTemplate(
                projectilePrefab: _missileProjectilePrefab,
                numberOfProjectiles: 1,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 360,
                spawnRadius: 1f,
                audioClip: _missileProjectileAudio,
                failedAudioClip: _missileProjectileFailedAudio
            );

            public static AttackTemplate ThreeForward() => new AttackTemplate(
                projectilePrefab: _missileProjectilePrefab,
                numberOfProjectiles: 10,
                ammoCost: 0,
                projectileSpeed: 9f,
                spawnDelay: 0f,
                spawnDegreesIn360: 50,
                spawnRadius: 1f,
                audioClip: _missileProjectileAudio,
                failedAudioClip: _missileProjectileFailedAudio
            );
        }

        public static AttackTemplate OneForward() => new AttackTemplate(
            projectilePrefab: _missileProjectilePrefab,
            numberOfProjectiles: 1,
            ammoCost: 0,
            projectileSpeed: 9f,
            spawnDelay: 0f,
            spawnDegreesIn360: 360,
            spawnRadius: 1f,
            audioClip: _missileProjectileAudio,
            failedAudioClip: _missileProjectileFailedAudio
        );

        public static AttackTemplate ThreeForward() => new AttackTemplate(
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
    }

    // =========================================


    private static GameObject _defaultProjectilePrefab;
    private static GameObject _ricochetProjectilePrefab;
    private static GameObject _missileProjectilePrefab;
    private static AudioClip _defaultProjectileAudio;
    private static AudioClip _defaultProjectileFailedAudio;
    private static AudioClip _missileProjectileAudio;
    private static AudioClip _missileProjectileFailedAudio;


    public static void Initialize(
        GameObject defaultProjectilePrefab,
        GameObject ricochetProjectilePrefab,
        GameObject missileProjectilePrefab,
        AudioClip defaultProjectileAudio, AudioClip defaultProjectileFailedAudio,
        AudioClip missileProjectileAudio, AudioClip missileProjectileFailedAudio)
    {
        _defaultProjectilePrefab        = defaultProjectilePrefab;
        _ricochetProjectilePrefab       = ricochetProjectilePrefab;
        _missileProjectilePrefab        = missileProjectilePrefab;
        _defaultProjectileAudio         = defaultProjectileAudio;
        _defaultProjectileFailedAudio   = defaultProjectileFailedAudio;
        _missileProjectileAudio         = missileProjectileAudio;
        _missileProjectileFailedAudio   = missileProjectileFailedAudio;
    }
}