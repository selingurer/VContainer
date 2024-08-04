using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIService : MonoBehaviour
{
    [SerializeField] private Slider sliderExperience;

    public void ExperienceValueChanged(int exValue,int maxValue)
    {
        sliderExperience.maxValue = maxValue;
        sliderExperience.DOValue(exValue, 0.5f).SetEase(Ease.Linear);
    }
}
