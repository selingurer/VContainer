using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private FixedJoystick _joystick;
    private Rigidbody2D _rigidbody;
    float _horizontal;
    float _vertical;
    public Action<Enemy> TakeDamage;
    private float _speed;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GetMovementInput();
        SetMovement();
    }
    private void SetMovement()
    {
        _rigidbody.velocity = new Vector2(_horizontal, _vertical) * _speed;
    }

    private void GetMovementInput()
    {
        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {

            TakeDamage?.Invoke(collision.GetComponent<Enemy>());
        }
    }
}
