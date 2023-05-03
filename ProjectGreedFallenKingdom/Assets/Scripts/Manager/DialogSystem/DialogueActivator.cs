using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private bool autoActive;

    private bool canActivate;
    private bool isActivated;
    private bool canAuto = false;

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

        EventManager.AfterSceneLoadedLoadingScreenEvent += EventManager_AfterSceneLoadedLoadingScreenEventHandler;
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadedLoadingScreenEvent -= EventManager_AfterSceneLoadedLoadingScreenEventHandler;
    }

    private void Update()
    {
        if (isActivated) return;

        switch(dialogueState)
        {
            case DialogueState.manual:
                if (canActivate && Input.GetKeyDown(KeyCode.F))
                {
                    DialogManager.Instance.SetDialogLines(dialogueLines);
                    DialogManager.Instance.SetDialogPanelActiveState(true);
                    isActivated = true;
                }
                break;
            case DialogueState.auto:
                if (canAuto)
                {
                    DialogManager.Instance.SetDialogLines(dialogueLines);
                    DialogManager.Instance.SetDialogPanelActiveState(true);
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