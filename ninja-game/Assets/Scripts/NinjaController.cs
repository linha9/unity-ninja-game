using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField]
    private float XSpeed = 200.0f;
    [SerializeField]
    private float XSpeedRun = 300.0f;
    [SerializeField]
    private float XAcceleration = 3.0f;
    [SerializeField]
    private float XMoveSmooth = 0.0f;

    private Rigidbody2D rb2d;
    private Animator animator;

    private Vector2 currentVelocity;
    private bool isFacingRight;
    private float horizontalInput;

    private float currentSpeed;

    #region Unity Cycle
    private void Awake()
    {
        this.rb2d = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.isFacingRight = true;
        this.currentVelocity = Vector2.zero;
        this.currentSpeed = this.XSpeed;
    }

    private void Update()
    {
        CaptureHorizontalMovementInput();
    }

    private void CaptureHorizontalMovementInput()
    {
        this.horizontalInput = Input.GetAxis("Horizontal");

        CaptureRunInput();
        CaptureFlipNecessity();

        animator.SetFloat("Speed", Mathf.Abs(this.horizontalInput));
    }

    private void CaptureRunInput()
    {
        if (Input.GetButton("Run") && this.currentSpeed < this.XSpeedRun)
        {
            this.currentSpeed += this.XAcceleration;
        }
        else if (Input.GetButtonUp("Run"))
        {
            this.currentSpeed = this.XSpeed;
        }
    }

    private void CaptureFlipNecessity()
    {
        if (this.horizontalInput < 0.0f && this.isFacingRight)
        {
            Flip();
        }
        else if (this.horizontalInput > 0.0f && !this.isFacingRight)
        {
            Flip();
        }
    }



    private void FixedUpdate()
    {
        if (!Mathf.Approximately(this.horizontalInput, 0.0f))
        {
            Move();
        }
    }
    #endregion

    private void Move()
    {
        Vector2 targetLocation = new Vector2(this.currentSpeed * this.horizontalInput * Time.deltaTime, rb2d.velocity.y);
        this.rb2d.velocity = Vector2.SmoothDamp(this.rb2d.velocity, targetLocation, ref this.currentVelocity, this.XMoveSmooth);
    }

    private void Flip()
    {
        this.isFacingRight = !this.isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        base.transform.localScale = currentScale;
    }
}
