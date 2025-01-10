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

    [SerializeField] private float speed = 3f; 
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        direction = playerInput.actions["Movement"].ReadValue<Vector2>();

        Debug.Log(direction);
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direction * speed * Time.fixedDeltaTime);
    }
    
}
