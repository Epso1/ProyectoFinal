using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    private Rigidbody2D rb2D;
    private Vector2 direction;
    private GameObject player;
    private Animator animator;
    [SerializeField] private string IdleStateName = "MeleeEnemy_Idle";
    [SerializeField] private string MoveStateName = "MeleeEnemy_Move";
    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;

        if (direction == Vector2.zero)
        {
            animator.Play(IdleStateName);
        }
        else
        {
            animator.Play(MoveStateName);
        }

        FlipHorizontally();
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direction * speed * Time.fixedDeltaTime);
    }

    private void FlipHorizontally()
    {
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
