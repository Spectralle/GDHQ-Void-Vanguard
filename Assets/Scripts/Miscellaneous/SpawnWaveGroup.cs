using UnityEngine;


[CreateAssetMenu(fileName = "SpawnWave Group", menuName = "SpawnWave Group", order = 0)]
public class SpawnWaveGroup : ScriptableObject
{
    public WeightedSpawnTable[] SpawnWaves;
}