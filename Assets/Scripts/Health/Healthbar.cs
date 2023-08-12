using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthbarCurrent;

    public void UpdateBar(float healthCurrent, float healthTotal)
    {
        healthbarCurrent.fillAmount = healthCurrent / healthTotal;
    }
}
