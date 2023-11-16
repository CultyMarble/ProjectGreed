using UnityEngine;

public class PrefabPutInHand : MonoBehaviour
{
    [SerializeField] private Transform pfZombie = default;

    private Transform parent = default;

    //===========================================================================
    public void CreateZombie()
    {
        parent = GameObject.Find("Prefabs").transform;

        Instantiate(pfZombie, parent).transform.position = transform.position;
    }

    public void DestroyHand()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}