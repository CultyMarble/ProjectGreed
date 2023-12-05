using UnityEngine;
using UnityEngine.UI;

public class DisplayItemUpgradeIcon : SingletonMonobehaviour<DisplayItemUpgradeIcon>
{
    //===========================================================================
    public void AddItemUpdateIcon(Sprite newIcon, int newID)
    {
        foreach (Transform _icon in transform)
        {
            PrefabItemDisplayIcon _displayIcon = _icon.GetComponent<PrefabItemDisplayIcon>();

            if (_icon.gameObject.activeInHierarchy && _displayIcon.iconID == newID)
            {
                _displayIcon.UpdateAmount();
                return;
            }

            if (_displayIcon.iconID == 0) // newIcon
            {
                _icon.gameObject.SetActive(true);
                _displayIcon.iconID = newID;
                _displayIcon.itemDisplayIcon.sprite = newIcon;
                _displayIcon.UpdateAmount();
                return;
            }
        }
    }

    //===========================================================================
    public void Clear()
    {
        foreach (Transform _icon in transform)
        {
            PrefabItemDisplayIcon _displayIcon = _icon.GetComponent<PrefabItemDisplayIcon>();
            _displayIcon.iconID = 0;
            _displayIcon.itemDisplayIcon.sprite = null;
            _displayIcon.ResetAmount();

            _icon.gameObject.SetActive(false);
        }
    }
}
