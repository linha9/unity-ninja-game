using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaMovement : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 200.0f;
    [SerializeField]
    private float runSpeed = 300.0f;
    [SerializeField]
    private float runAccelaration = 3.0f;
    [SerializeField]
    private float xSmooth = 0.0f;
    [SerializeField]
    private float jumpForce = 300.0f;
    [SerializeField]
    private Transform groundPoint;
    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private Animator animator;

    private Vector2 currentVelocity;

    private float currentSpeed;

    private float xInput;

    private bool isMoving;
    private bool isWalking;
    private bool isRunning;
    private bool isFacingRight;

    private bool isGrounded;
    private bool isJumping;

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2.0f;

    private List<Collider2D> collisions;

    #region Unity Cycle
    private void Awake()
    {
        this.rb2d = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.currentSpeed = 0.0f;
        this.currentVelocity = Vector2.zero;
        this.collisions = new List<Collider2D>();

        this.xInput = 0.0f;
        this.isMoving = false;
        this.isRunning = false;
        this.isWalking = false;
        this.isJumping = false;
        this.isGrounded = false;
        this.isFacingRight = true;
    }

    private void Update()
    {
        handleXInput();

        if (Input.GetButtonDown("Jump") && !this.isJumping)
        {
            this.isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        if (this.isMoving)
        {
            Move();
        }

        GroundCheck();
        
        if (this.isJumping && this.isGrounded)
        {
            Jump();
            this.isJumping = false;
        }
    }
    #endregion

    private void Move()
    {
        Vector2 targetLocation = new Vector2(this.xInput * this.currentSpeed * Time.deltaTime, rb2d.velocity.y);
        this.rb2d.velocity = Vector2.SmoothDamp(this.rb2d.velocity, targetLocation, ref this.currentVelocity, this.xSmooth);
    }

    private void GroundCheck()
    {
        this.isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.groundPoint.position, .15f, this.groundLayer);


        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != this.gameObject)
            {
                this.isGrounded = true;
            }
        }
    }

    private void Jump()
    {
        /*if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }*/

        Debug.Log(-(Physics2D.gravity.y) * (fallMultiplier) * Time.deltaTime * 10);
        this.rb2d.velocity = new Vector2(rb2d.velocity.x, -(Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * 10));
        //this.rb2d.velocity = new Vector2(rb2d.velocity.x, 6.0f);

        //this.rb2d.AddForce(new Vector2(0.0f, this.jumpForce + this.currentSpeed));
    }

    private void handleXInput()
    {
        // Get horizontal input
        this.xInput = Input.GetAxis("Horizontal");

        // Determine if isMoving, isWalking or isRunning
        if (!Mathf.Approximately(this.xInput, 0.0f))
        {
            this.isMoving = true;

            if (Input.GetButton("Run"))
            {
                this.isRunning = true;
                this.isJumping = false;
            }
            else
            {
                this.isRunning = false;
            }

            this.isWalking = !this.isRunning;
        }
        else
        {
            this.isMoving = false;
            this.isRunning = false;
            this.isWalking = false;
        }

        // Determine speed
        if (this.isRunning && this.currentSpeed < this.runSpeed)
        {
            this.currentSpeed += this.runAccelaration;
        }
        else if (this.isWalking)
        {
            this.currentSpeed = this.walkSpeed;
        }
        else if (!this.isMoving)
        {
            this.currentSpeed = 0.0f;
        }

        // Flip
        if (this.xInput < -0.01f && isFacingRight)
        {
            Flip();
        }
        else if (this.xInput > 0.01f && !isFacingRight)
        {
            Flip();
        }

        // Animate
        animator.SetBool("isRunning", this.isRunning);
        animator.SetBool("isWalking", this.isWalking);
        animator.SetBool("isMoving", this.isMoving);
    }

    private void Flip()
    {
        this.isFacingRight = !this.isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        base.transform.localScale = currentScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            this.isGrounded = true;
        }
    }
}
