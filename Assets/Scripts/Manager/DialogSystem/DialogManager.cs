using UnityEngine;
using TMPro;

public class DialogManager : SingletonMonobehaviour<DialogManager>
{
    [SerializeField] private Transform dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;

    private string[] dialogLines;
    private int lineIndex;
    public bool activated = false;

    //===========================================================================
    private void Start()
    {
        //SetDialogPanelActiveState(false);
        //Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (!activated)
        {
            return;
        }
        if (dialogPanel.gameObject.activeSelf == false)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            lineIndex++;
            if(dialogLines == null)
            {
                lineIndex = 0;

                SetDialogPanelActiveState(false);

                return;
            }
            if (lineIndex >= dialogLines.Length)
            {
                lineIndex = 0;

                SetDialogPanelActiveState(false);

                return;
            }

            dialogText.SetText(dialogLines[lineIndex]);
        }
    }

    //===========================================================================
    public void SetDialogPanelActiveState(bool newBool)
    {
        if (newBool == true)
            SceneControlManager.Instance.GameState = GameState.Dialogue;
        else
            SceneControlManager.Instance.GameState = GameState.Dungeon;

        dialogPanel.gameObject.SetActive(newBool);
    }

    public void SetDialogLines(string[] newDialogLines)
    {
        dialogLines = newDialogLines;

        dialogText.SetText(dialogLines[lineIndex]);
    }
    public void SetDialogLine(string newDialogLines)
    {
        dialogText.SetText(newDialogLines);
    }
}
