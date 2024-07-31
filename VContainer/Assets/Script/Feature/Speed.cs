using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : ISpeed
{
    public float _speed = 0.05f;

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float speed)
    {
       _speed = speed;
    }

}
