using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Object/SODialogueTutorial")]

public class SODialogueTutorial : SODialogueEntry
{
    public KeyAction key = default;
    public string dialogueLine1 = default;
    public string dialogueLine2 = default;

    public void UpdateDisplayText()
    {
        dialogueLines[0] = dialogueLine1 + PlayerInteractTrigger.Instance.GetInteractKey(key) + dialogueLine2;
    }
}