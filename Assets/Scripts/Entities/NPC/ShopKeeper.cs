using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    [Header("List of Items")]
    [SerializeField] private Transform itemForSale = default;
    [SerializeField] private List<Transform> itemList = default;

    [Header("Item Position List")]
    [SerializeField] private List<Transform> itemPositionList = default;

    private int randomItemIndex = default;
    //===========================================================================
    private void OnEnable()
    {
        GenerateNewItemList();
    }

    //===========================================================================
    private void GenerateNewItemList()
    {
        foreach (Transform position in itemPositionList)
        {
            randomItemIndex = Random.Range(0, itemList.Count);

            Instantiate(itemList[randomItemIndex], itemForSale).transform.position = position.transform.position;
        }
    }
}
