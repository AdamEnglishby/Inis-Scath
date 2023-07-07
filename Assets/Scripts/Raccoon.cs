using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Raccoon : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera rollingCamera, walkingCamera;
    [SerializeField] private AnimationCurve cameraDistance, gravityScale;
    
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private float rollingMoveSpeed = 10f, rollingMoveSpeedAir = 5f;
    [SerializeField] private float walkingMoveSpeed = 0.2f, walkingMoveSpeedAir = 0.05f;
    [SerializeField] private bool isRolling = true;

    [SerializeField] private SpriteRenderer mainFrame1, mainFrame2, rollFrame1;

    private float _frame;
    public ContactFilter2D collisions;
    
    private bool _isGrounded;

    private CircleCollider2D _collider;
    private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody2D => this._rigidbody;

    void Start()
    {
        this._collider = this.GetComponent<CircleCollider2D>();
        this._rigidbody = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (GameManager.gameplayPaused) {
            return;
        }
        
        this._rigidbody.gravityScale = this.gravityScale.Evaluate(this._rigidbody.velocity.magnitude);
        this._isGrounded = this._rigidbody.IsTouching(this.collisions);

        bool usingGamepad = Gamepad.all.Count > 0;

        Vector2 wasd = new Vector2(-1 * Convert.ToInt32(Keyboard.current[Key.A].isPressed) + Convert.ToInt32(Keyboard.current[Key.D].isPressed), 0f).normalized;
        bool jump = Keyboard.current[Key.W].isPressed || Keyboard.current[Key.W].isPressed;
        bool roll = Keyboard.current[Key.Space].isPressed;
        
        Vector2 leftStick = Vector2.zero;
        if (usingGamepad) {
            leftStick = Gamepad.current.leftStick.ReadValue().normalized;
            jump = jump || Gamepad.current[GamepadButton.South].isPressed;
            roll = roll || Gamepad.current[GamepadButton.RightTrigger].isPressed;
        }

        if (!this.isRolling) {
            if(roll)
            {
                this._collider.radius = 0.35f;
                this.mainFrame1.enabled = false;
                this.mainFrame2.enabled = false;
                this.rollFrame1.enabled = true;
                this.rollingCamera.Priority = 1;
                this.walkingCamera.Priority = 0;
                this._rigidbody.constraints = RigidbodyConstraints2D.None;
            }
        } else {
            Vector2.ClampMagnitude(this._rigidbody.velocity, 2f);
            if(!roll)
            {
                this.transform.rotation = new Quaternion();
                this._collider.radius = 0.45f;
                this.mainFrame1.enabled = true;
                this.mainFrame2.enabled = false;
                this.rollFrame1.enabled = false;
                this.rollingCamera.Priority = 0;
                this.walkingCamera.Priority = 1;
                this._rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        this.isRolling = roll;
        
        if (this._isGrounded)
        {
            _frame += Time.deltaTime * this._rigidbody.velocity.magnitude * 2.5f;
            if (jump)
            {
                this._rigidbody.AddForce(new Vector2(0, this.jumpHeight));
            }

            if (!this.isRolling)
            {
                if (Mathf.Ceil(this._frame) % 2 == 0)
                {
                    this.mainFrame1.enabled = true;
                    this.mainFrame2.enabled = false;
                }
                else
                {
                    this.mainFrame1.enabled = false;
                    this.mainFrame2.enabled = true;
                }
            }

            // L/R movement on ground
            if (this.isRolling)
            {
                Vector2 movement = new Vector2(this.rollingMoveSpeed * leftStick.x + this.rollingMoveSpeed * wasd.x, 0);
                this._rigidbody.AddForce(movement);
            } else {
                Vector2 movement = new Vector2(this.walkingMoveSpeed * leftStick.x + this.walkingMoveSpeed * wasd.x, 0);
                this._rigidbody.AddForce(movement);
                this._rigidbody.velocity *= 0.8f; // filthy magic number for damping
            }
        } else {
            // L/R movement in air
            if (this.isRolling) {
                Vector2 movement = new Vector2(this.rollingMoveSpeedAir * leftStick.x + this.rollingMoveSpeedAir * wasd.x, 0);
                this._rigidbody.AddForce(movement);
            } else {
                Vector2 movement = new Vector2(this.walkingMoveSpeedAir * leftStick.x + this.walkingMoveSpeedAir * wasd.x, 0);
                this._rigidbody.AddForce(movement);
            }
        }

        // zoom camera out based on velocity
        float horizontalVelocity = this._rigidbody.velocity.x;
        this.rollingCamera.m_Lens.OrthographicSize = cameraDistance.Evaluate(Mathf.Abs(horizontalVelocity));
    }
}
