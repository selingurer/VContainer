using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private List<T> pool;
    private T prefab;
    private Transform parentTransform;

    public ObjectPool(T prefab, int initialSize, Transform parentTransform = null)
    {
        this.prefab = prefab;
        this.parentTransform = parentTransform;
        pool = new List<T>(initialSize);
        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    private T AddObjectToPool()
    {
        T obj = GameObject.Instantiate(prefab, parentTransform);
        obj.gameObject.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public T Get()
    {
        foreach (T obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        return AddObjectToPool();
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }
}
