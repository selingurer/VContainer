
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btnPlayClicked;

    private void Start()
    {
        btnPlayClicked.onClick.AddListener(PlayBtnClicked);
    }

    private void OnDestroy()
    {
        btnPlayClicked.onClick.RemoveListener(PlayBtnClicked);
    }

    private void PlayBtnClicked()
    {
        
    }
}
