using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private TMP_Text dialogue, speaker;
    
    private Conversation _activeConversation;
    private int _textBoxIndex, _lineIndex;

    public DialogueManager Instance => this;

    public void Awake()
    {
        StartConversation("Dialogue/Conversation - Opening.asset");
    }

    public void StartConversation(Conversation conversation)
    {
        GameManager.gameplayPaused = true;
        _activeConversation = conversation;
        _textBoxIndex = 0;
        _lineIndex = 0;
        RefreshText();
    }

    public void StartConversation(string assetPath)
    {
        #if UNITY_EDITOR
        Conversation conversation = AssetDatabase.LoadAssetAtPath<Conversation>("Assets/Resources/" + assetPath);
        StartConversation(conversation);
        #else
        Conversation conversation = Resources.Load<Conversation>(assetPath);
        #endif
        
    }

    public void ProceedConversation()
    {
        TextBox currentTextBox = _activeConversation.textBoxes[_textBoxIndex];
        if (currentTextBox.HasNextLine(this._lineIndex))
        {
            this._lineIndex++;
        }
        else
        {
            if (_activeConversation.HasNextBox(this._textBoxIndex))
            {
                this._textBoxIndex++;
            }
            else
            {
                EndConversation();
            }
        }
        RefreshText();
    }

    private void EndConversation()
    {
        GameManager.gameplayPaused = false;
        _activeConversation = null;
        _textBoxIndex = 0;
        _lineIndex = 0;
        RefreshText();
    }

    private void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            if (_activeConversation == null)
            {
                StartConversation("Assets/Dialogue/Conversation - Opening.asset");
            }
            else
            {
                ProceedConversation();
            }
        }
    }

    private void RefreshText()
    {
        if (this._activeConversation == null)
        {
            speaker.transform.parent.gameObject.SetActive(false);
            dialogue.transform.parent.gameObject.SetActive(false);
            return;
        }
        
        speaker.transform.parent.gameObject.SetActive(true);
        dialogue.transform.parent.gameObject.SetActive(true);
        
        TextBox activeTextBox = _activeConversation.textBoxes[this._textBoxIndex];
        speaker.text = activeTextBox.speaker;
        dialogue.text = activeTextBox.lines[this._lineIndex];
    }

}