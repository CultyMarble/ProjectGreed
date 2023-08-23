using UnityEngine;
using TMPro;

public class RunInfoController : SingletonMonobehaviour<RunInfoController>
{
    [SerializeField] private Transform itemInfoPanel = default;
    [SerializeField] private TextMeshProUGUI itemNameText = default;
    [SerializeField] private TextMeshProUGUI itemDescriptionText = default;

    //===========================================================================
    public void SetItemInfoPanelActive(bool newBool)
    {
        itemInfoPanel.gameObject.SetActive(newBool);
    }

    public void SetItemNameText(string newString)
    {
        itemNameText.SetText(newString);
    }

    public void SetItemDescriptionText(string newString)
    {
        itemDescriptionText.SetText(newString);
    }
}