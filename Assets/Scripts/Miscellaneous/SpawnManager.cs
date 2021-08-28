using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    #pragma warning disable CS0414
    public static SpawnManager i;

    [HideInInspector] public bool CanSpawn = true;
    [SerializeField] private GameObject _player;
    [SerializeField, Range(1, 10)] private int _startWave = 1;
    [Space]
    [SerializeField, Range(1, 20)] private int _bossWaveLives = 15; 
    [SerializeField, Range(15, 60)] private int _bossWaveAmmo = 50; 

    [Header("Wave Spawning:")]
    [SerializeField] private AnimationCurve _enemiesEachWave;
    [SerializeField] private AnimationCurve _waveEnemyDifficulty;
    [SerializeField, Range(4, 20)] private int _waveRecoveryBuffer = 5;
    [SerializeField] private TextMeshProUGUI _waveText;

    [Header("Enemies:")]
    [SerializeField] private bool _spawnEnemies = true;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Vector2 _enemySpawnDelay = new Vector2(2.5f, 5);
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private WeightedSpawnTable _enemies;
    [Header("Asteroids:")]
    [SerializeField] private bool _spawnAsteroids = true;
    [SerializeField] private Transform _asteroidContainer;
    [SerializeField] private Vector2 _asteroidSpawnDelay = new Vector2(16, 30);
    [SerializeField] private WeightedSpawnTable _asteroids;
    [Header("PowerUps:")]
    [SerializeField] private bool _spawnPowerups = true;
    [SerializeField] private Transform _powerupContainer;
    [SerializeField] private Vector2 _powerupSpawnDelay = new Vector2(6, 17);
    [Space]
    [SerializeField] private WeightedSpawnTable_TotalChance _powerups;
    [SerializeField] private WeightedSpawnTable_TotalChance _refills;
    [SerializeField] private WeightedSpawnTable_TotalChance _powerdowns;

    public static List<Transform> EnemyList = new List<Transform>();
    public static bool EnemiesExist => EnemyList.Count > 0;
    public static int EnemyCount => EnemyList.Count;

    public static List<Transform> ItemList = new List<Transform>();
    public static bool ItemsExist => ItemList.Count > 0;
    public static int ItemCount => ItemList.Count;

    private Vector3 directionToPlayer = Vector3.zero;
    private static int _waveNumber = 1;
    public static int GetCurrentWave() => _waveNumber;
    private static bool _allEnemiesSpawned;

    private bool _isBeforeStartAsteroid = true;
    public void AsteroidDestroyed() => _isBeforeStartAsteroid = false;
    #pragma warning restore CS0414


    private void Awake()
    {
        i = this;
        _waveNumber = _startWave;
    }

    public static void Init()
    {
        EnemyList.Clear();
        ItemList.Clear();
    }

    public static void Reset()
    {
        i.StopAllCoroutines();
        _waveNumber = i._startWave;
        Init();
    }

    private void Update()
    {
        if (_player == null)
            CanSpawn = false;

        if (!_isBeforeStartAsteroid && _player && _allEnemiesSpawned && !EnemiesExist && CanSpawn)
        {
            Debug.Log($"<color=green>All wave {_waveNumber} enemies defeated. Starting recovery period.</color>");

            StopAllCoroutines();

            _allEnemiesSpawned = false;

            i.StartCoroutine(BetweenWaveBuffer());
        }
    }

    #region Manage spawning
    private void StartSpawningNextWave()
    {
        if (_player)
        {
            if (_waveNumber <= 0)
                _waveNumber = 1;

            CanSpawn = true;

            UIManager.i.ChangeWave(_waveNumber);

            int numberOfEnemiesThisWave = Mathf.RoundToInt(_enemiesEachWave.Evaluate(_waveNumber - 1));

            if (numberOfEnemiesThisWave > 0)
            {
                if (_enemyContainer && _enemies.HasArrayEntries())
                    StartCoroutine(ManageEnemySpawning(numberOfEnemiesThisWave));

                if (_asteroidContainer && _asteroids.HasArrayEntries())
                    StartCoroutine(ManageAsteroidSpawning());

                if (_powerupContainer && _powerups.HasArrayEntries() && _refills.HasArrayEntries())
                    StartCoroutine(ManagePowerupSpawning());
            }
            else if (numberOfEnemiesThisWave == -1)
            {
                SpawnBoss();

                _spawnAsteroids = false;

                if (_powerupContainer && _powerups.HasArrayEntries() && _refills.HasArrayEntries())
                    StartCoroutine(ManagePowerupSpawning());
            }

            #if UNITY_EDITOR
            string enemies = numberOfEnemiesThisWave == -1 ?
                " <color=orange>Boss Enemy</color>" : $" { numberOfEnemiesThisWave} Enemies";
            string E = _spawnEnemies ?
                enemies : string.Empty;
            string A = _spawnAsteroids ?
                " Asteroids" : string.Empty;
            string P = _spawnPowerups ?
                " Powerups" : string.Empty;
            string B(string pre, string post) => pre != string.Empty && post != string.Empty ? " and" : string.Empty;
            
            Debug.Log($"<color=blue>Started spawning{E}{B(E,A)}{A}{B(A,P)}{P} for Wave {_waveNumber}!</color>");
            #endif
        }
        else
        {
            Debug.LogError($"Missing the Player reference in the Spawn Manager. Spawning canceled!", gameObject);
            enabled = false;
        }    
    }

    private IEnumerator ManageEnemySpawning(int numberToSpawn)
    {
        yield return new WaitForSeconds(0.3f);

        int spawnedEnemies = 0;
        while (CanSpawn && _spawnEnemies && spawnedEnemies < numberToSpawn)
        {
            GameObject toSpawn = _enemies.GetWeightedSpawnable(_waveEnemyDifficulty.Evaluate(_waveNumber));
            Transform enemy = Instantiate(toSpawn, GetEnemySpawnPosition(1.5f), Quaternion.identity, _enemyContainer).transform;
            ChangeEnemiesAlive(enemy);
            spawnedEnemies++;

            yield return new WaitForSeconds(Random.Range(_enemySpawnDelay.x, _enemySpawnDelay.y));
        }

        Debug.Log($"<color=blue>Wave {_waveNumber} has finished spawning.</color>");

        _waveNumber++;
        _allEnemiesSpawned = true;
    }

    private IEnumerator ManageAsteroidSpawning()
    {
        yield return new WaitForSeconds(Random.Range(_asteroidSpawnDelay.x, _asteroidSpawnDelay.y));

        while (CanSpawn && _spawnAsteroids)
        {
            GameObject toSpawn = _asteroids.GetRandomWeightedSpawnable();
            EnemyMovement EM = Instantiate(toSpawn, GetAsteroidSpawnPosition(), Quaternion.identity, _asteroidContainer)
                .GetComponent<EnemyMovement>();
            EM.SetAsAsteroid();
            EM.SetDamageAmount(3);
            directionToPlayer = (_player.transform.position - EM.transform.position).normalized;
            EM.SetMovementDirection(directionToPlayer);
            yield return new WaitForSeconds(Random.Range(_asteroidSpawnDelay.x, _asteroidSpawnDelay.y));
        }
    }

    private IEnumerator ManagePowerupSpawning()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 10f));

        while (CanSpawn && _spawnPowerups)
        {
            int total = _powerups.ChanceForSpawn + _refills.ChanceForSpawn + _powerdowns.ChanceForSpawn;
            int powerupOrRefillOrPowerdown = Random.Range(0, total);

            GameObject toSpawn;
            if (powerupOrRefillOrPowerdown < _powerups.ChanceForSpawn)
                toSpawn = _powerups.GetRandomWeightedSpawnable();
            else if (powerupOrRefillOrPowerdown > (_powerups.ChanceForSpawn + _refills.ChanceForSpawn))
                toSpawn = _powerdowns.GetRandomWeightedSpawnable();
            else
                toSpawn = _refills.GetRandomWeightedSpawnable();

            Transform item = Instantiate(toSpawn, GetEnemySpawnPosition(1.5f), Quaternion.identity, _powerupContainer).transform;
            ChangeItemsExist(item);

            yield return new WaitForSeconds(Random.Range(_powerupSpawnDelay.x, _powerupSpawnDelay.y));
        }
    }

    public static void StartWaveBuffer() => i.StartCoroutine(i.BetweenWaveBuffer());

    private IEnumerator BetweenWaveBuffer()
    {
        if (!_player || _isBeforeStartAsteroid)
            yield break;

        int extraSecsOnBossWave = 6;
        bool isBossWave = i._enemiesEachWave.Evaluate(_waveNumber - 1) == -1;

        if (isBossWave)
        {
            i._player.GetComponent<PlayerHealth>().OverrideLives(i._bossWaveLives);
            UIManager.i.DisableHealthSprites();

            i._player.GetComponent<PlayerGun>().OverridePrimaryAmmo(i._bossWaveAmmo, i._bossWaveAmmo);
            UIManager.i.ChangeAmmo(i._bossWaveAmmo, i._bossWaveAmmo);

            i._spawnPowerups = false;
        }

        int seconds = (isBossWave ? i._waveRecoveryBuffer + extraSecsOnBossWave : i._waveRecoveryBuffer);
        for (int s = seconds; s >= 0; s--)
        {
            bool showText = s > (seconds - (isBossWave ? 9 : 3));
            string newText = showText ?
                (!isBossWave ?
                    (_waveNumber == 1) ?
                        "<wave>First wave\nincoming!" :
                        "<wave>Next wave\nincoming!" :
                    (s > (seconds - 3) ?
                        "<boss><wave>DREADNAUGHT\nINCOMING!" :
                        (s > (seconds - 6) ?
                            "<boss><wave><size=60>YOU GET</size>\nFIFTY AMMO\n<size=60>AND</size>\nFIFTEEN LIVES!" :
                            "<boss><wave><size=60>1. Shield Generators\n<size=75>2. Sentries\n<size=85>3. Main Body"))) :
                ((isBossWave ? "<boss>" : "") + ((s != 0) ? s.ToString() : "BEGIN!"));
            
            string current = i._waveText.text.Replace("<noparse></noparse>", string.Empty);
            string formatted = newText.Replace("<boss>", string.Empty).Replace("<wave>", string.Empty);

            if (current != formatted)
                i._waveText.SetText(newText);

            if (!_player || _isBeforeStartAsteroid)
                yield break;

            yield return new WaitForSeconds(1);
        }

        i._waveText.SetText(string.Empty);

        StartSpawningNextWave();
    }

    private void SpawnBoss()
    {
        Transform boss = Instantiate(_bossPrefab, new Vector3(0, 12, 0), Quaternion.identity, _enemyContainer).transform;
        ChangeEnemiesAlive(boss);
    }

    public static Vector2 GetEnemySpawnPosition() => GetEnemySpawnPosition(0);
    public static Vector2 GetEnemySpawnPosition(float Xoffset) => new Vector2(Random.Range(LevelBoundary.L(Xoffset), LevelBoundary.R(-Xoffset)), LevelBoundary.U(2));

    public static Vector2 GetAsteroidSpawnPosition()
    {
        float limit = 2.5f;

        Vector2 spawnPosition = new Vector2(Random.Range(LevelBoundary.L(-6), LevelBoundary.R(6)), LevelBoundary.U(7));

        if (spawnPosition.x > i._player.transform.position.x - limit && spawnPosition.x <= 0)
            spawnPosition.x = -limit;
        else if (spawnPosition.x > 0 && spawnPosition.x < i._player.transform.position.x + limit)
            spawnPosition.x = limit;

        return spawnPosition;
    }
    #endregion

    #region Update Lists
    public static void ChangeEnemiesAlive(Transform enemy)
    {
        if (EnemyList.Contains(enemy))
            EnemyList.Remove(enemy);
        else
            EnemyList.Add(enemy);
    }

    public static void ChangeItemsExist(Transform item)
    {
        if (ItemList.Contains(item))
            ItemList.Remove(item);
        else
            ItemList.Add(item);
    }

    public static Transform GetClosestEnemy(Transform origin)
    {
        Transform closest = null;
        if (EnemiesExist)
        {
            closest = EnemyList[0];

            for (int i = 1; i < EnemyList.Count; i++)
            {
                if (Vector2.Distance(origin.position, EnemyList[i].position) <
                    Vector2.Distance(origin.position, closest.position))
                    closest = EnemyList[i];
            }
        }
        return closest;
    }
    #endregion
}