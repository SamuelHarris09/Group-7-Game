using UnityEngine;

public class AttackWarning : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private float flashSpeed = 10f;

    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 0.7f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * flashSpeed, 1f);

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        spriteRenderer.color = new Color(1f, 0f, 0f, alpha);
    }
}