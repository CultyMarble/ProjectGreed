using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private string quickText;

    [SerializeField] private bool autoActive;
    [SerializeField] private GameObject dialogueIndicator;

    private DialogueEntry currentDialogueEntry;
    private int dialogueEntryIndex;
    private bool canActivate;
    private bool isActivated;
    private bool canAuto = false;
    private bool previouslyActivated = false;


    private enum DialogueState
    {
        auto,
        manual,
    }
    private DialogueState dialogueState;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Player.Instance.ShowFPromtText();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dialogueState == DialogueState.auto)
            return;

        if (collision.CompareTag("Player"))
            canActivate = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (dialogueState == DialogueState.auto)
            return;

        if (collision.CompareTag("Player"))
        {
            Player.Instance.HideFPromtText();
            canActivate = false;
            isActivated = false;
        }
    }

    //===========================================================================
    private void Start()
    {
        if (dialogueEntries == null)
            return;

        if (autoActive)
        {
            dialogueState = DialogueState.auto;
        }
        else
        {
            dialogueState = DialogueState.manual;
            canAuto = false;
        }

        dialogueEntryIndex = Random.Range(0, dialogueEntries.Length);

        EventManager.AfterSceneLoadedLoadingScreenEvent += EventManager_AfterSceneLoadedLoadingScreenEventHandler;
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadedLoadingScreenEvent -= EventManager_AfterSceneLoadedLoadingScreenEventHandler;
    }

    private void Update()
    {
        //if (!previouslyActivated)
        //{
        //    previouslyActivated = true;
        //}
        if (dialogueEntries == null )
            return;
        if (dialogueEntries.Length > 0)
        {
            currentDialogueEntry = dialogueEntries[dialogueEntryIndex];
        }

        if (dialogueEntries.Length > 0)
        {
            if (dialogueEntries[dialogueEntryIndex].hasBeenUsed)
            {
                dialogueIndicator.SetActive(false);
            }
            else
            {
                dialogueIndicator.SetActive(true);
            }
        }
        switch (dialogueState)
        {
            case DialogueState.manual:
                if (canActivate && Input.GetKeyDown(KeyCode.F))
                {
                    currentDialogueEntry.hasBeenUsed = true;
                    if (quickText.Length <= 0)
                    {
                        DialogManager.Instance.SetupDialoguePanel(currentDialogueEntry.GetLines(), currentDialogueEntry.portrait);
                    }
                    else
                    {
                        DialogManager.Instance.SetQuickText(quickText);
                    }
                    DialogManager.Instance.SetDialogPanelActiveState(true);

                    Time.timeScale = 0.0f;
                    isActivated = true;
                }
                break;
            case DialogueState.auto:
                if (canAuto)
                {
                    DialogManager.Instance.SetupDialoguePanel(currentDialogueEntry.GetLines(), currentDialogueEntry.portrait);
                    DialogManager.Instance.SetDialogPanelActiveState(true);

                    Time.timeScale = 0.0f;
                    isActivated = true;
                }
                break;
        }
    }

    private void EventManager_AfterSceneLoadedLoadingScreenEventHandler()
    {
        canAuto = true;
    }
}
