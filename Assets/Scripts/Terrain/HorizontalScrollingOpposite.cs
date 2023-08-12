using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalScrollingOpposite : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;

    private bool movingRight;
    private float LeftEdge;
    private float RightEdge;

    private void Awake()
    {
        LeftEdge = transform.position.x - movementDistance;
        RightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        if (movingRight)
        {
            if (transform.position.x < RightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingRight = false;
        }
        else
        {
            if (transform.position.x > LeftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}