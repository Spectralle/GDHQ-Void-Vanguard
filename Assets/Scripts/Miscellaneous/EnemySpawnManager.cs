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
        Vector3 spawnPosition = new Vector3(
            Random.Range(LevelBoundary.L(2), LevelBoundary.R(-2)),
            LevelBoundary.U(2),
            transform.position.z
        );
        Instantiate(enemy, spawnPosition, Quaternion.identity, _enemyContainer);
        EnemiesAlive++;
    }
}