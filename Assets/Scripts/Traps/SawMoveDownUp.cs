using UnityEngine;

public class SawMoveDownUp : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private bool movingDown;
    private float upEdge;
    private float downEdge;

    private void Awake()
    {
        upEdge = transform.position.y + movementDistance;
        downEdge = transform.position.y - movementDistance; ;
    }

    private void Update()
    {
        if (movingDown)
        {
            if (transform.position.y > downEdge)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            else
            {
                movingDown = false;
            }
        }
        else
        {
            if (transform.position.y < upEdge)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            else
            {
                movingDown = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
