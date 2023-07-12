using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private string quickText;

    [SerializeField] private bool autoActive;
    [SerializeField] private GameObject dialogueIndicator;

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
                    dialogueEntries[dialogueEntryIndex].hasBeenUsed = true;
                    if (quickText.Length <= 0)
                    {
                        DialogManager.Instance.SetDialogLines(dialogueEntries[dialogueEntryIndex].dialogueLines);
                    }
                    else
                    {
                        DialogManager.Instance.SetDialogLines(quickText);
                    }
                    DialogManager.Instance.SetDialogPanelActiveState(true);

                    Time.timeScale = 0.0f;
                    isActivated = true;
                }
                break;
            case DialogueState.auto:
                if (canAuto)
                {
                    DialogManager.Instance.SetDialogLines(dialogueEntries[dialogueEntryIndex].dialogueLines);
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
