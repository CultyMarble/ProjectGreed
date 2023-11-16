using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : SingletonMonobehaviour<ObjectPoolingManager>
{
    [Header("Pool Transform List:")]
    [SerializeField] private List<Transform> poolTranformList;

    [Header("Pool Size List:")]
    [SerializeField] private List<int> poolSizeList;

    [Header("Object Transform List:")]
    [SerializeField] private List<Transform> poolObjectList;

    private int currentPool = default;

    //===========================================================================
    private void OnEnable()
    {
        PopulateObjectPool();
    }

    //===========================================================================
    private void PopulateObjectPool()
    {
        foreach(Transform pool in poolTranformList)
        {
            for (int i = 0; i < poolSizeList[currentPool]; i++)
            {
                Instantiate(poolObjectList[currentPool], pool).gameObject.SetActive(false);
            }

            currentPool++;
        }
    }

    //===========================================================================
    public void SetAllObjectsActive(bool active)
    {
        foreach (Transform pool in poolTranformList)
        {
            foreach (Transform obj in pool)
            {
                obj.gameObject.SetActive(active);
            }
        }
    }
}