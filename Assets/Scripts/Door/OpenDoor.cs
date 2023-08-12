using System.Collections;
using TMPro;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 offset;

    [Header("Door")]
    public Transform door;
    public int doorIndex;

    [Header("Key")]
    public KeyCollector keyCollector;

    [Header("SFX")]
    [SerializeField] private AudioClip openSound;

    [Header("Message")]
    public TextMeshProUGUI messageText;

    private bool movingDown;
    private float downEdge;

    private void Start()
    {
        messageText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        downEdge = transform.position.y - movementDistance;
    }

    private void Update()
    {
        if (movingDown)
        {
            if (transform.position.y > downEdge)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                SoundManager.instance.PlaySound(openSound);
            }
            else
            {
                movingDown = false;
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
                StartCoroutine(MoveDownCoroutine());
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

    private IEnumerator MoveDownCoroutine()
    {
        movingDown = true;
        downEdge = transform.position.y - movementDistance;

        while (transform.position.y > downEdge)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        movingDown = false;
    }
}