using UnityEngine;

public class UpgradeStatue : MonoBehaviour
{
    private UpgradeMenu upgradeMenu = default;

    private bool canOpenUpgradeMenu = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canOpenUpgradeMenu = true;
        Player.Instance.SetInteractPromtTextActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canOpenUpgradeMenu = false;
        Player.Instance.SetInteractPromtTextActive(false);
    }

    //===========================================================================
    private void OnEnable()
    {
        upgradeMenu = GameObject.Find("UpgradeMenu").GetComponent<UpgradeMenu>();

        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger += UpgradeStatue_OnPlayerInteractTrigger;
    }

    //===========================================================================
    private void UpgradeStatue_OnPlayerInteractTrigger(object sender, System.EventArgs e)
    {
        if (canOpenUpgradeMenu == false)
            return;

        upgradeMenu.SetContentActive(true);
        SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
    }
}