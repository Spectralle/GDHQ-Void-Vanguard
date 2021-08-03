using UnityEngine;

[System.Serializable]
public class WeightedSpawnTable_TotalChance : WeightedSpawnTable
{
    [Range(0, 100)]
    public int ChanceForSpawn;
}