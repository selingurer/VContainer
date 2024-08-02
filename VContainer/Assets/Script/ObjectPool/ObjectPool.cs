using Assets.Script.Services;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ObjectPool<T> where T : Component
{
    public readonly Queue<T> _objects = new Queue<T>();
    private T _prefab;
    private IObjectResolver _resolver;
    private Transform _parent;

    public ObjectPool(T prefab, int size, IObjectResolver resolver)
    {
        _prefab = prefab;
        //_parent = transform;
        _resolver = resolver;
        for (int i = 0; i < size; i++)
        {
            AddObjectToPool();
        }
    }

    // Havuzdan bir nesne alýyoruz.
    public T Get()
    {
        // Havuzda nesne yoksa, yeni bir tane oluþturuyoruz.
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

    // Havuz büyütülmesi gerektiðinde yeni bir nesne ekliyoruz.
    private T AddObjectToPool()
    {
        var newObject = _resolver.Instantiate(_prefab, _parent);
        newObject.gameObject.SetActive(false);
        _objects.Enqueue(newObject);
        return newObject;
    }
}
