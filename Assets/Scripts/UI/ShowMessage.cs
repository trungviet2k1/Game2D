using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMessage : MonoBehaviour
{
    [Header("Message")]
    public TextMeshProUGUI messageText;

    private void Start()
    {
        messageText.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Message("Hold the switch to open the door", 3f));
        }
    }

    private IEnumerator Message(string message, float duration)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        yield return new WaitForSeconds(duration);

        messageText.gameObject.SetActive(false);
    }
}