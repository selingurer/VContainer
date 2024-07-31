using DG.Tweening;
using UnityEngine;
using VContainer;


public class Player : MonoBehaviour
{
    public Speed _speed;
    [SerializeField] private FixedJoystick _joystick;
    private Rigidbody2D _rigidbody;
    float _horizontal;
    float _vertical;

    [Inject]
    private void Construct(Speed speed)
    {
        _speed = speed;
    }

    void Start()
    {
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
