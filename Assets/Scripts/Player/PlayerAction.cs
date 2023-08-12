using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GameObject coinNumPrefab;
    private CoinsManager coinsManager;

    private void Start()
    {
        coinsManager = GetComponent<CoinsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if player collides with a coin
        if (collision.tag == "Player")
        {
            //Add coins 1
            //collision.GetComponent<CoinsManager>().AddCoins(collision.transform.position, 1);

            gameObject.SetActive(false);

            //Show (+1) number
            Destroy(Instantiate(coinNumPrefab, collision.transform.position, Quaternion.identity), 1);
        }
    }
}
