using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private new BoxCollider2D collider;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float repelForce;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemys;
    [SerializeField] EnemyHealth enemyHealth;
    [SerializeField] private float attackRateTime;
    private float attackRateCounter;
    [HideInInspector] public bool isAttacking;
    private Movement movement;

    private Animator animator;

    private void Start()
    {
        movement = gameObject.GetComponent<Movement>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private bool attack;
    private void Update()
    {
        if(!Menu.gameIsPaused && movement.detectInput)
        {
            attack = Input.GetButtonDown("Fire1");

            if (attack && attackRateCounter > attackRateTime && !isAttacking && !movement.dash)
            {
                isAttacking = true;
                animator.SetBool("Attack", true);
            
            }
            else
                attackRateCounter += Time.deltaTime;

        }

    }
    private void finishAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        attackRateCounter = 0;
    }
    private void doDamage()
    {
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemys);
        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<EnemyMove>().stopCounter = 0.5f;
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            int directionX = (collider.bounds.min.x > enemy.GetComponent<BoxCollider2D>().bounds.min.x) ? -1 : 1;
            int directionY = (collider.bounds.min.y > enemy.GetComponent<BoxCollider2D>().bounds.min.y) ? -1 : 1;
            enemy.GetComponent<Rigidbody2D>().AddForce(repelForce * directionX * Vector2.right, ForceMode2D.Impulse);
            enemy.GetComponent<Rigidbody2D>().AddForce(repelForce * directionY * Vector2.up, ForceMode2D.Impulse);


            if (enemy.GetComponent<EnemyHealth>().takeDamageEnemie(damage) == 0)
                enemy.GetComponent<EnemyMove>().isAlive = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
