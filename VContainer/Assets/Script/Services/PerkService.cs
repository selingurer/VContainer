
using Assets.Script.Feature.Rewards;
using System.Collections.Generic;
public enum RewardTypes
{
    SpeedPlayer = 0,
}
public class PerkService : IPerkService
{
    public List<Rewards> _rewardList { get; set; }

    public List<Rewards> GetRewardList()
    {
       return _rewardList;
    }

    public void SelectedReward(RewardTypes types,Rewards reward)
    {
        reward.SetRewardPerk();
    }

}
