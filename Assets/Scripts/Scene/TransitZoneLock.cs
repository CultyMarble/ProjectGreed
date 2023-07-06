using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitZoneLock : MonoBehaviour
{
    [SerializeField] private Transform enemies;
    [SerializeField] private GameObject transitPoint;

    private void Update()
    {
        if (enemies.GetChild(0).gameObject.activeSelf == false)
        {
            transitPoint.SetActive(true);
        }
        else
        {
            transitPoint.SetActive(false);
        }
    }
}
