using UnityEngine;
using TMPro;

public class DisplayPlayerCurrency : SingletonMonobehaviour<DisplayPlayerCurrency>
{
    [SerializeField] private TextMeshProUGUI tempCurrencyText;
    //[SerializeField] private TextMeshProUGUI permCurrencyText;
    [SerializeField] private GameObject silverKeyIcon;
    [SerializeField] private GameObject goldKeyIcon;

    //===========================================================================
    public void UpdateTempCurrencyText(int amount)
    {
        tempCurrencyText.SetText(amount.ToString());
    }

    //public void UpdatePermCurrencyText(int amount)
    //{
    //    permCurrencyText.SetText(amount.ToString());
    //}

    public void UpdateSilverKeyIcon(bool hasSilverKey)
    {
        if (hasSilverKey) 
        { 
            silverKeyIcon.SetActive(true); 
        }
        else
        {
            silverKeyIcon.SetActive(false);

        }
    }
    public void UpdateGoldKeyIcon(bool hasGoldKey)
    {
        if (hasGoldKey)
        {
            goldKeyIcon.SetActive(true);
        }
        else
        {
            goldKeyIcon.SetActive(false);
        }
    }
}
