using System;

public class Healt : IHealt
{
    private float _healt;
    private float _firstHealt;
    public Action<float, BaseCharacter> OnHealthChanged { get; set; }
    float IHealt.FirstHealt { get => _firstHealt; set
        {
            _firstHealt = value;
            _healt = value;
        }
    }

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
