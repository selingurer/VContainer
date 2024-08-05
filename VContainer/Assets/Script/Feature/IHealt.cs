
using System;

public interface IHealt
{
    float FirstHealt {  get; set; }
    Action<float, BaseCharacter> OnHealthChanged {  get; set; }
    void SetHealt(float healt, BaseCharacter character);
    public float GetHealt();
}
