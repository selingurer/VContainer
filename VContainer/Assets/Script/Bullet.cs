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
    private ObjectPool<Bullet> _bulletPool;
    public void SetAttack(IAttack attack)
    {
        Debug.LogWarning(attack.GetAttack());
        _attactValue = attack;
    }
    public void Target(BaseCharacter target)
    {
        _targetObj = target;
        transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
    }
    public void SetObjectPool(ObjectPool<Bullet> pool)
    {
        _bulletPool = pool;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _targetObj.tag)
        {
            float Healt = collision.gameObject.GetComponent<Enemy>()._healt.GetHealt() - _attactValue.GetAttack();
            collision.gameObject.GetComponent<Enemy>()._healt.SetHealt(Healt, collision.gameObject.GetComponent<Enemy>());
            _bulletPool.ReturnToPool(this);
        }
    }

  
}
