using System.Collections.Generic;
using System;
using UnityEngine;

public interface IClosestTargetLocator<T>
{
    Component GetClosestTarget(Vector3 origin, IEnumerable<Component> targets, Func<Component, Vector3> positionSelector, float maxDistance = Mathf.Infinity);
    Component GetClosestTarget(Vector3 origin, Component target, Vector3 positionSelector, float maxDistance);
}