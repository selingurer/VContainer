using System;
using System.Collections.Generic;
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

    private async void OnLevelUp(int level, List<SkillData> skillDatalist)
    {
        await _gameUIPanel.LevelChanged(level);
        _gameUIPanel.CreateSkillCard(skillDatalist);
    }
}