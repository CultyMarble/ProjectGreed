using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        unlocked,
        silver,
        gold
    }
    [SerializeField] private SpawnCurrency spawnCurrency = default;
    public ChestType chestType;
    public GameObject[] regularItems;
    public GameObject[] silverItems;
    public GameObject[] goldItems;
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private bool canOpen = false;
    private PlayerInput playerInput;
    //===========================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.SetInteractPromtTextActive(true);
            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.SetInteractPromtTextActive(false);
            canOpen = false;
        }
    }
    //===========================================================================
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = FindObjectOfType<PlayerInput>();
        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger += Chest_OnPlayerInteractTrigger;
    }

    //===========================================================================

    private void OpenChest()
    {
        int randomIndex;
        switch (chestType)
        {
            case ChestType.unlocked:
                spriteRenderer.sprite = sprites[3];
                randomIndex = Random.Range(0, regularItems.Length);
                Instantiate(regularItems[randomIndex], transform.position, Quaternion.identity);
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;

                AudioManager.Instance.playSFXClip(AudioManager.SFXSound.openLockDoors);

                spawnCurrency.SpewOutCurrency();
                break;
            case ChestType.silver:
                if (PlayerCurrencies.Instance.hasSilverKey)
                {
                    spriteRenderer.sprite = sprites[1];
                    randomIndex = Random.Range(0, silverItems.Length);
                    Instantiate(silverItems[randomIndex], transform.position, Quaternion.identity);
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<CircleCollider2D>().enabled = false;

                    AudioManager.Instance.playSFXClip(AudioManager.SFXSound.openLockDoors);

                    spawnCurrency.SpewOutCurrency();
                }
                else
                {
                    ActivateDialogueManager("Hmm... I think I need to find a specific key to open this.");
                }
                break;
            case ChestType.gold:
                if (PlayerCurrencies.Instance.hasGoldKey)
                {
                    spriteRenderer.sprite = sprites[5];
                    randomIndex = Random.Range(0, goldItems.Length);
                    Instantiate(goldItems[randomIndex], transform.position, Quaternion.identity);
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<CircleCollider2D>().enabled = false;

                    spawnCurrency.SpewOutCurrency();
                }
                else
                {
                    ActivateDialogueManager("Hmm... I think I need to find a specific key to open this.");
                }
                break;
        }
    }
    private void ActivateDialogueManager(string text)
    {
        DialogManager.Instance.SetDialogLine(text);
        DialogManager.Instance.SetDialogPanelActiveState(true);
    }

    private void Chest_OnPlayerInteractTrigger(object sender, System.EventArgs e)
    {
        if (canOpen)
        {
            OpenChest();
        }
    }
}
