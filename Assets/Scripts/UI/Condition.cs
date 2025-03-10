using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float passiveValue;

    private float _minValue = 0;
    public float maxValue;

    public Image uiBar;

    public event Action onTakeDamage;

    private void Start()
    {
        curValue = startValue;
    }
    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    protected float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Substract(float value)
    {
        curValue = Mathf.Max(curValue - value, _minValue);
    }

    public IEnumerator CoroutineAdd(float value, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            curValue = Mathf.Min(curValue + value * Time.deltaTime, maxValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public virtual void TakeOnDamage(float value)
    {
        Substract(value);
        onTakeDamage?.Invoke();
    }
}
