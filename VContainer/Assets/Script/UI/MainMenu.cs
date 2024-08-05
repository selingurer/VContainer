
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _btnPlayClicked;
    private SceneService _sceneService;
    [Inject]
    private void Construct(SceneService sceneService)
    {
       _sceneService = sceneService;
    }
    private void Start()
    {
        _btnPlayClicked.onClick.AddListener(PlayBtnClicked);
    }

    private void OnDestroy()
    {
        _btnPlayClicked.onClick.RemoveListener(PlayBtnClicked);
    }

    private void PlayBtnClicked()
    {
        _sceneService.LoadScene(SceneType.GameScene);
    }
}
