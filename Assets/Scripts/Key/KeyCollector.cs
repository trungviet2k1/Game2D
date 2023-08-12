using TMPro;
using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    public TextMeshProUGUI GoldKeyText;
    public TextMeshProUGUI GreenKeyText;
    private int GoldKeyCount = 0;
    private int[] GreenKeyCount;
    private bool hasGoldKey = false;
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        int doorCount = GameObject.FindGameObjectsWithTag("Door").Length;
        GreenKeyCount = new int[doorCount];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoldKey")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            GoldKeyCount++;
            GoldKeyText.text = GoldKeyCount.ToString();
            CollectGoldKey();
        }

        if (collision.gameObject.tag == "GreenKey1")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            CollectGreenKey(0);
        }
        else if (collision.gameObject.tag == "GreenKey2")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            CollectGreenKey(1);
        }
        else if (collision.gameObject.tag == "GreenKey3")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            CollectGreenKey(2);
        }
        else if (collision.gameObject.tag == "GreenKey4")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            CollectGreenKey(3);
        }
        else if (collision.gameObject.tag == "GreenKey5")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            CollectGreenKey(4);
        }
        else if (collision.gameObject.tag == "GreenKey6")
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            CollectGreenKey(5);
        }
    }
    public void CollectGoldKey()
    {
        hasGoldKey = true;
    }

    private void CollectGreenKey(int doorIndex)
    {
        GreenKeyCount[doorIndex]++;
        GreenKeyText.text = GreenKeyCount[doorIndex].ToString();
    }

    public bool HasGoldKey()
    {
        return hasGoldKey;
    }

    public bool HasGreenKey(int doorIndex)
    {
        return GreenKeyCount[doorIndex] > 0;
    }

    public void ConsumeGreenKey(int doorIndex)
    {
        GreenKeyCount[doorIndex]--;
        GreenKeyText.text = GreenKeyCount[doorIndex].ToString();
    }
}
