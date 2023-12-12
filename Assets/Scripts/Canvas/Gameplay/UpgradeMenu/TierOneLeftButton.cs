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
        Debug.Log("Before Applied!: " + Player.Instance.GetComponent<PlayerHeart>().CurrentMaxHeart);

        PlayerHeart _playerHeart = Player.Instance.GetComponent<PlayerHeart>();

        _playerHeart.UpdateCurrentMaxHeart(1);
        _playerHeart.ResetPlayerHeart();

        Debug.Log("Applied!: " + Player.Instance.GetComponent<PlayerHeart>().CurrentMaxHeart);
    }

    public override void RemoveEffect()
    {
        Debug.Log("Before Remove!: " + Player.Instance.GetComponent<PlayerHeart>().CurrentMaxHeart);

        PlayerHeart _playerHeart = Player.Instance.GetComponent<PlayerHeart>();

        _playerHeart.UpdateCurrentMaxHeart(-1);
        _playerHeart.ResetPlayerHeart();

        Debug.Log("Removed!: " + Player.Instance.GetComponent<PlayerHeart>().CurrentMaxHeart);
    }
}