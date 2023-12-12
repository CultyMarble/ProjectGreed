using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    
    public PlayerCurrencies.KeyType keyType;
    private bool canPickup = false;

    private void Start()
    {
        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger += PickupKey;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(true);
            canPickup = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(false);
            canPickup = false;
        }
    }

    private void PickupKey(object sender, System.EventArgs e)
    {
        if (canPickup)
        {
            switch (keyType)
            {
                case PlayerCurrencies.KeyType.Silver:
                    PlayerCurrencies.Instance.AddSilverKey();
                    break;
                case PlayerCurrencies.KeyType.Gold:
                    PlayerCurrencies.Instance.AddGoldKey();
                    break;
            }
            Destroy(this.gameObject);
        }
    }
    private void OnDisable()
    {
        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger -= PickupKey;

    }
}
