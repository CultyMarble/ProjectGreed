using UnityEngine;
using UnityEngine.EventSystems;

public class TierOneMiddleButton : UpgradeMenuButton, IPointerEnterHandler, IPointerExitHandler
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
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(25);
    }

    public override void RemoveEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(-25);
    }
}