using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointList : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int initSpawnAmount;

    private int currentSpawnAmount;
    void Awake()
    {
        currentSpawnAmount = initSpawnAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetSpawnAmount()
    {
        return currentSpawnAmount;
    }
    public void SetSpawnAmount(int amount)
    {
        currentSpawnAmount = amount;
    }
}
