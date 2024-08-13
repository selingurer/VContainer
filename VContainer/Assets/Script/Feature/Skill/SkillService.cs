using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SkillService : IStartable
{
    public List<SkillData> _skillList { get; set; }

    [Inject] PlayerPresenter _playerPresenter;

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
            int randomIndex = Random.Range(0, _skillList.Count);
            selectedIndices.Add(randomIndex);
        }

        foreach (int index in selectedIndices)
        {
            selectedSkills.Add(_skillList[index]);
        }

        return selectedSkills;

    }
    public void AddSkill()
    {
        _skillList.AddRange(new List<SkillData>
        {
            new SkillData()
            {
               SkillAction  = async () =>
               {
                   Time.timeScale = 1;
                  await _playerPresenter._skillSheild.SetSkillSheild(_playerPresenter._dataPlayer,_playerPresenter.GetTransform());
               },
               SkillType = SkillTypes.Shield,
               DescSkill = "Koruma Kalkanı sağlar. 10 saniye boyunca hasar almazsın.",
               SpriteSkill = Resources.Load<Sprite>("HeartPlusIcon")
            },
            new SkillData()
            {
                 SkillAction = () =>
                 {
                  _playerPresenter._skillHealth.SetSkillHealth(_playerPresenter._dataPlayer);
                   Time.timeScale = 1;
                 },
               SkillType = SkillTypes.Heart,
               DescSkill = "Can hasarını %100 iyileştirir.",
               SpriteSkill = Resources.Load<Sprite>("HeartSkill")

            },
            new SkillData()
            {
               SkillAction = () =>
                 {
                   Time.timeScale = 1;
                   _playerPresenter._skillSpeed.SetSkillSpeed(_playerPresenter._dataPlayer);
                 },
               SkillType = SkillTypes.Speed,
               DescSkill = "Hızını %30 arttırır.",
               SpriteSkill = Resources.Load<Sprite>("SpeedSkill")
            }
        });
    }


    public void SelectedSkill(SkillData skill)
    {
        _skillList.Remove(skill);
        skill.SkillAction();
    }

    public void Start()
    {
        _skillList = new List<SkillData>();
        AddSkill();
    }


}