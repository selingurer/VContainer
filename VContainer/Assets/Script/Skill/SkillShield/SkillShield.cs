using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SkillShield : ISkillShield
{
    IObjectResolver _resolver;
    GameObject _skillSheildObject;

    [Inject]
    private void Construct(IObjectResolver resolver, GameObject skillSheildObj)
    {
        _resolver = resolver;
        _skillSheildObject = skillSheildObj;
    }

    public async UniTask SetSkillShield(PlayerData dataPlayer, Transform transform)
    {
        dataPlayer.Shield = true;
        var objectSkill = _resolver.Instantiate(_skillSheildObject);
        objectSkill.transform.SetParent(transform, false);
        await UniTask.Delay(10000);
        objectSkill.gameObject.SetActive(false);
        dataPlayer.Shield = false;
    }
}
