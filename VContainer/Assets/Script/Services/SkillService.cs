using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public enum SkillTypes
{
    SpeedPlayer = 0,
    ShieldPlayer = 1,
    HeartPlayer = 2
}
public class SkillService : ISkillService, IStartable
{
    public List<Skill> _skillList { get; set; }

    public List<Skill> GetSkillList()
    {

        List<Skill> selectedSkills = new List<Skill>();

        if (_skillList.Count <= 3)
        {
            return new List<Skill>(_skillList);
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
    public void AddSkill(Skill skill)
    {
        _skillList.Add(skill);
    }
    public void SelectedSkill(SkillTypes types, Skill skill)
    {
        _skillList.Remove(skill);
    }

    public void Start()
    {
        _skillList = new List<Skill>();
    }
}
