using System;
using UnityEngine;
using VContainer;

public class PlayerView : MonoBehaviour, ITargetable
{
    private IInputProvider _inputProvider;
    private Rigidbody2D _rigidbody;
    public Action<float> TakeDamage;
    private PlayerData _playerData;

    [Inject]
    private void Construct(IInputProvider inputProvider, PlayerData playerData)
    {
        _inputProvider = inputProvider;
        _playerData = playerData;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_playerData != null)
        {
            SetMovement();
        }
    }
    private void SetMovement()
    {
        var horizontal = _inputProvider.Horizontal;
        var vertical = _inputProvider.Vertical;
        _rigidbody.velocity = new Vector2(horizontal, vertical) * _playerData.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var targetable = collision.GetComponent<ITargetable>();
        if (targetable != null)
        {
            float attackValue = targetable.GetAttackValue();
            TakeDamage?.Invoke(attackValue);
        }
    }

    void ITargetable.TakeDamage(float damage)
    {
        TakeDamage.Invoke(damage);
    }

    public float GetAttackValue()
    {
        return _playerData.Attack;
    }
}
