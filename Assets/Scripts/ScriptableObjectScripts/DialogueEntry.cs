using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DialogueEntry : ScriptableObject
{
    public string[] dialogueLines;
    public bool hasBeenUsed = false;
}
