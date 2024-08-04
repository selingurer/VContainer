
using System;

public interface IHealt
{
    Action<float, BaseCharacter> OnHealthChanged {  get; set; }
    void SetHealt(float healt, BaseCharacter character);
    public float GetHealt();
}
