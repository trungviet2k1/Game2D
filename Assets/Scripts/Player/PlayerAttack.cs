using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Cooldown")]
    [SerializeField] private float fireballCooldown;
    [SerializeField] private float strikeCooldown;

    [Header("Fireball Skill")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Strike Skill")]
    [SerializeField] private float strikeMoveDistance;
    [SerializeField] private float pushBackDistance;
    [SerializeField] private float damageAmount;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip strikeSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private GiftBox box;
    private HealthEnemy healthEnemy;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        box = GetComponent<GiftBox>();
        healthEnemy = GetComponent<HealthEnemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer > fireballCooldown && playerMovement.canAttack())
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.W) && cooldownTimer > strikeCooldown && playerMovement.canAttack())
        {
            SoundManager.instance.PlaySound(strikeSound);
            anim.SetTrigger("strike");
            cooldownTimer = 0;
            StartCoroutine(MoveDuringStrike());

            if (IsCollidingWithWall(transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * pushBackDistance, 0f, 0f)))
            {
                PushBack();
            }

            Strike();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        //pool fireballs
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<FireBall>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void Strike()
    {
        Vector2 contactPoint = GetStrikeContactPoint();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(contactPoint, 2f);
        foreach (Collider2D collider in colliders)
        {
            box = collider.GetComponent<GiftBox>();
            healthEnemy = collider.GetComponent<HealthEnemy>();

            if (box != null)
            {
                box.TakeDamage(damageAmount);
            }

            if(healthEnemy != null)
            {
                healthEnemy.TakeDamage(damageAmount);
            }
        }
    }

    private void PushBack()
    {
        Vector3 pushBackDirection = new Vector3(-Mathf.Sign(transform.localScale.x), 0f, 0f);
        Vector3 pushBackTarget = transform.position + pushBackDirection * pushBackDistance;

        StartCoroutine(MoveToTargetPosition(pushBackTarget));
    }

    private Vector2 GetStrikeContactPoint()
    {
        Vector2 contactPoint = (Vector2)transform.position + new Vector2(Mathf.Sign(transform.localScale.x) * 2f, -0.6f);
        return contactPoint;
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private IEnumerator MoveDuringStrike()
    {
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * strikeMoveDistance, 0f, 0f);

        if (!IsCollidingWithWall(targetPosition))
        {
            float elapsedTime = 0f;
            float duration = 0.2f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator MoveToTargetPosition(Vector3 targetPosition)
    {
        Vector3 initialPosition = transform.position;

        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private bool IsCollidingWithWall(Vector3 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetStrikeContactPoint(), 0.1f);
    }
}
