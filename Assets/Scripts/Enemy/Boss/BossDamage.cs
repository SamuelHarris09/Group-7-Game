using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] bool changed;
    [SerializeField] private Sprite bloodyHand;
    [SerializeField] private Sprite cleanHand;

    Health playerHealth;
    BossMovement bossMovement;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<Health>();
        bossMovement = FindFirstObjectByType<BossMovement>();

        spriteRenderer = FindFirstObjectByType<SpriteRenderer>();

        damage = (int)playerHealth.maxHealth / 10;

        changed = false;
    }

    private void Update()
    {
        if (bossMovement.reset == true)
        {
            cleanHand = spriteRenderer.sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health player = other.GetComponent<Health>();

        if (player != null)
        {
            if (changed == false)
            {
                Debug.Log("yes");
                bloodyHand = spriteRenderer.sprite;
                changed = true;
            }
        }
    }
}