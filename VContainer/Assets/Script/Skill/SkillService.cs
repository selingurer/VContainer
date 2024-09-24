using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SkillService : IStartable,IDisposable
{
    public List<SkillData> _skillList { get; set; }
    CancellationTokenSource _cancellationToken;
   [Inject] PlayerService _playerService;
    public Action SelectSkillAction;
    public List<SkillData> GetSkillList()
    {

        List<SkillData> selectedSkills = new List<SkillData>();

        if (_skillList.Count <= 3)
        {
            return new List<SkillData>(_skillList);
        }

        HashSet<int> selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, _skillList.Count);
            selectedIndices.Add(randomIndex);
        }

        foreach (int index in selectedIndices)
        {
            selectedSkills.Add(_skillList[index]);
        }

        return selectedSkills;

    }
    private void AddSkill()
    {
        _skillList.AddRange(new List<SkillData>
        {
            new SkillData()
            {
               SkillAction  = async () =>
               {
                   SelectedSkill();
                   _playerService.SetSkillShield();
                   //   await _playerService._skillSheild.SetSkillShield(_playerService._dataPlayer,_playerService.GetTransform(),_cancellationToken.Token);
               },
               SkillType = SkillTypes.Shield,
               DescSkill = "Koruma Kalkanı sağlar. 10 saniye boyunca hasar almazsın.",
               SpriteSkill = Resources.Load<Sprite>("HeartPlusIcon")
            },
            new SkillData()
            {
                 SkillAction = () =>
                 {
                  SelectedSkill();
                  _playerService.SetSkillHealth();
                //  _playerService._skillHealth.SetSkillHealth(_playerService._dataPlayer);
                 },
               SkillType = SkillTypes.Heart,
               DescSkill = "Can hasarını %100 iyileştirir.",
               SpriteSkill = Resources.Load<Sprite>("HeartSkill")

            },
            new SkillData()
            {
               SkillAction = () =>
                 {
                   SelectedSkill();
                 // _playerService._skillSpeed.SetSkillSpeed(_playerService._dataPlayer);
                 },
               SkillType = SkillTypes.Speed,
               DescSkill = "Hızını %30 arttırır.",
               SpriteSkill = Resources.Load<Sprite>("SpeedSkill")
            }
        });
    }


    private void SelectedSkill()
    {
        SelectSkillAction?.Invoke();
        Time.timeScale = 1;
    }

    public void Start()
    {
        _skillList = new List<SkillData>();
        AddSkill();
    }

    public void Dispose()
    {
        if (_cancellationToken != null)
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = null;
        }
    }
}