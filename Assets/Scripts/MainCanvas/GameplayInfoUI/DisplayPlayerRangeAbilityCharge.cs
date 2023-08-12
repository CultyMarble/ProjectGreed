using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerRangeAbilityCharge : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private RangeAbility rangeAbility = default;

    [Header("Pooling Settings:")]
    [Header("Empty Charge:")]
    [SerializeField] private Transform pfEmptyCharge = default;
    [SerializeField] private Transform emptyChargePool = default;
    [Header("Full Charge:")]
    [SerializeField] private Transform pfFullCharge = default;
    [SerializeField] private Transform fullChargePool = default;

    private readonly int poolSize = 20;
    private readonly float offsetX = 17.0f;
    private readonly float offsetY = -27.0f;

    //===========================================================================
    private void Awake()
    {
        PopulatePool(pfFullCharge, fullChargePool);
        PopulatePool(pfEmptyCharge, emptyChargePool);
    }

    private void OnEnable()
    {
        rangeAbility.OnMaxChargeChangedEvent += UpdateEmptyChargeUI;
        rangeAbility.OnCurrentChargeChangedEvent += UpdateFullChargeUI;
    }

    private void OnDisable()
    {
        rangeAbility.OnMaxChargeChangedEvent -= UpdateEmptyChargeUI;
        rangeAbility.OnCurrentChargeChangedEvent -= UpdateFullChargeUI;
    }

    //===========================================================================
    private void UpdateFullChargeUI(object sender, RangeAbility.OnCurrentChargeChangedEventArgs e)
    {
        foreach (Transform fullCharge in fullChargePool)
        {
            fullCharge.gameObject.SetActive(false);
        }

        for (int i = 0; i < e.currentCharge; i++)
        {
            if (fullChargePool.GetChild(i).gameObject.activeInHierarchy == false)
            {
                fullChargePool.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(i * offsetX, offsetY, 0);
                fullChargePool.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void UpdateEmptyChargeUI(object sender, RangeAbility.OnMaxChargeChangedEventArgs e)
    {
        foreach (Transform emptyCharge in emptyChargePool)
        {
            emptyCharge.gameObject.SetActive(false);
        }

        for (int i = 0; i < e.maxCharge; i++)
        {
            if (emptyChargePool.GetChild(i).gameObject.activeInHierarchy == false)
            {
                emptyChargePool.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(i * offsetX, offsetY, 0);
                emptyChargePool.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    //===========================================================================
    private void PopulatePool(Transform pfObject, Transform pool)
    {
        for (int i = 0; i < poolSize; i++)
        {
            Instantiate(pfObject, pool).gameObject.SetActive(false);
        }
    }
}
