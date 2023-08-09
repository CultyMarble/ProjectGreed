using UnityEngine;

public class PrefabUnlockAbilityItem : MonoBehaviour
{
    [Header("Loading Data")]
    [SerializeField] private SOBool canLoad;

    [Header("Item Function")]
    [SerializeField] private bool unlockAoEAbility;
    [SerializeField] private bool unlockBombAbility;

    private bool canPickedUp;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(true);
            canPickedUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(false);
            canPickedUp = false;
        }
    }

    //===========================================================================
    private void Update()
    {
        if (canPickedUp == false)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (unlockAoEAbility)
            {
                FindObjectOfType<Player>().GetComponentInChildren<AreaAbility>().canUseAbility = true;
            }
            else if (unlockBombAbility)
            {
                FindObjectOfType<Player>().GetComponentInChildren<BombAbility>().canUseAbility = true;
            }

            canLoad.value = false;

            Player.Instance.SetInteractPromtTextActive(false);

            Invoke(nameof(Destroy), 0.1f);
        }
    }
    private void Destroy()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}