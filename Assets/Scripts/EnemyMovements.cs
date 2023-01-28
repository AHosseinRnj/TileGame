using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    [SerializeField] float walkSpeed = 2f;

    Rigidbody2D enemyRigidBody;

    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Walk();
    }

    void Walk()
    {
        enemyRigidBody.velocity = new Vector2(walkSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        walkSpeed = -(walkSpeed);
        FlipSprite();
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidBody.velocity.x)), 1f);
    }
}