using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public Healthbar healthbar;
    public Image healthBarTotal;
    public float currentHealth { get; private set; }
    private bool dead;

    private Animator anim;
    private UIManager uiManager;
    private PlayerBlocking playerBlocking;
    private int deathCount = 0;

    [Header("Iframes")]
    [SerializeField] private float iFramesDuraction;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRenderer;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("SFX")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip blockSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        healthbar.UpdateBar(currentHealth, startingHealth);
        anim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBlocking = GetComponent<PlayerBlocking>();
    }

    public void TakeDamage(float damage)
    {
        if (playerBlocking.isBlocking)
        {
            SoundManager.instance.PlaySound(blockSound);
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (!dead)
            {
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");

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

    public void AddHealth(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, startingHealth);
        healthbar.UpdateBar(currentHealth, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuraction / (numberOfFlashes * 4));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuraction / (numberOfFlashes * 4));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        healthBarTotal.enabled = false;
    }
    public void Respawn()
    {
        if (deathCount > 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            deathCount++;

            currentHealth = startingHealth;
            healthbar.UpdateBar(currentHealth, startingHealth);
            anim.ResetTrigger("die");
            anim.Play("Idle");
            StartCoroutine(Invunerability());
            dead = false;

            foreach (Behaviour component in components)
                component.enabled = true;
        }
    }
}