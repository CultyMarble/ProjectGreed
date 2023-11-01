using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private GateManager gateManager;
    private void Awake()
    {
        gateManager = transform.parent.GetComponent<GateManager>();
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
                        Player.Instance.SetInteractPromtTextActive(false);
                    }
                    break;
                case PlayerCurrencies.KeyType.Gold:
                    if (PlayerCurrencies.Instance.hasGoldKey)
                    {
                        gateManager.disableGate = true;
                        Player.Instance.SetInteractPromtTextActive(false);
                    }
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D && gateManager.locked)
        {
            Player.Instance.SetInteractPromtTextActive(true);
            gateManager.playerInLockZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D && gateManager.locked)
        {
            Player.Instance.SetInteractPromtTextActive(false);
            gateManager.playerInLockZone = false;
        }
    }

}
