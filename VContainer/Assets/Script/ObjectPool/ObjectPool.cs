using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> _objects = new Queue<T>();
    private readonly T _prefab;
    private readonly IObjectResolver _resolver;
    private readonly Transform _parent;

    // Object Pool'u olu�tururken prefab ve resolver al�yoruz.
    public ObjectPool(T prefab, int initialSize, IObjectResolver resolver, Transform parent)
    {
        _prefab = prefab;
        _resolver = resolver;
        _parent = parent;

        // Ba�lang��ta belirli say�da nesne olu�turarak havuza ekliyoruz.
        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    // Havuzdan bir nesne al�yoruz.
    public T Get()
    {
        // Havuzda nesne yoksa, yeni bir tane olu�turuyoruz.
        if (_objects.Count == 0)
        {
            AddObjectToPool();
        }

        var obj = _objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    // Nesneyi havuza geri koyuyoruz.
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
    }

    // Havuz b�y�t�lmesi gerekti�inde yeni bir nesne ekliyoruz.
    private void AddObjectToPool()
    {
        var newObject = _resolver.Instantiate(_prefab, _parent);
        _objects.Enqueue(newObject);
    }
}
