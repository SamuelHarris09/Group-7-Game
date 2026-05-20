using UnityEngine;

public class AttackWarning : MonoBehaviour
{
    [SerializeField] float duration = 0.6f;
    [SerializeField] float flashSpeed = 10f;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float alpha = Mathf.Lerp(0.2f, 0.7f, Mathf.PingPong(Time.time * flashSpeed, 1));

        spriteRenderer.color = new Color(1f, 0f, 0f, alpha);
    }

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}