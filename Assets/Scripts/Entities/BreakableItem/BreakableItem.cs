using UnityEngine;

[System.Serializable]
public class ItemAndDropChance
{
    [SerializeField] private Transform item = default;
    [SerializeField] private float chance = default;

    public Transform Item => item;
    public float Chance => chance;
}

public class BreakableItem : MonoBehaviour
{
    [SerializeField] ItemAndDropChance[] itemAndDropChances;
    [SerializeField] private SpawnCurrency spawnCurrency = default;

    private float itemHealth = 10;

    //===========================================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Force Damage Item");
            itemHealth--;
        }

        if (itemHealth < 0)
        {
            SpawnItem();
            AudioManager.Instance.playSFXClip(AudioManager.SFXSound.wood);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    //===========================================================================
    private void SpawnItem()
    {
        int totalItems = itemAndDropChances.Length;
        bool spawned = false;
        var _value = Random.Range(0, 2);
        if (_value == 1)
        {
            for(int i = 0; i < totalItems; i++ )
            {
                int randomIndex = Random.Range(0, totalItems);
                if ((Random.value * 100) < itemAndDropChances[randomIndex].Chance)
                {
                    Instantiate(itemAndDropChances[randomIndex].Item).position = transform.position;
                    spawned = true;
                    break;
                }
            }
        }
        if(_value == 0 || !spawned)
        {
            spawnCurrency.SpewOutCurrency();
        }
    }

    public void UpdateCurrentHealth(float amount)
    {
        itemHealth += amount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { 
            if(collision.gameObject.GetComponent<Player>().actionState == PlayerActionState.IsDashing)
            {
                SpawnItem();

                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }



}