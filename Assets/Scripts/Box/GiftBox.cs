using UnityEngine;

public class GiftBox : MonoBehaviour
{
    [SerializeField] float Solidity;

    [Header("Broken Barrel Sound")]
    [SerializeField] private AudioClip boxSound;
    float currentHealth;

    public void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        if(currentHealth <= 0 ) 
        {
            Die();
        }
    }

    public void Die()
    {
        GetComponent<LootBag>().Instantiate(transform.position);
        Destroy(gameObject);
        SoundManager.instance.PlaySound(boxSound);
    }
}
