using System;
using UnityEngine;
using VContainer;

interface IPlayerData
{
    public Vector3 GetPosition();
    public Component GetComponent();
    public ITargetable GetTargetable();
}

public class PlayerView : MonoBehaviour, ITargetable, IPlayerData
{
    public Action<float> TakeDamage;
    private IInputProvider _inputProvider;
    private Rigidbody2D _rigidbody;
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
        if (collision.TryGetComponent(out ITargetable target))
        {
            float attackValue = target.GetAttackValue();
            TakeDamage?.Invoke(attackValue);
        }
    }
    public float GetAttackValue()
    {
        return _playerData.Attack;
    }
    void ITargetable.TakeDamage(float damage)
    {

    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public Component GetComponent()
    {
        return this;
    }
    public ITargetable GetTargetable()
    {
        return this;
    }
}
