public class Attack : IAttack
{
    private float _attack;
    public float GetAttack()
    {
       return _attack;
    }

    public void SetAttack(float attackValue)
    {
     _attack = attackValue;
    }
}
