using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Script.Feature.Rewards
{
    public class Rewards : MonoBehaviour, IRewards
    {
        [SerializeField] private Image _imgIcon;
        [SerializeField] private Text _txtDesc;
        private Button btnRewardSelected;
        private IPerkService _perkService;
        public RewardTypes _rewardTypes;
        public Player _player;
     
     

        public RewardTypes RewardTypes => _rewardTypes;

        [Inject]
        private void Construct(IPerkService perkService, Player player)
        {
            _perkService = perkService;
            _player = player;
        }
        private void Awake()
        {
            btnRewardSelected = GetComponent<Button>();
            btnRewardSelected.onClick.AddListener(OnBtnRewardSelectedCLicked);
        }

        private void OnDisable()
        {
            btnRewardSelected.onClick.RemoveListener(OnBtnRewardSelectedCLicked);
        }
        private void OnBtnRewardSelectedCLicked()
        {
            _perkService.SelectedReward(RewardTypes, this);
        }
        protected virtual void SetRewardsGame()
        {

        }
        public void SetRewardPerk()
        {
            SetRewardsGame();
        }

    }
}