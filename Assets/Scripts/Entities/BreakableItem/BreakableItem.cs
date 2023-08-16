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

    private int itemHealth = 10;

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
        foreach(var item in itemAndDropChances)
        {
            if ((Random.value * 100) < item.Chance)
            {
                Instantiate(item.Item).position = transform.position;
            }
        }
    }

}