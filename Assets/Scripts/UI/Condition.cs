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

    public Coroutine addCoroutine;

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

    public void StartCorouinteAdd(float value, float time)
    {
        StartCoroutine(CoroutineAdd(value, time));
    }
    public IEnumerator CoroutineAdd(float value, float time)
    {
        float elapsedTime = 0f;
        Debug.Log($"[Coroutine Start] Target Value: {value}, Duration: {time}");
        while (elapsedTime <= time)
        {
            curValue = Mathf.Min(curValue + value * Time.deltaTime, maxValue);
            elapsedTime += Time.deltaTime;
            Debug.Log($"ElapsedTime: {elapsedTime:F2}");
            yield return null;
        }
        Debug.Log("[Coroutine Finished]");
    }

    public virtual void TakeOnDamage(float value)
    {
        Substract(value);
        onTakeDamage?.Invoke();
    }
}
