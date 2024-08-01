using DG.Tweening;
using System;
using UnityEngine;
using VContainer;
using VContainer.Diagnostics;


public class Player : MonoBehaviour
{
    [SerializeField] private FixedJoystick _joystick;
    public ISpeed _speed;
    public IHealt _healt;
    public IAttack _attack;
    private Rigidbody2D _rigidbody;
    float _horizontal;
    float _vertical;

 
    [Inject]
    private void Construct(ISpeed speed, IHealt healt,IAttack attack)
    {
        _speed = speed;
        _healt = healt;
        _attack = attack;
    }

    void Start()
    {
        _speed.SetSpeed(0.05f);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetMovementInput();
        SetMovement();
    }
    private void SetMovement()
    {
        _rigidbody.DOMove(_rigidbody.position + new Vector2(_horizontal, _vertical), _speed.GetSpeed()).SetEase(Ease.Linear);
    }
    private void GetMovementInput()
    {
        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;
    }

}
