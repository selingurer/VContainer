using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ObjectPool<T> where T : Component
{
    public readonly Queue<T> _objectsQ = new Queue<T>();
    private List<T> _objectList = new List<T>();
    private T _prefab;
    private IObjectResolver _resolver;
    private Transform _parent;
    private int _maxPoolSize;

    [Inject]
    private void Construct(T prefab, int size, IObjectResolver resolver, GameData gameData)
    {
        _prefab = prefab;
        _resolver = resolver;
        _maxPoolSize = gameData.MaxPoolSize;
        for (int i = 0; i < size; i++)
        {
            AddObjectToPool();
        }
    }

    public T Get()
    {
        if (_objectsQ.Count == 0)
        {
            AddObjectToPool();
        }

        var obj = _objectsQ.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _objectsQ.Enqueue(obj);
        ShrinkPool();
    }

    private T AddObjectToPool()
    {
        var newObject = _resolver.Instantiate(_prefab, _parent);
        newObject.gameObject.SetActive(false);
        _objectsQ.Enqueue(newObject);
        _objectList.Add(newObject);
        return newObject;
    }
    private void ShrinkPool()
    {

        //while (_objectList.Count > _maxPoolSize)
        //{
        //    var objectToRemove = _objectList[_objectList.Count - 1];
        //    if (objectToRemove != null)
        //    {
        //        _objectList.Remove(objectToRemove);
        //        Object.Destroy(objectToRemove.gameObject);
        //    }
        //}
    }
}
