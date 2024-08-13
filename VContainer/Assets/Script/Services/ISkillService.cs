using System.Collections.Generic;

public interface ISkillService
{
    public SkillDataList _skillList { get; set; }
    public List<SkillData> GetSkillList();
    public void AddSkill();
    public void SelectedSkill(SkillTypes types, SkillData skill);
}
