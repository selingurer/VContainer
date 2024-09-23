using DG.Tweening;
using System;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public float _attackValue { get; set; }
    public Action<BulletView> ReturnToPoolBulletAction;
    private Component _owner;
    public void SetTarget(ITargetable target,Component owner)
    {
        _owner = owner;
        transform.DOMove((target as Component).transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ReturnToPoolBulletAction?.Invoke(this);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent(_owner.GetType()) != null)
            return;

        var target = collision.GetComponent<ITargetable>();
        if (target != null)
        {
            target.TakeDamage(_attackValue);
            ReturnToPoolBulletAction?.Invoke(this);
        }
    }
}