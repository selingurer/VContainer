
using UnityEngine;

public interface ITargetable<T> where T : class
{
    void TakeDamage(float damage);
    float GetAttackValue();

    Vector3 GetTargetPos();
    T GetTarget();



}