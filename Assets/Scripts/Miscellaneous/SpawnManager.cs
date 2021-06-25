using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public static bool CanSpawn = true;
    public static int EnemiesAlive;

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _powerupContainer;
    [Space]
    [SerializeField] private Vector2 _enemySpawnDelay = new Vector2(2.5f, 5);
    [SerializeField] private Vector2 _powerupSpawnDelay = new Vector2(6, 17);
    [Space]
    [SerializeField] private GameObject[] _enemyTypes = new GameObject[0];
    [SerializeField] private GameObject[] _powerupTypes = new GameObject[0];


    private void Start()
    {
        if (_player && _enemyContainer && _powerupContainer && _enemyTypes.Length > 0 && _powerupTypes.Length > 0)
        {
            StartCoroutine(ManageEnemySpawning());
            StartCoroutine(ManagePowerupSpawning());
        }
        else
        {
            Debug.Log($"Missing variables in the Spawn Manager on {name}. Spawning canceled!", gameObject);
            enabled = false;
        }    
    }

    private void Update()
    {
        if (_player == null)
            CanSpawn = false;
    }

    IEnumerator ManageEnemySpawning()
    {
        yield return new WaitForSeconds(1.5f);

        while (CanSpawn)
        {
            Instantiate(_enemyTypes[Random.Range(0, _enemyTypes.Length)],
                GetSpawnPosition(1f), Quaternion.identity, _enemyContainer);
            EnemiesAlive++;
            yield return new WaitForSeconds(Random.Range(_enemySpawnDelay.x, _enemySpawnDelay.y));
        }
    }
    
    IEnumerator ManagePowerupSpawning()
    {
        yield return new WaitForSeconds(Random.Range(2.5f, 10f));

        while (CanSpawn)
        {
            Instantiate(_powerupTypes[Random.Range(0, _powerupTypes.Length)],
                GetSpawnPosition(1.5f), Quaternion.identity, _powerupContainer);
            yield return new WaitForSeconds(Random.Range(_powerupSpawnDelay.x, _powerupSpawnDelay.y));
        }
    }

    public static Vector2 GetSpawnPosition() => GetSpawnPosition(0);
    public static Vector2 GetSpawnPosition(float Xoffset) => new Vector2(Random.Range(LevelBoundary.L(Xoffset), LevelBoundary.R(-Xoffset)), LevelBoundary.U(2));
}