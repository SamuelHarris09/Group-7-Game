using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeakPointVisual : MonoBehaviour
{
    [SerializeField] private Color inactiveColor = new(1, 1, 1, 0.2f);
    [SerializeField] private Color activeColor = Color.red;

    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseSize = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;

    private bool active;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;

        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (!active)
            return;

        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseSize;

        transform.localScale = baseScale * pulse;

        spriteRenderer.color = Color.Lerp(inactiveColor, activeColor, Mathf.PingPong(Time.time * pulseSpeed, 1f));
    }

    public void SetActive(bool state)
    {
        active = state;

        spriteRenderer.enabled = state;

        if (!state)
        {
            transform.localScale = baseScale;
        }
    }
}