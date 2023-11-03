using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    private enum DialogueActivateType { AutoTrigger, ManualTrigger, }

    [Header("Set Dialogue Trigger Method:")]
    [SerializeField] private DialogueActivateType activateType = default;
    //[SerializeField] private SOBool haveActivated = default;
    [SerializeField] private bool haveActivated = default;


    [Header("Dialog Entry Data:")]
    [SerializeField] private SODialogueEntry[] dialogueEntryArray = default;
    [SerializeField] private string quickText;

    private int dialogueEntryIndex = default;
    private bool canActivateDialogBox = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        if (activateType == DialogueActivateType.ManualTrigger)
            Player.Instance.SetInteractPromtTextActive(true);

        canActivateDialogBox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        if (activateType == DialogueActivateType.ManualTrigger)
            Player.Instance.SetInteractPromtTextActive(false);
        
        canActivateDialogBox = false;
    }

    //===========================================================================
    private void Update()
    {
        //if (SceneControlManager.Instance.IsLoadingScene == true)
        //    return;

        if (canActivateDialogBox == false)
            return;

        switch (activateType)
        {
            case DialogueActivateType.ManualTrigger:
                ManualTriggerDialogHandler();
                break;
            case DialogueActivateType.AutoTrigger:
                AutoTriggerDialogHandler();
                break;
        }
    }

    private void OnDisable()
    {
        //haveActivated.value = false;
    }

    //===========================================================================
    private void ActivateDialogueManager(SODialogueEntry entry)
    {
        DialogManager.Instance.SetDialogLines(entry.dialogueLines);
        DialogManager.Instance.SetDialogPanelActiveState(true);
    }
    private void ActivateDialogueManager(string entry)
    {
        DialogManager.Instance.SetDialogLine(entry);
        DialogManager.Instance.SetDialogPanelActiveState(true);
    }
    private void ManualTriggerDialogHandler()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(quickText.Length != 0)
            {
                ActivateDialogueManager(quickText);
                return;
            }
            foreach (SODialogueEntry entry in dialogueEntryArray)
            {
                if (entry.hasBeenUsed == false)
                {
                    entry.hasBeenUsed = true;
                    ActivateDialogueManager(entry);

                    return;
                }
            }

            dialogueEntryIndex = Random.Range(0, dialogueEntryArray.Length);
            ActivateDialogueManager(dialogueEntryArray[dialogueEntryIndex]);
        }
    }

    private void AutoTriggerDialogHandler()
    {
        if (SceneControlManager.Instance.GameState == GameState.PauseMenu ||
            SceneControlManager.Instance.GameState == GameState.OptionMenu ||
            SceneControlManager.Instance.GameState == GameState.Dialogue)
        {
            return;
        }
        if (haveActivated)
            return;

        haveActivated = true;

        if (quickText.Length != 0)
        {
            ActivateDialogueManager(quickText);
            return;
        }
        dialogueEntryIndex = Random.Range(0, dialogueEntryArray.Length);
        ActivateDialogueManager(dialogueEntryArray[dialogueEntryIndex]);
    }
}