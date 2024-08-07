using UnityEngine;
using VContainer;

public class SkillUIBase : MonoBehaviour
{
    public  Skill _skill;

    [Inject]
    private void Construct(Skill skill)
    {
        _skill = skill;
    }

}
