using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DialogueSystem/Conversation", order = 1)]
public class Conversation : ScriptableObject
{

    public List<TextBox> textBoxes = new List<TextBox>();

    public bool HasNextBox(int textBoxIndex)
    {
        return this.textBoxes.Count - 1 > textBoxIndex;
    }

}