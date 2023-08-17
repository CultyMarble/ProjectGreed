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

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    //===========================================================================
    private void SpawnItem()
    {
        var _value = Random.Range(0, 2);
        if (_value == 1)
        {
            foreach (var item in itemAndDropChances)
            {
                if ((Random.value * 100) < item.Chance)
                {
                    Instantiate(item.Item).position = transform.position;
                }
            }
        }
        else
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