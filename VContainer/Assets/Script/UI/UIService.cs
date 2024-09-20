using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class UIService : IStartable, IDisposable
{
    private GameUIPanel _gameUIPanel;

    [Inject]
    private void Construct(GameUIPanel gameUIPanel)
    {
        _gameUIPanel = gameUIPanel;
    }
    public void Start()
    {
        SubscribeToEvents();
    }
    public void Dispose()
    {
        GameEvents.OnPlayerHealthChanged -= OnPlayerHeathChanged;
        GameEvents.OnExperienceChanged -= OnExperienceChanged;
        GameEvents.OnLevelUp -= OnLevelUp;
        GameEvents.OnGameOver -= _gameUIPanel.GameOver;
        GameEvents.OnSkillSelected -= _gameUIPanel.ClearSkillCard;
    }
    private void SubscribeToEvents()
    {
        GameEvents.OnPlayerHealthChanged += OnPlayerHeathChanged;
        GameEvents.OnExperienceChanged += OnExperienceChanged;
        GameEvents.OnLevelUp += OnLevelUp;
        GameEvents.OnGameOver += _gameUIPanel.GameOver;
        GameEvents.OnSkillSelected += _gameUIPanel.ClearSkillCard;
    }

    private void OnExperienceChanged(int currentExperience, int maxExperience)
    {
        _gameUIPanel.ExperienceValueChanged(currentExperience, maxExperience);
    }

    private void OnPlayerHeathChanged(float currentHealth, float maxHealth)
    {
        _gameUIPanel.SliderHeartValueChanged((int)currentHealth, (int)maxHealth);
    }
    private void OnLevelUp(int level, List<SkillData> skillDatalist)
    {
        _gameUIPanel.LevelChanged(level).Forget();
        _gameUIPanel.CreateSkillCard(skillDatalist);
    }
  
}