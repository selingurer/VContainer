using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    [SerializeField] private Button btnSkill;
    [SerializeField] private Text skillText;
    [SerializeField] private Image imgSkill;

    private SkillData _skill;
    private void OnEnable()
    {
        btnSkill.onClick.AddListener(OnBtnSkillClicked);
    }
    private void OnDisable()
    {
        btnSkill.onClick.RemoveListener(OnBtnSkillClicked);
    }
    private void OnBtnSkillClicked()
    {
       transform.parent.parent.gameObject.SetActive(false);
        _skill.SkillAction();
    }

    public void SetSkillData(SkillData skillData)
    {
        skillText.text = skillData.DescSkill;
        imgSkill.sprite = skillData.SpriteSkill;
        _skill = skillData;
    }
}
