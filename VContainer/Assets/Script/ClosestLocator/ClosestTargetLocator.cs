using System.Collections.Generic;
using System;
using UnityEngine;

public class ClosestTargetLocator<T> : IClosestTargetLocator<T>
{
    public Component GetClosestTarget(Vector3 origin, IEnumerable<Component> targets, Func<Component, Vector3> positionSelector, float maxDistance = Mathf.Infinity)
    {
        Component closestTarget = default;
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

    public Component GetClosestTarget(Vector3 origin, Component target, Vector3 positionSelector, float maxDistance)
    {
        if (target == null)
            return default;

        float distanceToTarget = Vector3.Distance(origin, positionSelector);

        return distanceToTarget < maxDistance ? target : default;
    }
}