
using UnityEngine;

public interface ITargetable
{
    void TakeDamage(float damage);
    float GetAttackValue();

    Vector3 GetTargetPos();

}