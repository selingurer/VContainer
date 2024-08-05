
using Assets.Script.Feature.Rewards;

public class SpeedPlayer : Rewards
{
    private void Awake()
    {
        _rewardTypes = RewardTypes.SpeedPlayer;
    }
    protected override void SetRewardsGame()
    {
        base.SetRewardsGame();
        
        _player._speed.SetSpeed(_player._speed.GetSpeed() * 2);
    }

}
