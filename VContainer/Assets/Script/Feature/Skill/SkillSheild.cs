using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SkillSheild : ISkillSheild
{
    IObjectResolver _resolver;
    GameObject _skillSheildObject;

    [Inject]
    private void Construct(IObjectResolver resolver, GameObject skillSheildObj)
    {
        _resolver = resolver;
        _skillSheildObject = skillSheildObj;
    }

    public async UniTask SetSkillSheild(PlayerData dataPlayer, Transform transform)
    {
        dataPlayer.Sheild = true;
        var objectSkill = _resolver.Instantiate(_skillSheildObject);
        objectSkill.transform.SetParent(transform, false);
        await UniTask.Delay(10000);
        objectSkill.gameObject.SetActive(false);
        dataPlayer.Sheild = false;
    }
}
