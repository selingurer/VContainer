using DG.Tweening;
using UnityEngine;
using VContainer;

public class Bullet : MonoBehaviour
{
    public float _attackValue { get; set; }
    private Component _target;
    public void Target<T>(T target) where T : Component
    {
        transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
        _target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent(_target.GetType()) is not null)
        {
      //      float Healt = collision.gameObject.GetComponent<Enemy>().Healt -= _attackValue;
           // _bulletPool.ReturnToPool(this);
        }
    }

  
}
public class BulletSpawnerService
{
   [Inject] private ObjectPool<Bullet> _bulletPool;

    public void BulletReturnToPool(Bullet bullet)
    {

    }
    public void GetBullet<T>(T target,Vector3 pos,float attackValue) where T : Component
    {
        var obj = _bulletPool.Get();
        obj.transform.position = pos;
        obj.Target(target);
        obj._attackValue = attackValue;
    }
}