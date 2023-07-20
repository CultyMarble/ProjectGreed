using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private string quickText;
    [SerializeField] private bool playOnlyOnce = false;


    [SerializeField] private bool autoActive;
    [SerializeField] private GameObject dialogueIndicator;

    private DialogueEntry currentDialogueEntry;
    private int dialogueEntryIndex;
    private bool canActivate;
    private bool isActivated;
    private bool canAuto = false;
    private bool previouslyActivated = false;
    private float standTimer = 0.75f;
    private float standCounter = 0f;

    private enum DialogueState
    {
        auto,
        manual,
    }
    private DialogueState dialogueState;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && dialogueState == DialogueState.manual)
            Player.Instance.ShowFPromtText();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (dialogueState == DialogueState.auto)
        //    return;
        if (collision.CompareTag("Player") && standCounter >= standTimer)
        {
            standCounter = 0f;
            canActivate = true;
        }
        else
        {
            standCounter += Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (dialogueState == DialogueState.auto)
        //    return;

        if (collision.CompareTag("Player"))
        {
            Player.Instance.HideFPromtText();
            canActivate = false;
            isActivated = false;
            standCounter = 0f;
        }
    }

    //===========================================================================
    private void Awake()
    {
        EventManager.AfterSceneLoadedLoadingScreenEvent += EventManager_AfterSceneLoadedLoadingScreenEvent;
        Debug.Log("Subscribed");
    }


    private void Start()
    {
        if (dialogueEntries == null && quickText == null)
            return;
        if (playOnlyOnce && currentDialogueEntry.hasBeenUsed)
        {
            return;
        }

        currentDialogueEntry = new DialogueEntry();

        if (autoActive)
        {
            dialogueState = DialogueState.auto;
        }
        else
        {
            dialogueState = DialogueState.manual;
            canAuto = false;
        }
        if(dialogueEntries.Length > 0)
        {
            dialogueEntryIndex = Random.Range(0, dialogueEntries.Length);
        }
        else
        {
            dialogueEntryIndex = 0;
        }

    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadedLoadingScreenEvent -= EventManager_AfterSceneLoadedLoadingScreenEvent;
    }

    private void Update()
    {
        //if (!previouslyActivated)
        //{
        //    previouslyActivated = true;
        //}
        if(playOnlyOnce && currentDialogueEntry.hasBeenUsed)
        {
            return;
        }

        if (currentDialogueEntry.hasBeenUsed && dialogueEntries.Length > 0)
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
                        currentDialogueEntry.hasBeenUsed = true;
                    }
                    else
                    {
                        DialogManager.Instance.SetQuickText(quickText);
                        currentDialogueEntry.hasBeenUsed = true;
                    }
                    DialogManager.Instance.SetDialogPanelActiveState(true);

                    Time.timeScale = 0.0f;
                    isActivated = true;
                }
                break;
            case DialogueState.auto:
                if (canAuto && canActivate && currentDialogueEntry.hasBeenUsed == false)
                {
                    if (quickText.Length <= 0)
                    {
                        DialogManager.Instance.SetupDialoguePanel(currentDialogueEntry.GetLines(), currentDialogueEntry.portrait);
                        currentDialogueEntry.hasBeenUsed = true;
                    }
                    else
                    {
                        DialogManager.Instance.SetQuickText(quickText);
                        currentDialogueEntry.hasBeenUsed = true;
                    }
                    DialogManager.Instance.SetDialogPanelActiveState(true);

                    Time.timeScale = 0.0f;
                    isActivated = true;
                }
                break;
        }
    }

    //===========================================================================

    private void EventManager_AfterSceneLoadedLoadingScreenEvent()
    {
        canAuto = true;
    }
}