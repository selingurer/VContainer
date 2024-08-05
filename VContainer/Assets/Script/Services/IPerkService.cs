
using Assets.Script.Feature.Rewards;
using System.Collections.Generic;

public interface IPerkService
{
    public List<Rewards> _rewardList { get; set; }
    public List<Rewards> GetRewardList();

    public void SelectedReward(RewardTypes types, Rewards reward);
}
