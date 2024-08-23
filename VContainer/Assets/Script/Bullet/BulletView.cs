using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public float _attackValue { get; set; }
    private ITargetable<Component> _target;
    private Component _owner;
    public Action<BulletView> ReturnToPoolBulletAction;
    public void SetTarget(ITargetable<Component> target,Component owner)
    {
        _target = (ITargetable<Component>)(target as Component);
        _owner = owner;
        transform.DOMove((_target as Component).transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ReturnToPoolBulletAction?.Invoke(this);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent(_owner.GetType()) != null)
            return;

        var target = collision.GetComponent<ITargetable<Component>>();
        if (target != null)
        {
            target.TakeDamage(_attackValue);
            ReturnToPoolBulletAction?.Invoke(this);
        }
    }
}