using System.Collections.Generic;

public interface ISkillService
{
    public List<Skill> _skillList { get; set; }
    public List<Skill> GetSkillList();
    public void AddSkill(Skill skill);
    public void SelectedSkill(SkillTypes types, Skill reward);
}
