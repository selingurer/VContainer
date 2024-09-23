
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ExperienceService : IStartable, IDisposable
{
    public Action<int> ExperienceValueChanged;
    [Inject] private ObjectPool<ExperienceView> _experiencePool;
    private int exValue;
    private CancellationTokenSource _cancellationTokenSource;
    public void GetExperience(Vector3 pos)
    {
        var obj = _experiencePool.Get();
        obj.transform.position = pos;
        obj.ExperienceClaim += OnExperienceClaim;
        obj.ReturnToPoolExperienceAction += OnReturnToExperiencePool;
        _ = ReturnToPoolAsync(obj);
    }
    public void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
        _cancellationTokenSource = new CancellationTokenSource();
    }
    private void OnExperienceClaim(int value)
    {
        exValue = value;
        ExperienceValueChanged?.Invoke(exValue);
    }

    private void OnReturnToExperiencePool(ExperienceView experience)
    {
        _experiencePool.ReturnToPool(experience);
        experience.ExperienceClaim -= OnExperienceClaim;
        experience.ReturnToPoolExperienceAction -= OnReturnToExperiencePool;
    }
    private async UniTask ReturnToPoolAsync(ExperienceView experience)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(10));
        if (experience != null) OnReturnToExperiencePool(experience);
    }
}
