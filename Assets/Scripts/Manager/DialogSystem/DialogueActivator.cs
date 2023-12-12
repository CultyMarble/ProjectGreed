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
    [SerializeField] private GameObject newDialogueIndicator;

    private int dialogueEntryIndex = default;
    private bool canActivateDialogBox = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (activateType == DialogueActivateType.ManualTrigger)
            Player.Instance.SetInteractPromtTextActive(true);

        canActivateDialogBox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (activateType == DialogueActivateType.ManualTrigger)
            Player.Instance.SetInteractPromtTextActive(false);
        
        canActivateDialogBox = false;
    }

    //===========================================================================
    private void Start()
    {
        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger += ManualTriggerDialogHandler;
        if (dialogueEntryArray.Length == 0)
        {
            return;
        }
        dialogueEntryIndex = Random.Range(0, dialogueEntryArray.Length);
        if (newDialogueIndicator == null)
        {
            return;
        }
        if (dialogueEntryArray[dialogueEntryIndex].hasBeenUsed)
        {
            newDialogueIndicator.SetActive(false);
        }
        else
        {
            newDialogueIndicator.SetActive(true);
        }
    }
    private void Update()
    {
        if (!DialogManager.Instance.activated)
        {
            return;
        }

        if (canActivateDialogBox == false)
            return;
        if(activateType == DialogueActivateType.AutoTrigger)
        {
            AutoTriggerDialogHandler();
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
    private void ActivateDialogueManager(SODialogueTutorial entry)
    {
        entry.UpdateDisplayText();
        DialogManager.Instance.SetDialogLines(entry.dialogueLines);
        DialogManager.Instance.SetDialogPanelActiveState(true);
    }
    private void ManualTriggerDialogHandler(object sender, System.EventArgs e)
    {
        if (canActivateDialogBox && activateType != DialogueActivateType.AutoTrigger)
        {
            Player.Instance.SetInteractPromtTextActive(false);

            if (quickText.Length != 0)
            {
                ActivateDialogueManager(quickText);
                return;
            }
            ActivateDialogueManager(dialogueEntryArray[dialogueEntryIndex]);
            if(newDialogueIndicator != null)
            {
                newDialogueIndicator.SetActive(false);
            }
            dialogueEntryArray[dialogueEntryIndex].hasBeenUsed = true;
            haveActivated = true;
        }
        canActivateDialogBox = false;
    }

    private void AutoTriggerDialogHandler()
    {
        if (haveActivated || SceneControlManager.Instance.IsLoadingScene)
            return;

        haveActivated = true;

        if (quickText.Length != 0)
        {
            ActivateDialogueManager(quickText);
            return;
        }
        ActivateDialogueManager(dialogueEntryArray[dialogueEntryIndex]);
        if (newDialogueIndicator != null)
        {
            newDialogueIndicator.SetActive(false);
        }
        dialogueEntryArray[dialogueEntryIndex].hasBeenUsed = true;
        haveActivated = true;
        canActivateDialogBox = false;
    }
}