using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [Header ("SFX")]
    [SerializeField] private AudioClip nextLevelSound;
    private bool levelComplete = false;

    [Header ("Key")]
    public KeyCollector keyCollector;

    [Header ("Message")]
    public TextMeshProUGUI messageText;
    
    [Header ("Animation")]
    public Animator playerAnimator;
    private Animator anim;

    private void Start()
    {
        messageText.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !levelComplete && keyCollector.HasGoldKey())
        {
            SoundManager.instance.PlaySound(nextLevelSound);
            levelComplete = true;
            Invoke("CompleteLevel", 1f);
        }
        else if(collision.tag == "Player" && !keyCollector.HasGoldKey())
        {
            StartCoroutine(ShowMessage("You need the Gold Key to open the gate", 3f));
            return;
        }

        if (collision.tag == "Player")
        {
            anim.SetTrigger("Open");
        }
    }

    private IEnumerator ShowMessage(string message, float duration)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        yield return new WaitForSeconds(duration);

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator ChangeToNextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void CompleteLevel()
    {
        UnLockNewLevel();
        playerAnimator.SetTrigger("win");
        StartCoroutine(ChangeToNextScene());
    }

    void UnLockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachIndex"))
        {
            PlayerPrefs.SetInt("ReachIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
