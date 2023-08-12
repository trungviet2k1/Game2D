using System.Collections;
using UnityEngine;
public class SpeedRunItemController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attractSpeed = 4f;
    public Transform target;
    private bool isAttracted = false;
    private Vector3 initialPosition;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isAttracted = false;
        StartCoroutine(WaitAndAttract());
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (!isAttracted)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition + Vector3.up * 1.5f, moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, initialPosition + Vector3.up * 2.5f) < 0.1f)
            {
                isAttracted = true;
            }

            PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.UnlockDashWind();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, attractSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator WaitAndAttract()
    {
        yield return new WaitForSeconds(1f);
        isAttracted = true;
    }
}
