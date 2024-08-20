using System.Collections.Generic;
using System;
using UnityEngine;

public interface IClosestTargetLocator<T>
{
    T GetClosestTarget(Vector3 origin, IEnumerable<T> targets, Func<T, Vector3> positionSelector, float maxDistance = Mathf.Infinity);
}