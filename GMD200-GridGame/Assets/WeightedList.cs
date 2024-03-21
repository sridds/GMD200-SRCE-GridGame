using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedList<T>
{
    [System.Serializable]
    public struct Pair
    {
        public T item;
        public float weight;

        public Pair(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }

    public List<Pair> list = new List<Pair>();
    public int Count { get => list.Count; }

    // Adds an item to the list of the specified weight value
    public void Add(T item, float weight) => list.Add(new Pair(item, weight));

    // Returns a random value of type T
    public T GetRandom()
    {
        float totalWeight = 0;

        foreach(Pair p in list)
        {
            totalWeight += p.weight;
        }

        float value = Random.value * totalWeight;
        float sumWeight = 0;

        foreach(Pair p in list)
        {
            sumWeight += p.weight;

            if(sumWeight >= value)
            {
                return p.item;
            }
        }

        return default(T);
    }
}