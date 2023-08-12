using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorUp : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 offset;

    [Header("Door")]
    public Transform door;
    public int doorIndex;

    [Header("SFX")]
    [SerializeField] private AudioClip openSound;

    [Header("Key")]
    public KeyCollector keyCollector;

    [Header("Message")]
    public TextMeshProUGUI messageText;

    private bool movingUp;
    private float upEdge;

    private void Start()
    {
        messageText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        upEdge = transform.position.y + movementDistance;
    }

    private void Update()
    {
        if (movingUp)
        {
            if (transform.position.y < upEdge)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                SoundManager.instance.PlaySound(openSound);
            }
            else
            {
                movingUp = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KeyCollector keyCollector = FindObjectOfType<KeyCollector>();
            if (keyCollector.HasGreenKey(doorIndex))
            {
                StartCoroutine(MoveUpCoroutine());
                keyCollector.ConsumeGreenKey(doorIndex);
            }
            else
            {
                StartCoroutine(ShowMessage("You need the Green Key to open the gate", 3f));
            }
        }
    }

    private IEnumerator ShowMessage(string message, float duration)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        yield return new WaitForSeconds(duration);

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator MoveUpCoroutine()
    {
        movingUp = true;
        upEdge = transform.position.y + movementDistance;

        while (transform.position.y < upEdge)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        movingUp = false;
    }
}
