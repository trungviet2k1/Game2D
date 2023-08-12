using UnityEngine;

public class PlayerBlocking : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] public float knockbackDistance = 1.0f;

    public bool isBlocking { get; private set; } = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            StartBlocking();
        }
        else
        {
            StopBlocking();
        }
    }

    public void StartBlocking()
    {
        if (!isBlocking)
        {
            isBlocking = true;
            anim.SetBool("block", true);
        }
    }

    public void StopBlocking()
    {
        isBlocking = false;
        anim.SetBool("block", false);
    }
}
