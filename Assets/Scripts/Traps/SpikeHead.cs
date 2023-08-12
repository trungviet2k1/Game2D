using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;

    [Header("Movement Range")]
    [SerializeField] private float minXLimit;
    [SerializeField] private float maxXLimit;
    [SerializeField] private float minYLimit;
    [SerializeField] private float maxYLimit;

    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;

    [Header("SFX")]
    [SerializeField] private AudioClip spikeSound;
    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        if (attacking) 
        {
            transform.Translate(destination * Time.deltaTime * speed);
            float clampedX = Mathf.Clamp(transform.position.x, minXLimit, maxXLimit);
            float clampedY = Mathf.Clamp(transform.position.y, minYLimit, maxYLimit);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    public void CheckForPlayer()
    {
        CalculateDirections();
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    public void CalculateDirections()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void Stop()
    {
        destination = transform.position;
        attacking = false;
    }
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(spikeSound);
        base.OnTriggerEnter2D(collision);
        Stop();
    }
}
