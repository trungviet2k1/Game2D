using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Door")]
    public Transform door;

    [Header("SFX")]
    [SerializeField] private AudioClip openSound;

    public float doorOpenOffset = 2f;
    [SerializeField] private float speed;

    private bool movingDown = false;
    private bool playerIsOnButton = false;
    private int boxCount = 0;
    private Vector3 targetPosition;
    private Vector3 initialPosition;

    private void Awake()
    {
        targetPosition = door.position - new Vector3(0f, doorOpenOffset, 0f);
        initialPosition = door.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerIsOnButton = true;
            }
            if (collision.gameObject.CompareTag("Box"))
            {
                boxCount++;
            }
            if (!movingDown && (playerIsOnButton || boxCount > 0))
            {
                StartCoroutine(MoveDoorDown());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerIsOnButton = false;
            }
            if (collision.gameObject.CompareTag("Box"))
            {
                boxCount--;
            }
            if (!playerIsOnButton && boxCount == 0)
            {
                StartCoroutine(MoveDoorUp());
            }
        }
    }

    private IEnumerator MoveDoorDown()
    {
        movingDown = true;
        while (door.position.y > targetPosition.y && (playerIsOnButton || boxCount > 0))
        {
            door.position = Vector3.MoveTowards(door.position, targetPosition, speed * Time.deltaTime);
            SoundManager.instance.PlaySound(openSound);
            yield return null;
        }
        movingDown = false;
    }

    private IEnumerator MoveDoorUp()
    {
        while (door.position.y < initialPosition.y)
        {
            door.position = Vector3.MoveTowards(door.position, initialPosition, speed * Time.deltaTime);
            SoundManager.instance.PlaySound(openSound);
            yield return null;
        }
    }
}
