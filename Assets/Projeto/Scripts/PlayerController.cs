using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody2d;
    public Transform groundCheck;

    public bool isGround = false;
    public bool facingRigth = true;

    public float speed;
    public float touchRun = 0.0f;

    //Pulo
    public bool jump = false;
    public int numberJumps = 0;
    public int maxJump = 2;
    public float jumpForce;

    private GameController _gameController;

    public AudioClip fxPulo;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        playerAnimator.SetBool("IsGrounded", isGround);

        Debug.Log(isGround.ToString());

        touchRun = Input.GetAxisRaw("Horizontal");
        Debug.Log(touchRun.ToString());

        if (Input.GetButtonDown("Jump")) // tecla de spaço
        {
            jump = true;
        }

        SetaMovimentos();
    }

    private void FixedUpdate()
    {
        MovePlayer(touchRun);

        if (jump) // jump for true
        {
            JumpPlayer();
        }
    }

    void JumpPlayer()
    {
        if (isGround)
        {
            numberJumps = 0;
        }

        if (isGround || numberJumps < maxJump)
        {
            playerRigidbody2d.AddForce(new Vector2(0f, jumpForce));
            isGround = false;
            numberJumps++;

            // Som da coleta da cenoura
            _gameController.fxGame.PlayOneShot(fxPulo);
        }
        jump = false;
    }

    void Flip()
    {
        facingRigth = !facingRigth;
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1; // 1 ou -1

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void MovePlayer(float movimentoHorizontal)
    {
        playerRigidbody2d.velocity = new Vector2(movimentoHorizontal * speed, playerRigidbody2d.velocity.y);

        if(movimentoHorizontal < 0 && facingRigth || (movimentoHorizontal > 0 && !facingRigth))
        {
            Flip();
        }
    }

    void SetaMovimentos()
    {
        playerAnimator.SetBool("Walk", playerRigidbody2d.velocity.x != 0 && isGround); // true
        playerAnimator.SetBool("Jump", !isGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coletaveis":
                _gameController.Pontuacao(1);
                Destroy(collision.gameObject);
                break;
            case "Enemy":
                //Instanciar a animação da explosão
                GameObject tempExplosao = Instantiate(_gameController.hitPrefab, transform.position, transform.localRotation);
                Destroy(tempExplosao, 0.5f);

                //Adiciona força ao pulo
                Rigidbody2D rigidbody2D = GetComponentInParent<Rigidbody2D>();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                rigidbody2D.AddForce(new Vector2(0, 900));

                // Som pulo
                _gameController.fxGame.PlayOneShot(_gameController.fxExplosao);

                // Destroi inimigo
                Destroy(collision.gameObject);
                break;
        }
    }
}
