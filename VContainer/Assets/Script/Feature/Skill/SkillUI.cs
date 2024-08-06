using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Button btnSkill;
    [SerializeField] private Text skillText;
    [SerializeField] private Image imgSkill;

    public Skill _skill;
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
        _skill.SelectSkill();
       transform.parent.parent.gameObject.SetActive(false);
    }

    public void SetSkillData(SkillData skillData)
    {
        skillText.text = skillData.DescSkill;
        imgSkill.sprite = skillData.SpriteSkill;
    }
}
