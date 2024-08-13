using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[Serializable]
public struct GameOverObj
{
    public Text txtLevel;
    public Text txtTotalExperience;
    public CanvasGroup objGameOverPanel;
}
public class GameOverData
{
    public int level;
    public int totalExperience;
}

public class GameUIPanel : MonoBehaviour
{
    [SerializeField] private GameOverObj _gameOverObj;
    [SerializeField] private Slider sliderExperience;
    [SerializeField] private Text txtLevel;
    [SerializeField] private Button btnContinue;
    [SerializeField] private Slider sliderHeart;
    [SerializeField] private Transform contentSkill;
    [SerializeField] SkillCardUI _objectSkillCard;

    private SceneService _sceneService;
    private List<SkillCardUI> _skillList = new List<SkillCardUI>();
    [Inject]
    private void Construct(SceneService sceneService)
    {
        _sceneService = sceneService;
    }
    private void Awake()
    {
        btnContinue.onClick.AddListener(BtnContinueClicked);
    }
    private void OnDestroy()
    {
        btnContinue.onClick.RemoveListener(BtnContinueClicked);
    }
    private void BtnContinueClicked()
    {
        _sceneService.LoadScene(SceneType.MainScene);
    }
    public async UniTask SliderHeartValueChanged(int heartValue, int maxValue)
    {
        sliderHeart.maxValue = maxValue;
        sliderHeart.DOValue(heartValue, 0.5f).SetEase(Ease.Linear).SetUpdate(true);

    }
    public async UniTask ExperienceValueChanged(int exValue, int maxValue)
    {
        sliderExperience.maxValue = maxValue;
        sliderExperience.DOValue(exValue, 0.5f).SetEase(Ease.Linear);
    }
    public async UniTask LevelChanged(int level)
    {
        txtLevel.text = "Level" + " " + level;
        txtLevel.gameObject.transform.DOScale(1, 0.5f).SetUpdate(true);
        Time.timeScale = 0f;
        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true);
        txtLevel.gameObject.transform.DOScale(0, 0.5f).SetUpdate(true);
    }
    public void GameOver(GameOverData data)
    {
        Time.timeScale = 0f;
        _gameOverObj.objGameOverPanel.interactable = true;
        _gameOverObj.objGameOverPanel.DOFade(1, 0.5f).SetUpdate(true);
        _gameOverObj.txtLevel.text = data.level.ToString();
        _gameOverObj.txtTotalExperience.text = data.totalExperience.ToString();
    }
    public void CreateSkill(List<SkillData> skillList)
    {
        contentSkill.parent.gameObject.SetActive(true);
        foreach (var skill in skillList)
        {
            var objectSkill = Instantiate(_objectSkillCard, contentSkill);
            objectSkill.SetSkillData(skill);
            _skillList.Add(objectSkill);
        }
    }
    public void ClearSkill()
    {
        foreach (var objSkill in _skillList)
        {
            Destroy(objSkill.gameObject);

        }
        _skillList.Clear();
    }

}
