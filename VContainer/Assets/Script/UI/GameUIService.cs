using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct GameOverObj
{
    public Text txtLevel;
    public Text txtTotalExperience;
    public Text txtGameTime;
    public CanvasGroup objGameOverPanel;
}
public class GameOverData
{
    public int level;
    public int totalExperience;
    public DateTime totalGameTime;
}

public class GameUIService : MonoBehaviour
{
    [SerializeField] private GameOverObj _gameOverObj;
    [SerializeField] private Slider sliderExperience;
    [SerializeField] private Text txtLevel;
    [SerializeField] private Button btnContinue;
    

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
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
    }

    public void ExperienceValueChanged(int exValue,int maxValue)
    {
        sliderExperience.maxValue = maxValue;
        sliderExperience.DOValue(exValue, 0.5f).SetEase(Ease.Linear);
    }
    public async UniTask LevelChanged(int level)
    {
        txtLevel.text = "Level"+" "+ level;
        txtLevel.gameObject.transform.DOScale(1, 0.5f).SetUpdate(true); 
        Time.timeScale = 0f;
        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true);
        Time.timeScale = 1f;
        txtLevel.gameObject.transform.DOScale(0, 0.5f).SetUpdate(true);
    }
    public  void GameOver(GameOverData data)
    {
        Time.timeScale = 0f;
        _gameOverObj.objGameOverPanel.interactable = true;
        _gameOverObj.objGameOverPanel.DOFade(1, 0.5f).SetUpdate(true);
        _gameOverObj.txtLevel.text = data.level.ToString();
      //  _gameOverObj.txtGameTime.text = _gameOverData.totalGameTime.ToString();
        _gameOverObj.txtTotalExperience.text = data.totalExperience.ToString();
    }
    
}
