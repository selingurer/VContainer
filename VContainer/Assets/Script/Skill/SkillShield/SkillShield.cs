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

            // UniTask.Delay s�ras�nda oyunun kapanmas�n� �nlemek i�in iptal edilebilir
            await UniTask.Delay(10000, cancellationToken: cancellationToken);

            // E�er iptal edilmediyse kalkan� devre d��� b�rak
            if (!cancellationToken.IsCancellationRequested)
            {
                objectSkill.gameObject.SetActive(false);
                dataPlayer.Shield = false;
            }
        }
        catch (OperationCanceledException)
        {
            // E�er i�lem iptal edilirse burada kontrol edilebilir
            Debug.Log("Skill shield task was cancelled.");
        }
        catch (MissingReferenceException ex)
        {
            // Oyun kapand���nda referans hatas� al�n�rsa
            Debug.LogError("Missing reference: " + ex.Message);
        }
    }
}
