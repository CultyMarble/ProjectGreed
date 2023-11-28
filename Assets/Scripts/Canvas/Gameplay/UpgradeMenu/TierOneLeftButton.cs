using UnityEngine;
using UnityEngine.EventSystems;

public class TierOneLeftButton : UpgradeMenuButton, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string upgradeName = default;
    [SerializeField] private string upgradeDescription = default;

    //===========================================================================
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpgradeMenu.Instance.SetUpgradeInfoPanelActive(true);
        UpgradeMenu.Instance.SetUpgradeNameText(upgradeName);
        UpgradeMenu.Instance.SetUpgradeDescriptionText(upgradeDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpgradeMenu.Instance.SetUpgradeInfoPanelActive(false);
    }

    //===========================================================================
    public override void AppliedEffect()
    {
        PlayerHeart _playerHeart = Player.Instance.GetComponent<PlayerHeart>();

        _playerHeart.UpdateCurrentMaxHeart(1);
        _playerHeart.ResetPlayerHeart();
    }

    public override void RemoveEffect()
    {
        PlayerHeart _playerHeart = Player.Instance.GetComponent<PlayerHeart>();

        _playerHeart.UpdateCurrentMaxHeart(-1);
        _playerHeart.ResetPlayerHeart();
    }
}