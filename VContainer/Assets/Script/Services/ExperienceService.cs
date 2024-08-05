
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

public class ExperienceService : IStartable ,IDisposable
{
    private int exValue;
    public ObjectPool<Experience> _experiencePool;
    public GameService _gameService;

    private CancellationTokenSource _cancellationTokenSource;
    public void GetExperience(Vector3 pos)
    {
        var obj = _experiencePool.Get();
        obj.transform.position = pos;
        _ = DestroyExperience(obj);
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
    private async UniTaskVoid DestroyExperience(Experience experience)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: true);
        if(experience!= null) ReturnToExperiencePool(experience);
    }
    public void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        if(_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
        _cancellationTokenSource = new CancellationTokenSource();
    }
}
