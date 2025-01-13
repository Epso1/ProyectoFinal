using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private PlayerInput playerInput;
    private Vector2 direction;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private string IdleStateName = "Player_Idle";
    [SerializeField] private string MoveStateName = "Player_Move";


    [SerializeField] private float speed = 3f; 
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        direction = playerInput.actions["Movement"].ReadValue<Vector2>();
        if (direction == Vector2.zero)
        {
            animator.Play(IdleStateName);
        }
        else
        {
            animator.Play(MoveStateName);
        }

        FlipHorizontally();

        // Cambia el sortingOrder según la posición en Y
        // Invertimos la posición en Y para que objetos más abajo en la pantalla se dibujen encima
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
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
