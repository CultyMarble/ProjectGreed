using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class DialogManager : SingletonMonobehaviour<DialogManager>
{
    [SerializeField] private Transform dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image portraitSprite;
    [SerializeField] private Sprite playerPortrait;

    private Tuple<string,bool>[] dialogLines;
    private string quickText;
    private int currentLine;
    private Sprite npcPortrait;

    //===========================================================================
    private void Start()
    {
        SetDialogPanelActiveState(false);
    }

    private void Update()
    {
        if (dialogPanel.gameObject.activeSelf == false)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            currentLine++;
            
            if (currentLine >= dialogLines.Length)
            {
                currentLine = 0;

                SetDialogPanelActiveState(false);
                Time.timeScale = 1.0f;

                return;
            }
            if (dialogLines[currentLine].Item2 == true)
            {
                portraitSprite.sprite = playerPortrait;
            }
            else
            {
                portraitSprite.sprite = npcPortrait;
            }
            dialogText.SetText(dialogLines[currentLine].Item1);
        }
    }

    //===========================================================================
    public void SetDialogPanelActiveState(bool newBool)
    {
        dialogPanel.gameObject.SetActive(newBool);
    }

    //public void SetDialogLines(string[] newDialogLines)
    //{
    //    dialogLines = newDialogLines;
    //    currentLine = 0;
    //    dialogText.SetText(dialogLines[currentLine]);

    //}
    public void SetQuickText(string newDialogLines)
    {
        quickText = newDialogLines;
        currentLine = 0;
        dialogText.SetText(quickText);
    }
    public void SetPortrait(Sprite portrait)
    {
        npcPortrait = portrait;
    }
    public void SetupDialoguePanel(Tuple<string,bool>[] newDialogueLines, Sprite portrait)
    {
        dialogLines = newDialogueLines;
        npcPortrait = portrait;
        currentLine = 0;
        if (dialogLines[currentLine].Item2 == true)
        {
            portraitSprite.sprite = playerPortrait;
        }
        else
        {
            portraitSprite.sprite = npcPortrait;
        }
        dialogText.SetText(dialogLines[currentLine].Item1);
    }
}
