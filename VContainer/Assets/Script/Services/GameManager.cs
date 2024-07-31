using UnityEngine;
using VContainer;


public class GameManager : MonoBehaviour
{
    public Player enemyPrefab;
    private ILevelService _levelService;
    private ObjectPool<Player> enemyPool;
    [Inject]
    private void Construct(ILevelService service)
    {
        _levelService = service;
    }

    private void Start()
    {
      
    }
}

