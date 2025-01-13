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
    private SpriteRenderer spriteRenderer;
    [SerializeField] private string IdleStateName = "MeleeEnemy_Idle";
    [SerializeField] private string MoveStateName = "MeleeEnemy_Move";
    [SerializeField] private int hitsToDie = 4;
    private int hitCount = 0;
    [SerializeField] private GameObject bloodPrefab;
    private BoxCollider2D boxCollider;
    private bool canMove = true;
    [SerializeField] float hurtForce = 10000000f;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // Cambia el sortingOrder según la posición en Y
        // Invertimos la posición en Y para que objetos más abajo en la pantalla se dibujen encima
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);

        direction = (player.transform.position - transform.position).normalized;

        if (direction == Vector2.zero)
        {
            animator.Play(IdleStateName);
        }
        else if (direction != Vector2.zero && canMove)
        {
            animator.Play(MoveStateName);
        }
        else if (direction != Vector2.zero && !canMove)
        {
            animator.Play(IdleStateName);
        }

        FlipHorizontally();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb2D.MovePosition(rb2D.position + direction * speed * Time.fixedDeltaTime);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 hurtDirection = -(collision.transform.position - transform.position).normalized;
            StartCoroutine(EnemyGetDamage(hurtDirection));
        }
    }

    private IEnumerator EnemyGetDamage(Vector2 hurtDirection)
    {
        boxCollider.enabled = false;
        canMove = false;
        hitCount++;
        rb2D.AddForce(hurtDirection * hurtForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = Color.white;
        
        if (hitCount >= hitsToDie)
        {
            EnemyDies();
        }

        boxCollider.enabled = true;
        canMove = true;
    }

    private void EnemyDies()
    {
        Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
