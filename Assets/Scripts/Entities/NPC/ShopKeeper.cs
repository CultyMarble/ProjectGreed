using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    [Header("List of Items")]
    [SerializeField] private List<Transform> itemList = default;

    [Header("Item Position List")]
    [SerializeField] private List<Transform> itemPositionList = default;

    private List<int> listOfItem = new List<int>();
    private int randomItemIndex = default;

    //===========================================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (listOfItem.Count == 0)
            {
                GenerateItemForSale();
            }
            else
            {
                RerollItem();
            }
        }
    }
    private void Awake()
    {
        GenerateItemForSale();
    }

    //===========================================================================
    public void GenerateItemForSale()
    {
        foreach (Transform itemPosition in itemPositionList)
        {
            bool _isIndexNew = true;
            while (_isIndexNew)
            {
                randomItemIndex = UnityEngine.Random.Range(0, itemList.Count);
                if (listOfItem.Contains(randomItemIndex) == false)
                {
                    listOfItem.Add(randomItemIndex);
                    Debug.Log(randomItemIndex.ToString());
                    _isIndexNew = false;
                }
            }

            Transform _item = Instantiate(itemList[randomItemIndex], itemPosition);
            _item.GetComponent<ItemCost>().itemCost = 299;
            _item.transform.position = itemPosition.transform.position;
        }
    }

    public void RerollItem()
    {
        listOfItem.Clear();

        // Delete Old Items
        foreach (Transform itemPosition in itemPositionList)
        {
            if (itemPosition.childCount != 0)
            {
                itemPosition.GetChild(0).gameObject.SetActive(false);
                Destroy(itemPosition.GetChild(0).gameObject);
            }
        }

        GenerateItemForSale();
    }
}