
public class TierOneLeftButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        PlayerHeart _playerHeart = Player.Instance.GetComponent<PlayerHeart>();

        _playerHeart.UpdateCurrentMaxHeart(1);
        _playerHeart.ResetPlayerHeart();

        //PlayerDataManager.Instance.PlayerDataRuntime.SetBaseMaxHealth(_playerHeart.CurrentMaxHeart);
        //PlayerDataManager.Instance.SaveRunTimeDataToPlayerDataSlot();
    }

    public override void RemoveEffect()
    {
        PlayerHeart _playerHeart = Player.Instance.GetComponent<PlayerHeart>();

        _playerHeart.UpdateCurrentMaxHeart(-1);
        _playerHeart.ResetPlayerHeart();

        //PlayerDataManager.Instance.PlayerDataRuntime.SetBaseMaxHealth(_playerHeart.CurrentMaxHeart);
        //PlayerDataManager.Instance.SaveRunTimeDataToPlayerDataSlot();
    }
}