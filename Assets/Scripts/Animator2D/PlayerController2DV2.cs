using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2DV2 : MonoBehaviour
{
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float moveVel;
    [SerializeField] private float maxVel;
    [SerializeField] private Transform groundPos;
    [SerializeField] private Vector2 boxTam;
    [SerializeField] private LayerMask sueloLayer;


    private PlayerInput _playerInput;
    private Rigidbody2D _rb;
    private Vector2 _pos;
    private Transform _transform;
    public bool _isGrounded;
    private Vector2 _groundPos2D;

    //Animator
    private Animator _animator;
    private bool _facingRight;
    private SpriteRenderer _sprite;
    private bool _atacando;

    // Start is called before the first frame update
    void Start()
    {

        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _facingRight = true;
        _atacando = false;
    }
    private void Update()
    {
        if (!_atacando) //si estoy atacando no me puedo mover
        {
            _pos = _playerInput.actions["Mover"].ReadValue<Vector2>();
            //Debug.Log(_pos);

            _transform.position += new Vector3(_pos.x , _pos.y , 0) * moveVel * Time.deltaTime  ; //Movemos por codigo (aconsejable pero más complejo)
        }

        _groundPos2D = groundPos.position;
        _isGrounded = Grounded();

        //Actualizo el animator
        _animator.SetFloat("velX", Mathf.Abs(_pos.x) + Mathf.Abs(_pos.y));
        _animator.SetFloat("velY", _rb.velocity.y);
        _animator.SetBool("grounded", _isGrounded);

        //Giro el sprite si se mueve hacia la izquierda
        if (_pos.x > 0 && !_facingRight) { _sprite.flipX = false; _facingRight = true; } //Si vamos hacia la derecha ( vel > 0 ) miramos hacia la derecha
        if (_pos.x < 0 && _facingRight) { _sprite.flipX = true; _facingRight = false; } //Si vamos hacia la izquierda (vel < 0) miramos hacia la izquierda

    }
    private void FixedUpdate()
    {
        if (!_atacando)
        {
            if (_rb.velocity.magnitude <= maxVel) //Limitamos la velocidad de movimiento.
            {
                //_rb.AddForce(new Vector2(_pos.x, 0) * moveVel); // Movemos aplicando una fuerza (aconsejable)
            }
            //_rb.velocity = new Vector2(_pos.x * moveVel, _rb.velocity.y); // Movemos machacando la velocidad del rigidBody (no aconsejable pero útil)
        }
    }

    public void Saltar(InputAction.CallbackContext context)
    {
        //Debug.Log(context.phase);
        if (context.started && _isGrounded && !_atacando)
        {
            _rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            _animator.SetTrigger("saltar");
        }
    }
    public bool Grounded()
    {

        if (Physics2D.BoxCast(_groundPos2D, boxTam, 0f, Vector3.zero, 0f, sueloLayer)) { return true; } else { return false; }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundPos.position, boxTam);
    }
    public void Atacar(InputAction.CallbackContext context)
    {
        //Debug.Log(context.phase);
        if (context.started && _isGrounded && !_atacando)
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool("atacar",true);
            Atacando();
        }
    }
    public void Atacando() { _atacando = true; _animator.SetBool("atacar", true); }
    public void NoAtacando() { _atacando = false; _animator.SetBool("atacar", false); }
}
