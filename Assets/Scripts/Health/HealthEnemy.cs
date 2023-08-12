using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthEnemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float startingHealth;
    public Healthbar healthbar;
    public Image healthBarTotal;
    float currentHealth;
    private bool dead;

    private Animator anim;

    [Header("Iframes")]
    [SerializeField] private float iFramesDuraction;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRenderer;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    public bool invulnerable;

    [Header("SFX")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        healthbar.UpdateBar(currentHealth, startingHealth);
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            if (!dead)
            {
                anim.SetTrigger("die");

                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
        else
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }

        healthbar.UpdateBar(currentHealth, startingHealth);
    }

    public void AddHealth(float _value)
    {
        currentHealth += _value;
        healthbar.UpdateBar(currentHealth, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuraction / (numberOfFlashes * 4));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuraction / (numberOfFlashes * 4));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        GetComponent<LootBag>().Instantiate(transform.position);
        gameObject.SetActive(false);
        healthBarTotal.enabled = false;
    }
}
