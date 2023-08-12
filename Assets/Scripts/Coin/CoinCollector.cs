using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CoinCollector : MonoBehaviour
{
    [Header("Minus the coin")]
    [SerializeField] private int coinsToSubtractOnDeath = 5;

    public TextMeshProUGUI coinText;
    private int coinCount;
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        coinCount = PlayerPrefs.GetInt("CoinCollected", coinCount);
        coinText.text = coinCount.ToString() + "$";
    }

    private void Update()
    {
        PlayerPrefs.SetInt("CoinCollected", coinCount);
    }

    public int CoinsToSubtractOnDeath
    {
        get { return coinsToSubtractOnDeath; }
    }

    public void collectedCoin()
    {
        coinCount++;
    }

    public int GetCollectedCoins()
    {
        return coinCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            SoundManager.instance.PlaySound(pickupSound);
            Destroy(collision.gameObject);
            coinCount = coinCount + 1;
            PlayerPrefs.SetInt("CoinCollected", coinCount);
            coinText.text = coinCount.ToString() + "$";
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CoinCollected", 0);
    }
}