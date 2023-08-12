using UnityEngine;

public class HealthBarForllowBoss : MonoBehaviour
{
    public Transform character;

    private void Update()
    {
        if (character != null)
        {
            Vector3 characterPosition = character.position + Vector3.up * 3f;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterPosition);

            transform.position = screenPosition;
        }
    }
}
