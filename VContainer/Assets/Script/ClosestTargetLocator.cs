using System.Collections.Generic;
using System;
using UnityEngine;

public class ClosestTargetLocator<T> : IClosestTargetLocator<T>
{
    public T GetClosestTarget(Vector3 origin, IEnumerable<T> targets, Func<T, Vector3> positionSelector, float maxDistance = Mathf.Infinity)
    {
        T closestTarget = default;
        float closestDistance = maxDistance;

        foreach (var target in targets)
        {
            Vector3 targetPosition = positionSelector(target);
            float distanceToTarget = Vector3.Distance(targetPosition, origin);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}