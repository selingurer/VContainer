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
    [SerializeField] private Slider _sliderExperience;
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Button _btnContinue;
    [SerializeField] private Slider _sliderHeart;
    [SerializeField] private Transform _contentSkill;
    [SerializeField] SkillCardUI _objectSkillCard;

    private SceneService _sceneService;
    private List<SkillCardUI> _skillCardList = new List<SkillCardUI>();
    [Inject]
    private void Construct(SceneService sceneService)
    {
        _sceneService = sceneService;
    }
    private void Awake()
    {
        _btnContinue.onClick.AddListener(BtnContinueClicked);
    }
    private void OnDestroy()
    {
        _btnContinue.onClick.RemoveListener(BtnContinueClicked);
    }
    private void BtnContinueClicked()
    {
        _sceneService.LoadScene(SceneType.MainScene);
    }
    public async UniTask SliderHeartValueChanged(int heartValue, int maxValue)
    {
        _sliderHeart.maxValue = maxValue;
        _sliderHeart.DOValue(heartValue, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
    }
    public async UniTask ExperienceValueChanged(int exValue, int maxValue)
    {
        _sliderExperience.maxValue = maxValue;
        _sliderExperience.DOValue(exValue, 0.5f).SetEase(Ease.Linear);
    }
    public async UniTask LevelChanged(int level)
    {
        Time.timeScale = 0f;
        _txtLevel.text = "Level" + " " + level;
        _txtLevel.gameObject.transform.DOScale(1, 0.5f).SetUpdate(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true);
        await _txtLevel.gameObject.transform.DOScale(0, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
    }
    public void GameOver(GameOverData data)
    {
        Time.timeScale = 0f;
        _gameOverObj.objGameOverPanel.interactable = true;
        _gameOverObj.objGameOverPanel.DOFade(1, 0.5f).SetUpdate(true);
        _gameOverObj.txtLevel.text = data.level.ToString();
        _gameOverObj.txtTotalExperience.text = data.totalExperience.ToString();
    }
    public void CreateSkillCard(List<SkillData> skillList)
    {
        _contentSkill.parent.gameObject.SetActive(true);
        foreach (var skill in skillList)
        {
            var objectSkill = Instantiate(_objectSkillCard, _contentSkill);
            objectSkill.SetSkillData(skill);
            _skillCardList.Add(objectSkill);
        }
    }
    public void ClearSkillCard()
    {
        foreach (var objSkill in _skillCardList)
        {
            Destroy(objSkill.gameObject);

        }
        _skillCardList.Clear();
    }

}
