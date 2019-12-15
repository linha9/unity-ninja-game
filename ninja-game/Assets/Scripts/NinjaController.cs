using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float movementSmooth = 0.0f;

    private Rigidbody2D rb2d;
    private Animator animator;

    private Vector2 velocity;
    private bool isFacingRight;
    private float horizontalInput;

    #region Unity Cycle
    private void Awake()
    {
        this.rb2d = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.isFacingRight = true;
        this.velocity = Vector2.zero;
    }

    private void Update()
    {
        this.horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(this.horizontalInput));
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
        Vector2 targetLocation = new Vector2(this.speed * this.horizontalInput * Time.deltaTime, rb2d.velocity.y);
        this.rb2d.velocity = Vector2.SmoothDamp(this.rb2d.velocity, targetLocation, ref this.velocity, this.movementSmooth);

        if (this.horizontalInput < 0.0f && this.isFacingRight)
        {
            Flip();
        }
        else if (this.horizontalInput > 0.0f && !this.isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        this.isFacingRight = !this.isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        base.transform.localScale = currentScale;
    }
}
