using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] public int damage;
    public bool HasLanded { get; set; }

    public float recoilForce = 10.0f;
    public float upwardForce = 5.0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        CheckCollision(other.gameObject);
    }

    private void CheckCollision(GameObject collider)
    {
        
            if (collider.CompareTag("Player"))
            {
                Health playerHealth = collider.GetComponent<Health>();
                if (playerHealth != null && HasLanded)
                {
                    playerHealth.TakeDamage(damage);

                    Rigidbody2D playerRigidbody = collider.GetComponent<Rigidbody2D>();
                    if (playerRigidbody != null)
                    {
                        Vector2 recoilDirection = (playerRigidbody.transform.position - transform.position).normalized;
                        Vector2 totalRecoilForce = new Vector2(recoilDirection.x, 1.0f) * recoilForce;
                        playerRigidbody.AddForce(totalRecoilForce, ForceMode2D.Impulse);
                    }
                }
            }
    }
}
