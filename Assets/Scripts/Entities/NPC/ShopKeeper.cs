using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] private SOListInt generatedItemForSale = default;

    [Header("List of Items")]
    [SerializeField] private List<Transform> itemList = default;

    [Header("Item Position List")]
    [SerializeField] private List<Transform> itemPositionList = default;

    private int randomItemIndex = default;

    //===========================================================================
    private void OnEnable()
    {
        GenerateItemForSale();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            RerollItem();
        }
    }

    //===========================================================================
    private void GenerateItemForSale()
    {
        if (generatedItemForSale.itemList.Count == 0)
        {
            GenerateNewListOfItemForSale();
            return;
        }
        else
        {
            GenerateItemItemForSaleBasedOnSOList();
        }
    }

    private void GenerateNewListOfItemForSale()
    {
        foreach (Transform itemPosition in itemPositionList)
        {
            bool _isIndexNew = true;
            while (_isIndexNew)
            {
                randomItemIndex = UnityEngine.Random.Range(0, itemList.Count);

                if (generatedItemForSale.itemList.Contains(randomItemIndex) == false)
                {
                    generatedItemForSale.itemList.Add(randomItemIndex);
                    _isIndexNew = false;
                }
            }

            Instantiate(itemList[randomItemIndex], itemPosition).transform.position = itemPosition.transform.position;
        }
    }

    private void GenerateItemItemForSaleBasedOnSOList()
    {
        int _itemIndex = default;
        foreach (Transform itemPosition in itemPositionList)
        {
            if (generatedItemForSale.itemList[_itemIndex] >= 0)
            {
                Instantiate(itemList[generatedItemForSale.itemList[_itemIndex]], itemPosition).transform.position =
                    itemPosition.transform.position;

            }

            _itemIndex++;
        }
    }

    private void RerollItem()
    {
        if (PlayerCurrencies.Instance.TempCurrencyAmount == 0)
            return;

        generatedItemForSale.itemList.Clear();

        // Delete Old Items
        foreach (Transform itemPosition in itemPositionList)
        {
            if (itemPosition.childCount != 0)
            {
                itemPosition.GetChild(0).gameObject.SetActive(false);
                Destroy(itemPosition.GetChild(0).gameObject);
            }
        }

        GenerateNewListOfItemForSale();

        PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-1);
    }

    //===========================================================================
    public void SetItemPurchase(int index)
    {
        generatedItemForSale.itemList[index] = -1;
    }
}