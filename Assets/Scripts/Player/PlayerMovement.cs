using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask boxLayer;

    [Header("Dash Wind Effect")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Transform dashPoint;
    [SerializeField] private Transform dashEffectHolder;
    [SerializeField] private GameObject dashWindPrefab;
    private GameObject currentDashWind;
    private bool hasReceivedSpeedRunSkill = false;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashwindSound;

    public Rigidbody2D rb2d { get; private set; }
    public static PlayerMovement Instance;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private PlayerBlocking playerBlocking;
    private float horizontalInput;
    private bool isJumping = false;
    private bool canDash = false;
    private bool isFastRunning = false;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private bool isDashWindReady = true;
    private float dashWindCooldown = 3f;

    private void Start()
    {
        hasReceivedSpeedRunSkill = false;

        if (PlayerPrefs.HasKey("SpeedRunSkillReceived") && PlayerPrefs.GetInt("SpeedRunSkillReceived") == 1)
        {
            hasReceivedSpeedRunSkill = true;
        }
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerBlocking = GetComponent<PlayerBlocking>();
    }

    private void Update()
    {
        if (isDashing || playerBlocking.isBlocking)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.F) && isGrounded())
        {
            if (horizontalInput != 0)
            {
                playerBlocking.StartBlocking();
            }
        }

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        rb2d.velocity = new Vector2(horizontalInput * speed, rb2d.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            Jump();
            SoundManager.instance.PlaySound(jumpSound);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb2d.velocity.y > 0)
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2);

        if (hasReceivedSpeedRunSkill)
        {
            UnlockDashWind();
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isFastRunning && isGrounded() && isDashWindReady)
            {
                StartCoroutine(Dash());
                anim.SetTrigger("fastRun");
                isFastRunning = true;

                if (dashPoint != null && dashWindPrefab != null)
                {
                    currentDashWind = Instantiate(dashWindPrefab, dashEffectHolder.position, dashEffectHolder.rotation, dashEffectHolder);
                    SoundManager.instance.PlaySound(dashwindSound);
                }
                StartCoroutine(DashWindCooldown());
            }
        }
    }

    private void Jump()
    {
        if (isGrounded() && !isDashing && isJumping)
        {
            jumpCounter = extraJumps;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else
        {
            if (jumpCounter > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
                jumpCounter--;
            }
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded();
    }

    public void UnlockDashWind()
    {
        hasReceivedSpeedRunSkill = true;
        canDash = true;
        PlayerPrefs.SetInt("SpeedRunSkillReceived", 1);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        isFastRunning = false;
        if (currentDashWind != null)
        {
            Destroy(currentDashWind);
        }
        canDash = true;
    }

    private IEnumerator DashWindCooldown()
    {
        isDashWindReady = false;
        yield return new WaitForSeconds(dashWindCooldown);
        isDashWindReady = true;
    }
}
