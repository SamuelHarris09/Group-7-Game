using System;
using UnityEngine;

public class WeakPointVisual : MonoBehaviour
{
    [SerializeField] private Color inactiveColor = new(1, 1, 1, 0.2f);
    [SerializeField] private Color activeColor = Color.red;

    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseSize = 0.15f;

    Vector3 baseScale;

    bool active;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        baseScale = transform.localScale;
    }

    void Update()
    {
        if (active)
        {
            float pulse = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseSpeed;

            transform.localPosition = baseScale * pulse;

            spriteRenderer.color = Color.Lerp(inactiveColor, activeColor, Mathf.PingPong(Time.time * pulseSpeed, 1));
        }
        else
        {
            transform.localPosition = baseScale;
            spriteRenderer.color = inactiveColor;
        }
    }

    public void SetActive(bool state)
    {
        active = state;
    }
}