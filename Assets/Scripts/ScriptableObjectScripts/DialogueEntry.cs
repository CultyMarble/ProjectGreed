using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Dialogue
{
    [TextArea(2,2)]public string dialogue;
    public bool playerSpeaking = false;
}
[CreateAssetMenu]

public class DialogueEntry : ScriptableObject
{
    public Dialogue[] dialogueLines;
    public Sprite portrait;
    public bool hasBeenUsed = false;

    public Tuple<string,bool>[] GetLines()
    {
        Tuple<string,bool>[] lines = new Tuple<string,bool>[dialogueLines.Length];

        for(int i = 0; i < dialogueLines.Length; i++)
        {
            Dialogue line = dialogueLines[i];
            lines[i] = Tuple.Create(line.dialogue, line.playerSpeaking);
        }
        return lines;
    }
}
