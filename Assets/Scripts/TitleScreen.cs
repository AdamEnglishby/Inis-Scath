using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    [SerializeField] private GameObject scroller, proceedArrow;
    [SerializeField] private SpriteRenderer black;
    [SerializeField] private TMP_Text pressAnyButton, title, dialogue1, dialogue2, dialogue3;
    private int _timer = 0;
    private const int TriggerFade = 90;
    private bool _hasPressedButton, _hasReachedEnd, _dialogue1Done, _dialogue1Faded, _dialogue2Done, _dialogue2Faded, _dialogue3Done, _dialogue3Faded;
    
    public void Update()
    {
        if (!this._hasPressedButton)
        {
            this._hasPressedButton = Keyboard.current.anyKey.wasPressedThisFrame;
        }
        
        if (this._dialogue1Done && !this._dialogue1Faded)
        {
            this.proceedArrow.gameObject.SetActive(true);
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                this.proceedArrow.gameObject.SetActive(false);
                this.dialogue1.gameObject.SetActive(false);
                StartCoroutine(FadeTo(Color.white, this.dialogue2, 100, 180, () =>
                {
                    this._dialogue1Faded = true;
                    this._dialogue2Done = true;
                }));
            }
        }
        
        if (this._dialogue2Done && !this._dialogue2Faded)
        {
            this.proceedArrow.gameObject.SetActive(true);
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                this.proceedArrow.gameObject.SetActive(false);
                this.dialogue2.gameObject.SetActive(false);
                StartCoroutine(FadeTo(Color.white, this.dialogue3, 100, 120, () =>
                {
                    this._dialogue2Faded = true;
                    this._dialogue3Done = true;
                }));
            }
        }
        
        if (this._dialogue3Done && !this._dialogue3Faded)
        {
            this.proceedArrow.gameObject.SetActive(true);
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene("Scenes/SampleScene", LoadSceneMode.Single);
            }
        }
        
    }

    public void FixedUpdate()
    {
        if (!this._hasPressedButton)
        {
            return;
        }
        
        this.pressAnyButton.gameObject.SetActive(false);
        this.title.gameObject.SetActive(false);
        
        scroller.transform.position -= Vector3.down * (Time.deltaTime / 2);
        if (this._timer < TriggerFade)
        {
            this._timer++;
        }
        else
        {
            if (this._hasReachedEnd)
            {
                if (!this._dialogue1Done)
                {
                    StartCoroutine(FadeTo(Color.white, this.dialogue1, 100, 60, () =>
                    {
                        this.proceedArrow.gameObject.SetActive(true);
                        this._dialogue1Done = true;
                    }));
                }

            }
            else
            {
                StartCoroutine(FadeTo(100, 60));
            }
        }

    }
    
    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = black.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha,aValue,t));
            black.color = newColor;
            this._hasReachedEnd = black.color.a * 100 >= aValue;

            yield return null;
        }
        
    }
    
    IEnumerator FadeTo(Color toFadeTo, TMP_Text text, float aValue, float aTime, Action d)
    {
        float alpha = text.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(toFadeTo.r, toFadeTo.g, toFadeTo.b, Mathf.Lerp(alpha,aValue,t));
            text.color = newColor;
            if (text.color.a * 100 >= aValue)
            {
                d.Invoke();
            }

            yield return null;
        }
    }
    
    IEnumerator FadeOut(Color toFadeTo, TMP_Text text, float aValue, float aTime, Action d)
    {
        float alpha = text.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(toFadeTo.r, toFadeTo.g, toFadeTo.b, Mathf.Lerp(alpha,aValue,t));
            text.color = newColor;
            Debug.Log(text.color.a);
            if (text.color.a * 100 <= aValue)
            {
                d.Invoke();
            }

            yield return null;
        }
    }

}
