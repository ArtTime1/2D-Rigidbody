using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 10f;
    public GameObject platform;
    private Vector3 spawnPoint;
   
    private bool isGrounded = true;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        spawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    void Update()
    {
        if(isGrounded) State = CharState.Idle;
        
        if (Input.GetButton("Horizontal")) Movement();
        if (Input.GetButtonDown("Jump") && isGrounded) Jump();
    }

    private void Movement()
    {       
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
      
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x < 0.0f;

        if (isGrounded) State = CharState.Run;
    }

    private void Jump()
    {    
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        State = CharState.Jump;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.7f);

        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Death")
        {
            Debug.Log("You are dead");
            transform.position = spawnPoint;
        }
    }
}

public enum CharState
{
    Idle,
    Run,
    Jump
}
