using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private float popupDur = .5f;

    private void Start()
    {
        bar.fillAmount = 1f;
        DisplayToggle(false);
    }

    private float _currentTimer;
    private Coroutine _activeRoutine;

    public void UpdateHealthBar(float health)
    {
        bar.fillAmount = health;
        bar.color = health switch { 
            > 0.5f => Color.green,
            > 0.1f => Color.yellow,
            _ => Color.red 
        };

        _currentTimer = popupDur;
        _activeRoutine ??= StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        DisplayToggle(true);

        while (_currentTimer > 0)
        {
            _currentTimer -= Time.deltaTime;
            yield return null;
        }

        DisplayToggle(false);
        _activeRoutine = null; 
    }

    private void DisplayToggle(bool toggle)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(toggle);
        }
    }
}

