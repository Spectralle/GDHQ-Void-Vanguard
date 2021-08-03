using UnityEngine;

[System.Serializable]
public class WeightedSpawnTable
{
    public SpawnTableObject[] SpawnList;
    public bool HasArrayEntries() => SpawnList.Length > 0;
    private int Total() => Total(1);
    private int Total(float tierNormalized)
    {
        tierNormalized = Mathf.Clamp01(tierNormalized);
        int t = 0;
        for (int i = 0; i < SpawnList.Length; i++)
        {
            int subtractFromChance = Mathf.RoundToInt(SpawnList[i].Chance *
                (i / Mathf.Clamp((float)(SpawnList.Length - 1), 1, 999)) * tierNormalized);
            t += (SpawnList[i].Chance - subtractFromChance);
        }
        return t;
    }

    [System.Serializable]
    public struct SpawnTableObject
    {
        public GameObject Object;
        [Range(0, 100)]
        public int Chance;
    }


    public GameObject GetRandomWeightedSpawnable() => GetWeightedSpawnable(0);

    public GameObject GetWeightedSpawnable(float tierNormalized)
    {
        int randomSpawnValue = Random.Range(0, Total(tierNormalized));
        int currentRangeMin = 0;

        foreach (SpawnTableObject obj in SpawnList)
        {
            if (randomSpawnValue >= currentRangeMin && randomSpawnValue < (currentRangeMin + obj.Chance))
                return obj.Object;
            else
                currentRangeMin += obj.Chance;
        }

        Debug.LogError("Spawn table returned NULL when that should be impossible!\n" +
            randomSpawnValue + " is not in range (0->" + Total(tierNormalized) + ")");
        return null;
    }
}