using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ObjectPool<Bullet> _bulletPool;
    public float _attackValue { get; set; }
   
    public void Target<T>(T target) where T : Component
    {
        transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() is not null)
        {
            float Healt = collision.gameObject.GetComponent<Enemy>().Healt -= _attackValue;
            _bulletPool.ReturnToPool(this);
        }
    }

  
}
