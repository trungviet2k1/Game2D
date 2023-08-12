using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    [SerializeField] private float damageSaw;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damageSaw);
        }
    }
}
