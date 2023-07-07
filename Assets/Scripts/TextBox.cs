using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextBox", menuName = "DialogueSystem/TextBox", order = 1)]
public class TextBox : ScriptableObject {
    
    public string speaker;
    public List<String> lines = new List<string>();

    public bool HasNextLine(int lineIndex)
    {
        return this.lines.Count - 1 > lineIndex;
    }
    
}