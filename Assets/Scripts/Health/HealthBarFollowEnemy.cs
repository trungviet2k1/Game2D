using UnityEngine;

public class HealthBarFollowEnemy : MonoBehaviour
{
    public Transform character;

    private void Update()
    {
        if (character != null)
        {
            Vector3 characterPosition = character.position + Vector3.up * 2.2f;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterPosition);

            transform.position = screenPosition;
        }
    }
}
