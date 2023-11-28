using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip
{
    public string _name;
    public string _text;

    public ToolTip(string name, string text)
    {
        _name = name;
        _text = text;
    }
    public void SetToolTip()
    {
        RunInfoController.Instance.SetItemInfoPanelActive(true);
        RunInfoController.Instance.SetItemNameText(_name);
        RunInfoController.Instance.SetItemDescriptionText(_text);
    }
    public void ClearToolTip()
    {
        RunInfoController.Instance.SetItemInfoPanelActive(false);
        RunInfoController.Instance.SetItemNameText("");
        RunInfoController.Instance.SetItemDescriptionText("");
    }
}