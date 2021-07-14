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
    [Header("PowerUps:")]
    [SerializeField] private bool _spawnPowerups = true;
    [SerializeField] private Transform _powerupContainer;
    [SerializeField] private Vector2 _powerupSpawnDelay = new Vector2(6, 17);
    [SerializeField] private WeightedSpawnTable _powerups;
    [SerializeField] private WeightedSpawnTable _refills;

    private void Awake() => i = this;

    public static void StartSpawning()
    {
        if (i._player)
        {
            #if UNITY_EDITOR
            string E = i._spawnEnemies ? " Enemies" : string.Empty;
            string P = i._spawnEnemies ? " Powerups" : string.Empty;
            string B = E != string.Empty && P != string.Empty ? " and" : string.Empty;
            Debug.Log($"Started spawning{E}{B}{P}!");
            #endif

            if (i._enemyContainer && i._enemies.HasArrayEntries())
                i.StartCoroutine(ManageEnemySpawning());
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
    
    private static IEnumerator ManagePowerupSpawning()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 10f));

        while (i.CanSpawn && i._spawnPowerups)
        {
            int total = i._powerups.ChanceForLoot + i._refills.ChanceForLoot;
            int powerupOrRefill = Random.Range(0, total);

            GameObject toSpawn = powerupOrRefill < i._powerups.ChanceForLoot ?
                i._powerups.GetRandomWeightedSpawnable() :
                i._refills.GetRandomWeightedSpawnable();

            Instantiate(toSpawn, GetSpawnPosition(1.5f), Quaternion.identity, i._powerupContainer);
            i.ItemsInLevel++;
            yield return new WaitForSeconds(Random.Range(i._powerupSpawnDelay.x, i._powerupSpawnDelay.y));
        }
    }

    public static Vector2 GetSpawnPosition() => GetSpawnPosition(0);
    public static Vector2 GetSpawnPosition(float Xoffset) => new Vector2(Random.Range(LevelBoundary.L(Xoffset), LevelBoundary.R(-Xoffset)), LevelBoundary.U(2));
}