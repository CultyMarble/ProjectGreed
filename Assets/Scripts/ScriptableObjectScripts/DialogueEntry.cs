using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]

public class Dialogue
{
    public string dialogue;
    public bool playerSpeaking = false;
}

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
