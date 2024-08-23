using System;
using UnityEngine;
using VContainer;

public class PlayerView : MonoBehaviour, ITargetable<PlayerView>
{
    private IInputProvider _inputProvider;
    private Rigidbody2D _rigidbody;
    public Action<float> TakeDamage;
    private PlayerData _playerData;
    private ITargetable<EnemyView> _target;

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
        var component = collision.GetComponent<Component>();

        // Sonra bu bileþeni ITargetable arayüzüne dönüþtürmeye çalýþýyoruz
        var targetable = component as ITargetable<Component>;
        if (targetable != null)
        {
            float attackValue = targetable.GetAttackValue();
            TakeDamage?.Invoke(attackValue);
        }
    }

    public float GetAttackValue()
    {
        return _playerData.Attack;
    }

    public Vector3 GetTargetPos()
    {
        return transform.position;
    }

    public Component GetTarget()
    {
        return this;
    }

    void ITargetable<PlayerView>.TakeDamage(float damage)
    {
        throw new NotImplementedException();
    }

    PlayerView ITargetable<PlayerView>.GetTarget()
    {
        throw new NotImplementedException();
    }
}
