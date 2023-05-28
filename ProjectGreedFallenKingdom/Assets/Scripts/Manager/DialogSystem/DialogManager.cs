using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : SingletonMonobehaviour<DialogManager>
{
    [SerializeField] private Transform dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;

    private string[] dialogLines;
    private int currentLine;

    //===========================================================================
    private void Start()
    {
        SetDialogPanelActiveState(false);
    }

    private void Update()
    {
        if (dialogPanel.gameObject.activeSelf == false)
        {
            Time.timeScale = 1.0f;
            return;
        }

        Time.timeScale = 0.0f;

        if (Input.GetMouseButtonUp(0))
        {
            currentLine++;

            if (currentLine >= dialogLines.Length)
            {
                currentLine = 0;
                SetDialogPanelActiveState(false);
                return;
            }

            dialogText.SetText(dialogLines[currentLine]);
        }
    }

    //===========================================================================
    public void SetDialogPanelActiveState(bool newBool)
    {
        dialogPanel.gameObject.SetActive(newBool);
    }

    public void SetDialogLines(string[] newDialogLines)
    {
        dialogLines = newDialogLines;
        currentLine = 0;
        dialogText.SetText(dialogLines[currentLine]);
    }
}
