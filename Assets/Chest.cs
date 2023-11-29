using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        unlocked,
        silver,
        gold
    }
    public ChestType chestType;
    public GameObject[] regularItems;
    public GameObject[] silverItems;
    public GameObject[] goldItems;

    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private bool canOpen = false;
    //private bool opened = false;
    [SerializeField] private SpawnCurrency spawnCurrency = default;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canOpen)
        {
            OpenChest();
        }
    }
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
}
