using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : MonoBehaviour
{
    private IAttack _attactValue;
    private BaseCharacter _targetObj;
    [Inject]
    private void Construct(IAttack attack)
    {
        _attactValue = attack;
    }
    public Bullet(IAttack attack)
    {  _attactValue = attack;}
    public void Target(BaseCharacter target)
    {
        transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
    }
   
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == _targetObj)
        {
            float Healt = _targetObj._healt.GetHealt() - _attactValue.GetAttack();
            _targetObj._healt.SetHealt(Healt);
        }
    }
}
