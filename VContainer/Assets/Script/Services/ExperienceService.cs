
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ExperienceService : IStartable
{
    private int exValue;
    public ObjectPool<Experience> _experiencePool;
    public GameService _gameService;

 
    public void GetExperience(Vector3 pos)
    {
        var obj = _experiencePool.Get();
        obj.transform.position = pos;
    }
    public void SetExperienceValue(int value)
    {
        exValue = value;
        _gameService.ExperienceValueChanged(exValue);
    }

    public void ReturnToExperiencePool(Experience experience)
    {
        _experiencePool.ReturnToPool(experience);
    }

    public void Start()
    {
        throw new NotImplementedException();
    }
}
