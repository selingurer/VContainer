
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
public enum SceneType
{
    MainScene = 0,
    GameScene = 1,
}

public class SceneService
{
    public void LoadScene(SceneType sceneType)
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync((int)sceneType);
    }

}
