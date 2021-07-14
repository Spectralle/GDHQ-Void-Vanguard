using UnityEngine;

[System.Serializable]
public struct WeightedSpawnTable
{
    [Range(0, 100)]
    public int ChanceForLoot;
    public LootTableObject[] LootList;
    public bool HasArrayEntries() => LootList.Length > 0;

    private int Total()
    {
        int t = 0;
        foreach (LootTableObject obj in LootList)
            t += obj.Chance;
        return t;
    }

    [System.Serializable]
    public struct LootTableObject
    {
        public GameObject Object;
        [Range(0,100)]
        public int Chance;
    }


    public GameObject GetRandomWeightedSpawnable()
    {
        int randomLootValue = Random.Range(0, Total());
        int currentRangeMin = 0;

        foreach (LootTableObject obj in LootList)
        {
            if (randomLootValue >= currentRangeMin && randomLootValue < (currentRangeMin + obj.Chance))
                return obj.Object;
            currentRangeMin += obj.Chance;
        }

        Debug.LogError("Loot table returned NULL when that should be impossible!\n" +
            randomLootValue + " is not in range (0->" + Total() + ")");
        return null;
    }
}