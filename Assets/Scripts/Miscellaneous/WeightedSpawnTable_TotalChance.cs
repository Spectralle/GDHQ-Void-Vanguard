using UnityEngine;

[System.Serializable]
public struct WeightedSpawnTable_TotalChance
{
    [Range(0, 100)]
    public int ChanceForSpawn;
    public SpawnTableObject[] SpawnList;
    public bool HasArrayEntries() => SpawnList.Length > 0;

    private int Total()
    {
        int t = 0;
        foreach (SpawnTableObject obj in SpawnList)
            t += obj.Chance;
        return t;
    }

    [System.Serializable]
    public struct SpawnTableObject
    {
        public GameObject Object;
        [Range(0,100)]
        public int Chance;
    }


    public GameObject GetRandomWeightedSpawnable()
    {
        int randomSpawnValue = Random.Range(0, Total());
        int currentRangeMin = 0;

        foreach (SpawnTableObject obj in SpawnList)
        {
            if (randomSpawnValue >= currentRangeMin && randomSpawnValue < (currentRangeMin + obj.Chance))
                return obj.Object;
            currentRangeMin += obj.Chance;
        }

        Debug.LogError("Spawn table returned NULL when that should be impossible!\n" +
            randomSpawnValue + " is not in range (0->" + Total() + ")");
        return null;
    }
}