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

    private float XCurrentSpeed;

    #region Unity Cycle
    private void Awake()
    {
        this.rb2d = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.isFacingRight = true;
        this.currentVelocity = Vector2.zero;
        this.XCurrentSpeed = this.XSpeed;
    }

    private void Update()
    {
        CaptureHorizontalMovementInput();
    }

    private void FixedUpdate()
    {
        if (!Mathf.Approximately(this.horizontalInput, 0.0f))
        {
            Move();
        }
    }
    #endregion

    private void CaptureHorizontalMovementInput()
    {
        this.horizontalInput = Input.GetAxis("Horizontal");

        if (rb2d.velocity.sqrMagnitude < .01 && rb2d.angularVelocity < .01)
        {
            
        }

        CaptureRunInput();
        CaptureFlipNecessity();

        Debug.Log(rb2d.velocity.sqrMagnitude);

        //Debug.Log(this.XCurrentSpeed / this.XSpeedRun);
        animator.SetFloat("Speed", this.XCurrentSpeed / this.XSpeedRun);
    }

    private void CaptureRunInput()
    {
        if (Input.GetButton("Run") && this.XCurrentSpeed < this.XSpeedRun && Mathf.Abs(this.horizontalInput) > 0.01f)
        {
            this.XCurrentSpeed += this.XAcceleration;
        }
        else if (Input.GetButtonUp("Run") || Mathf.Approximately(this.horizontalInput, 0.0f))
        {
            this.XCurrentSpeed = this.XSpeed;
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

    private void Move()
    {
        Vector2 targetLocation = new Vector2(this.XCurrentSpeed * this.horizontalInput * Time.deltaTime, rb2d.velocity.y);
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
