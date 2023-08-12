using UnityEngine;
using Cainos.LucidEditor;
using TMPro;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Reference")]
        public GameObject speedRunItemPrefab;

        [FoldoutGroup("Reference")]
        public AudioClip collectSound;

        [FoldoutGroup("Reference")]
        public Transform playerTransform;

        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;

        private const string ChestKey = "IsChestOpened";

        public void Open()
        {
            IsOpened = true;
            PlayerPrefs.SetInt(ChestKey, 1);
            PlayerPrefs.Save();

            GameObject speedRunItem = Instantiate(speedRunItemPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);

            SpeedRunItemController itemController = speedRunItem.GetComponent<SpeedRunItemController>();
            if (itemController != null)
            {
                itemController.SetTarget(playerTransform);
            }

            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.UnlockDashWind();
            }
        }

        [FoldoutGroup("Reference")]
        public GameObject interactTextObject;
        private TextMeshProUGUI interactText;
        private bool isPlayerInRange = false;

        private void Start()
        {
            interactText = interactTextObject.GetComponent<TextMeshProUGUI>();
            interactTextObject.SetActive(false);
            IsOpened = PlayerPrefs.GetInt(ChestKey, 0) == 1;
        }

        private void Update()
        {
            if (isPlayerInRange && !IsOpened)
            {
                interactTextObject.SetActive(true);
                interactText.text = "Press 'E' to open the chest";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Open();
                    SoundManager.instance.PlaySound(collectSound);
                }
            }
            else
            {
                interactTextObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }
    }
}
