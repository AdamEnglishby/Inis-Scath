using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private bool _destroyed;
    [SerializeField] private float velocityThreshold;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private ParticleSystem particleSystem;
    
    private void Start()
    {
        this._collider = GetComponent<Collider2D>();
        this._spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!this._destroyed && other.gameObject.CompareTag("Player"))
        {
            Raccoon raccoon = other.gameObject.GetComponent<Raccoon>();
            
            if (raccoon.Rigidbody2D.velocity.magnitude > this.velocityThreshold)
            {
                this._spriteRenderer.enabled = false;
                this.hitbox.gameObject.SetActive(false);
                this.particleSystem.Play();
                this._destroyed = true;
            }
        }
    }
}
