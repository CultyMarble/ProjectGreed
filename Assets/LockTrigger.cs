using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private GateManager gateManager;
    private ToolTip toolTipMenu;

    private void Awake()
    {
        gateManager = transform.parent.parent.GetComponent<GateManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && gateManager.playerInLockZone == true)
        {
            switch (gateManager.keytype)
            {
                case PlayerCurrencies.KeyType.Silver:
                    if (PlayerCurrencies.Instance.hasSilverKey)
                    {
                        gateManager.disableGate = true;
                        gateManager.locked = false;
                        if(toolTipMenu != null)
                        {
                            toolTipMenu.ClearToolTip();
                        }
                        Player.Instance.SetInteractPromtTextActive(false);
                    }
                    else
                    {
                        ActivateDialogueManager("Hmm... I think I need to find a specific key to open this.");
                    }
                    break;
                case PlayerCurrencies.KeyType.Gold:
                    if (PlayerCurrencies.Instance.hasGoldKey)
                    {
                        gateManager.disableGate = true;
                        gateManager.locked = false;
                        if (toolTipMenu != null)
                        {
                            toolTipMenu.ClearToolTip();
                        }
                        Player.Instance.SetInteractPromtTextActive(false);
                    }
                    else
                    {
                        ActivateDialogueManager("Hmm... I think I need to find a specific key to open this.");
                    }
                    break;
            }
        }
    }
    private void ActivateDialogueManager(string text)
    {
        DialogManager.Instance.SetDialogLine(text);
        DialogManager.Instance.SetDialogPanelActiveState(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gateManager.locked)
        {
            switch (gateManager.keytype)
            {
                case PlayerCurrencies.KeyType.Silver:
                    toolTipMenu = new ToolTip("Silver Locked Gate", "All rusted. I'll have to find some way to unlock this.");
                    toolTipMenu.SetToolTip();
                    break;
                case PlayerCurrencies.KeyType.Gold:
                    toolTipMenu = new ToolTip("Golden Locked Gate", "Quite magestic. Must be a key somewhere.");
                    toolTipMenu.SetToolTip();
                    break;
            }
            Player.Instance.SetInteractPromtTextActive(true);
            gateManager.playerInLockZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(toolTipMenu == null)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            toolTipMenu.ClearToolTip();
            Player.Instance.SetInteractPromtTextActive(false);
            gateManager.playerInLockZone = false;
        }
    }

}
