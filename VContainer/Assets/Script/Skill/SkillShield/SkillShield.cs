using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
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

    public async UniTask SetSkillShield(PlayerData dataPlayer, Transform transform, CancellationToken cancellationToken)
    {
        try
        {
            dataPlayer.Shield = true;
            var objectSkill = _resolver.Instantiate(_skillSheildObject);
            objectSkill.transform.SetParent(transform, false);

            // UniTask.Delay sýrasýnda oyunun kapanmasýný önlemek için iptal edilebilir
            await UniTask.Delay(10000, cancellationToken: cancellationToken);

            // Eðer iptal edilmediyse kalkaný devre dýþý býrak
            if (!cancellationToken.IsCancellationRequested)
            {
                objectSkill.gameObject.SetActive(false);
                dataPlayer.Shield = false;
            }
        }
        catch (OperationCanceledException)
        {
            // Eðer iþlem iptal edilirse burada kontrol edilebilir
            Debug.Log("Skill shield task was cancelled.");
        }
        catch (MissingReferenceException ex)
        {
            // Oyun kapandýðýnda referans hatasý alýnýrsa
            Debug.LogError("Missing reference: " + ex.Message);
        }
    }
}
