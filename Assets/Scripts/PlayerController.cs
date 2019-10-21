using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float health = 5f;
    public float runSpeed = 5f;
    public Text hpCount;
    public float startTimeBtwAttack;
    public int damage = 1;
    public float attackRange;
    public LayerMask whatIsEnemies;


    private float timeBtwAttack;
    private bool isGrounded;
    private bool isAttack;

    [SerializeField]
    Transform attackPos;

    [SerializeField]
    GameObject AttackHitbox;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Transform groundCheckL;

    [SerializeField]
    Transform groundCheckR;


    Vector2 moveDirection;
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        AttackHitbox.SetActive(false);
        hpCount.text = "HP: " + health.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        if ((Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))) ||
    (Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))) ||
    (Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground"))))
        {
            isGrounded = true;
        }
        else
        {

            isGrounded = false;
            if (!isAttack)
                animator.Play("Character_Jump");
        }

        if (Input.GetKey("d") && !isAttack || Input.GetKey("right") && !isAttack)
        {
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);
            if (isGrounded)
                animator.Play("Character_Run");

            spriteRenderer.flipX = false;

        }
        else if (Input.GetKey("a") && !isAttack || Input.GetKey("left") && !isAttack)
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);
            if (isGrounded)
                animator.Play("Character_Run");

            spriteRenderer.flipX = true;

        }
        else
        {
            if (isGrounded && !isAttack)
                animator.Play("Character_Idle");

            rb2d.velocity = new Vector2(0, rb2d.velocity.y);

        }
        if (Input.GetKey("w") && isGrounded || Input.GetKey("space") && isGrounded && !isAttack)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 8);
            animator.Play("Character_Jump");
        }
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown("k"))
            {
                isAttack = true;
                animator.Play("Character_Attack");
                //Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position,new Vector2 (attackRange, attackRange), whatIsEnemies);
                //for (int i = 0; i < enemiesToDamage.Length; i++)
                //{
                //    enemiesToDamage[i].GetComponent<EnemyScript>().health -= damage;
                //}
                StartCoroutine(DoAttack());
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "EnemyAttackHitbox" && isAttack == false)
        {
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
                hpCount.text = "HP: " + health.ToString();
            }
            else
            {
                hpCount.text = "HP: " + health.ToString();
                // moveDirection = rb2d.transform.position - collision.transform.position; // Odrzut. Jeszcze cos wymysle.
                // rb2d.AddForce(moveDirection.normalized * -500f);
            }
        }

    }

    void TakeDamage()
    {

    }

    IEnumerator DoAttack()
    {
        AttackHitbox.SetActive(true);
        yield return new WaitForSeconds(.7f);
        AttackHitbox.SetActive(false);
        isAttack = false;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(attackPos.position,new Vector2(attackRange, attackRange));
    //}
}
