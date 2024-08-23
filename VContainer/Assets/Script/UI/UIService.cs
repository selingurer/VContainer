using System;
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
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnPlayerHealthChanged += async (currentHealth, maxHealth) =>
            await _gameUIPanel.SliderHeartValueChanged((int)currentHealth, (int)maxHealth);

        GameEvents.OnExperienceChanged += async (currentExp, expTarget) =>
            await _gameUIPanel.ExperienceValueChanged(currentExp, expTarget);

        GameEvents.OnLevelUp += async (level, skillDatalist) =>
        {
            await _gameUIPanel.LevelChanged(level);
            _gameUIPanel.CreateSkillCard(skillDatalist);
        };


        GameEvents.OnGameOver += (data) =>
            _gameUIPanel.GameOver(data);

        GameEvents.OnSkillSelected += () =>
            _gameUIPanel.ClearSkillCard();
    }

    public void Dispose()
    {
        GameEvents.OnPlayerHealthChanged -= async (currentHealth, maxHealth) =>
            await _gameUIPanel.SliderHeartValueChanged((int)currentHealth, (int)maxHealth);

        GameEvents.OnExperienceChanged -= async (currentExp, expTarget) =>
            await _gameUIPanel.ExperienceValueChanged(currentExp, expTarget);

        GameEvents.OnLevelUp -= async (level, skillDatalist) =>
        {
            await _gameUIPanel.LevelChanged(level);
            _gameUIPanel.CreateSkillCard(skillDatalist);
        };

        GameEvents.OnGameOver -= (data) =>
            _gameUIPanel.GameOver(data);

        GameEvents.OnSkillSelected -= () =>
            _gameUIPanel.ClearSkillCard();
    }

    public void Start()
    {
   
    }
}