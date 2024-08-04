using System;

public class Healt : IHealt
{
    
    private float _healt;

    public Action<float, BaseCharacter> OnHealthChanged { get; set; }

    public float GetHealt()
    {
        return _healt;
    }

    public void SetHealt(float healt, BaseCharacter character)
    {
        _healt = healt;
        OnHealthChanged?.Invoke(healt, character);
    }

}
