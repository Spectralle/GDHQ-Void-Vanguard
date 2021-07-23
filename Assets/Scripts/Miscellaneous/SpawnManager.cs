using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager i;

    [HideInInspector] public bool CanSpawn = true;
    [HideInInspector] public int EnemiesAlive;
    [HideInInspector] public int ItemsInLevel;

    [SerializeField] private GameObject _player;
    [Header("Enemies:")]
    [SerializeField] private bool _spawnEnemies = true;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Vector2 _enemySpawnDelay = new Vector2(2.5f, 5);
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
    [SerializeField] private WeightedSpawnTable _powerups;
    [SerializeField] private WeightedSpawnTable _refills;
    [SerializeField] private WeightedSpawnTable _powerdowns;

    private Vector3 directionToPlayer = Vector3.zero;


    private void Awake() => i = this;

    public static void StartSpawning()
    {
        if (i._player)
        {
            #if UNITY_EDITOR
            string E = i._spawnEnemies ? " Enemies" : string.Empty;
            string A = i._spawnAsteroids ? " Asteroids" : string.Empty;
            string P = i._spawnEnemies ? " Powerups" : string.Empty;
            string B(string pre, string post) => pre != string.Empty && post != string.Empty ? " and" : string.Empty;
            Debug.Log($"Started spawning{E}{B(E,A)}{A}{B(A,P)}{P}!");
            #endif

            if (i._enemyContainer && i._enemies.HasArrayEntries())
                i.StartCoroutine(ManageEnemySpawning());

            if (i._asteroidContainer && i._asteroids.HasArrayEntries())
                i.StartCoroutine(ManageAsteroidSpawning());

            if (i._powerupContainer && i._powerups.HasArrayEntries() && i._refills.HasArrayEntries())
                i.StartCoroutine(ManagePowerupSpawning());
        }
        else
        {
            Debug.Log($"Missing the Player reference in the Spawn Manager. Spawning canceled!", i.gameObject);
            i.enabled = false;
        }    
    }

    private void Update()
    {
        if (_player == null)
            CanSpawn = false;
    }

    private static IEnumerator ManageEnemySpawning()
    {
        yield return new WaitForSeconds(0.5f);

        while (i.CanSpawn && i._spawnEnemies)
        {
            GameObject toSpawn = i._enemies.GetRandomWeightedSpawnable();
            Instantiate(toSpawn, GetSpawnPosition(1f), Quaternion.identity, i._enemyContainer);
            i.EnemiesAlive++;
            yield return new WaitForSeconds(Random.Range(i._enemySpawnDelay.x, i._enemySpawnDelay.y));
        }
    }

    private static IEnumerator ManageAsteroidSpawning()
    {
        yield return new WaitForSeconds(Random.Range(i._asteroidSpawnDelay.x, i._asteroidSpawnDelay.y));

        while (i.CanSpawn && i._spawnAsteroids)
        {
            GameObject toSpawn = i._asteroids.GetRandomWeightedSpawnable();
            EnemyMovement EM = Instantiate(toSpawn, GetAsteroidSpawnPosition(), Quaternion.identity, i._asteroidContainer)
                .GetComponent<EnemyMovement>();
            EM.SetAsAsteroid();
            EM.SetDamageAmount(3);
            i.directionToPlayer = (i._player.transform.position - EM.transform.position).normalized;
            EM.SetMovementDirection(i.directionToPlayer);
            yield return new WaitForSeconds(Random.Range(i._asteroidSpawnDelay.x, i._asteroidSpawnDelay.y));
        }
    }

    private static IEnumerator ManagePowerupSpawning()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 10f));

        while (i.CanSpawn && i._spawnPowerups)
        {
            int total = i._powerups.ChanceForLoot + i._refills.ChanceForLoot + i._powerdowns.ChanceForLoot;
            int powerupOrRefillOrPowerdown = Random.Range(0, total);

            GameObject toSpawn;
            if (powerupOrRefillOrPowerdown < i._powerups.ChanceForLoot)
                toSpawn = i._powerups.GetRandomWeightedSpawnable();
            else if (powerupOrRefillOrPowerdown > (i._powerups.ChanceForLoot + i._refills.ChanceForLoot))
                toSpawn = i._powerdowns.GetRandomWeightedSpawnable();
            else
                toSpawn = i._refills.GetRandomWeightedSpawnable();

            Instantiate(toSpawn, GetSpawnPosition(1.5f), Quaternion.identity, i._powerupContainer);
            i.ItemsInLevel++;
            yield return new WaitForSeconds(Random.Range(i._powerupSpawnDelay.x, i._powerupSpawnDelay.y));
        }
    }

    public static Vector2 GetSpawnPosition() => GetSpawnPosition(0);
    public static Vector2 GetSpawnPosition(float Xoffset) => new Vector2(Random.Range(LevelBoundary.L(Xoffset), LevelBoundary.R(-Xoffset)), LevelBoundary.U(2));

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
}