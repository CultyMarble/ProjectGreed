using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Object/Dialog Entry")]
public class SODialogueEntry : ScriptableObject
{
    public bool hasBeenUsed = default;
    public string[] dialogueLines = default;
}