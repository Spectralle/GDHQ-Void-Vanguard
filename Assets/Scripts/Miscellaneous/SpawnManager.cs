using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager i;

    public bool CanSpawn = true;
    public int EnemiesAlive;
    public int PowerupsInLevel;

    [SerializeField] private GameObject _player;
    [Header("Enemies:")]
    [SerializeField] private bool _spawnEnemies = true;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Vector2 _enemySpawnDelay = new Vector2(2.5f, 5);
    [SerializeField] private GameObject[] _enemyTypes = new GameObject[0];
    [Header("PowerUps:")]
    [SerializeField] private bool _spawnPowerups = true;
    [SerializeField] private Transform _powerupContainer;
    [SerializeField] private Vector2 _powerupSpawnDelay = new Vector2(6, 17);
    [SerializeField] private GameObject[] _powerupTypes = new GameObject[0];


    private void Awake() => i = this;

    public static void StartSpawning()
    {
        if (i._player && i._enemyContainer && i._powerupContainer && i._enemyTypes.Length > 0 && i._powerupTypes.Length > 0)
        {
            string E = i._spawnEnemies ? " Enemies" : string.Empty;
            string P = i._spawnEnemies ? " Powerups" : string.Empty;
            string B = E != string.Empty && P != string.Empty ? " and" : string.Empty;
            Debug.Log($"Started spawning{E}{B}{P}!");

            i.StartCoroutine(ManageEnemySpawning());
            i.StartCoroutine(ManagePowerupSpawning());
        }
        else
        {
            Debug.Log($"Missing variables in the Spawn Manager on {i.name}. Spawning canceled!", i.gameObject);
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
        Debug.Log(i.CanSpawn + " " + i._spawnEnemies);

        while (i.CanSpawn && i._spawnEnemies)
        {
            Instantiate(i._enemyTypes[Random.Range(0, i._enemyTypes.Length)], GetSpawnPosition(1f), Quaternion.identity, i._enemyContainer);
            i.EnemiesAlive++;
            yield return new WaitForSeconds(Random.Range(i._enemySpawnDelay.x, i._enemySpawnDelay.y));
        }
    }
    
    private static IEnumerator ManagePowerupSpawning()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 10f));

        while (i.CanSpawn && i._spawnPowerups)
        {
            Instantiate(i._powerupTypes[Random.Range(0, i._powerupTypes.Length)],
                GetSpawnPosition(1.5f), Quaternion.identity, i._powerupContainer);
            i.PowerupsInLevel++;
            yield return new WaitForSeconds(Random.Range(i._powerupSpawnDelay.x, i._powerupSpawnDelay.y));
        }
    }

    public static Vector2 GetSpawnPosition() => GetSpawnPosition(0);
    public static Vector2 GetSpawnPosition(float Xoffset) => new Vector2(Random.Range(LevelBoundary.L(Xoffset), LevelBoundary.R(-Xoffset)), LevelBoundary.U(2));
}