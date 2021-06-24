using System.Collections;
using UnityEngine;


public class EnemySpawnManager : MonoBehaviour
{
    public static bool CanSpawn = true;
    public static int EnemiesAlive;

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _enemyContainer;
    [Space]
    [SerializeField] private int _spawnDelay = 3;
    [Space]
    [SerializeField] private GameObject[] _enemyTypes = new GameObject[0];


    private void Start()
    {
        if (_player && _enemyContainer && _enemyTypes.Length > 0)
            StartCoroutine(ManageSpawning());
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

    IEnumerator ManageSpawning()
    {
        yield return new WaitForSeconds(1.5f);

        while (CanSpawn)
        {
            Spawn(_enemyTypes[0]);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void Spawn(GameObject enemy)
    {
        Instantiate(enemy, GetSpawnPosition(), Quaternion.identity, _enemyContainer);
        EnemiesAlive++;
    }

    public static Vector2 GetSpawnPosition()
    {
        return new Vector2(
            Random.Range(LevelBoundary.L(0.5f), LevelBoundary.R(-0.5f)),
            LevelBoundary.U(2)
        );
    }
}