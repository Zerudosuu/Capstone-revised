using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int initialSize = 4;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        // Pre-instantiate objects and add them to the pool
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false); // Deactivate the object
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true); // Activate and return the object
            return obj;
        }
        else
        {
            // If the pool is empty, instantiate a new object
            GameObject obj = Instantiate(prefab, transform);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // Deactivate the object
        pool.Enqueue(obj); // Add it back to the pool
    }
}
