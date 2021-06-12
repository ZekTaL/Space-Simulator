using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class the manages the spawn pool of the game objects
/// </summary>
public class SpawnPool : MonoBehaviour
{
    /// <summary>
    /// Struct for the spawn pool item
    /// </summary>
    [System.Serializable]
    public struct SpawnPoolItem
    {
        public GameObject PoolObject;
        public int poolAmount;
        public bool canAddMore;
    }

    public static SpawnPool instance;

    public List<SpawnPoolItem> spawnPoolList;
    public List<GameObject> spawnedObjects;


    private void Awake()
    {
        instance = this;
    }

    // Pre-Instantiate all the gameobject in the spawn pool
    void Start()
    {
        spawnedObjects = new List<GameObject>();

        foreach (SpawnPoolItem item in spawnPoolList)
        {
            for (int i = 0; i < item.poolAmount; i++)
            {
                GameObject obj = Instantiate(item.PoolObject);
                obj.SetActive(false);
                spawnedObjects.Add(obj);
            }
        }

        Game.SpawnAsteroids();
    }

    /// <summary>
    /// Retrieve a gameobject in the spawn pool
    /// </summary>
    /// <param name="tag">Tag of the gameobject searched</param>
    /// <returns>The gameobject with the tag searched</returns>
    public GameObject GetPoolObject(string tag)
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (!spawnedObjects[i].activeInHierarchy && spawnedObjects[i].CompareTag(tag))
            {
                return spawnedObjects[i];
            }
        }

        foreach (SpawnPoolItem item in spawnPoolList)
        {
            if (item.PoolObject.CompareTag(tag))
            {
                if (item.canAddMore)
                {
                    GameObject obj = Instantiate(item.PoolObject);
                    obj.SetActive(false);
                    spawnedObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}
