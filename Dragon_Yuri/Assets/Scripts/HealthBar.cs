using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image bar;

    private void Start()
    {
        bar.fillAmount = 1f;
    }

    public void UpdateHealthBar(float health)
    {
        bar.fillAmount = health;
        bar.color = health switch
        {
            > 0.5f => Color.green,
            > 0.1f => Color.yellow,
            _ => Color.red
        };
    }
}
