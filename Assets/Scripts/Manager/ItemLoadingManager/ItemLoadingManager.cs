using System.Collections.Generic;
using UnityEngine;

public class ItemLoadingManager : MonoBehaviour
{
    public List<ItemLoadData> itemLoadingDataList;

    //===========================================================================
    private void OnEnable()
    {
        foreach (ItemLoadData itemLoad in itemLoadingDataList)
        {
            if (itemLoad.canLoad.value == true)
            {
                itemLoad.item.gameObject.SetActive(true);
            }
            else
            {
                itemLoad.item.gameObject.SetActive(false);
            }
        }
    }
}