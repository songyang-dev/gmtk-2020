using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pools enemies into the scene for performance
/// </summary>
public class Pooler : MonoBehaviour
{
    /// <summary>
    /// Reference to the pooled prefabs, in order of names
    /// </summary>
    public GameObject[] Prefabs;

    /// <summary>
    /// Min size of each pool
    /// </summary>
    public int PoolSize;

    /// <summary>
    /// Size of a large pool
    /// </summary>
    public int LargePoolSize;

    /// <summary>
    /// Holds references to the disabled enemies
    /// </summary>
    private Dictionary<string, Queue<GameObject>> pools;

    /// <summary>
    /// Instantiates enemies to fill the pool, but disables them
    /// </summary>
    void Start()
    {
        if (PoolSize == 0 || LargePoolSize == 0)
            Debug.LogError("Size of pool is 0");

        // initialize pools
        pools = new Dictionary<string, Queue<GameObject>>();
        for (int i = 0; i < Prefabs.Length; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject>(PoolSize);

            // populate pool
            for (int j = 0; j < pool.Count ; j++)
            {
                GameObject item = Instantiate(Prefabs[i], Vector3.zero, Quaternion.identity, this.transform);
                item.SetActive(false);
                pool.Enqueue(item);
            }

            pools.Add(Prefabs[i].name, pool);
        }
    }

    /// <summary>
    /// Retrieves entities from the pool
    /// </summary>
    /// <param name="nameOfPrefab">Name of the prefab that is pooled</param>
    /// <returns>Active gameobject at origin</returns>
    public GameObject TakeFromPool(string nameOfPrefab)
    {
        var item = pools[nameOfPrefab].Dequeue();
        item.SetActive(true);
        return item;
    }

    /// <summary>
    /// Stores entities that would be destroyed to the pool
    /// </summary>
    /// <param name="nameofPrefab">Name of the prefab that is pooled</param>
    public void StoreToPool(string nameofPrefab, GameObject gameObject)
    {
        gameObject.SetActive(false);
        pools[nameofPrefab].Enqueue(gameObject);
    }
}
