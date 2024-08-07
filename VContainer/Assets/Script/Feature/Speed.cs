using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Speed : ISpeed
{
    public float _speed;
    private float _maxSpeed = 6;
    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        if (speed > _maxSpeed)
        {
            _speed = _maxSpeed;
        }
    }

}
