using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbSpeed = 6f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 13f);

    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimation;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float defaultGravity;
    bool isJumping = false;
    bool isAlive = true;

    const float ZeroGravity = 0f;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        defaultGravity = playerRigidBody.gravityScale;
    }

    void Update()
    {
        if (FindObjectOfType<GameSession>().isPaused)
            return;

        if (isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();

            isJumping = false;
        }
    }

    void OnMove(InputValue value)
    {
        if (isAlive)
            moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (isAlive)
        {
            if (IsTouchingGroundLayer() || IsTouchingClimbingLayer())
            {
                isJumping = true;
                playerRigidBody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2((moveInput.x * runSpeed), playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;

        playerAnimation.SetBool("isRunning", HasHorizontalSpeed());
    }

    void FlipSprite()
    {
        if (HasHorizontalSpeed())
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
    }

    void ClimbLadder()
    {
        if (IsTouchingClimbingLayer() && !isJumping)
        {
            Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, (moveInput.y * climbSpeed));

            playerRigidBody.velocity = climbVelocity;
            playerRigidBody.gravityScale = ZeroGravity;

            playerAnimation.SetBool("isClimbing", HasVerticalSpeed());
        }
        else
        {
            playerRigidBody.gravityScale = defaultGravity;
            playerAnimation.SetBool("isClimbing", false);
        }
    }

    void Die()
    {
        if (IsTouchingEnemyAndTrapLayer())
        {
            isAlive = false;
            playerAnimation.SetTrigger("Dying");
            playerRigidBody.velocity = deathKick;

            Invoke("DethProc", 1f);
        }
    }

    void DethProc()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    bool HasHorizontalSpeed()
    {
        return (Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon);
    }

    bool HasVerticalSpeed()
    {
        return (Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon);
    }

    bool IsTouchingGroundLayer()
    {
        return (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")));
    }

    bool IsTouchingClimbingLayer()
    {
        return (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")));
    }

    bool IsTouchingEnemyAndTrapLayer()
    {
        return (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Trap")) || playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Trap")));
    }
}