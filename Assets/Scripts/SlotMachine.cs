using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    #region Data Class
    public enum PowerUpType
    {
        Speed,
        Health,
        Slowness,
        DashLength,
        DashSpeed
    }
    [System.Serializable]
    public class SlotSymbol
    {
        public Sprite sprite;
        public int value;
    }

    [System.Serializable]
    public class RewardTier
    {
        public int minValue;
        public int maxValue;
        public PowerUp powerUp;
    }

    [System.Serializable]
    public class PowerUp
    {
        public string powerUpName;
        public PowerUpType type;
        public float amount;
        public string description;
        public Sprite icon;
    }
    #endregion
    #region Inspector
    [Header("Slot Machine Data")]
    [SerializeField] private SlotSymbol[] symbols;
    [SerializeField] private RewardTier[] rewards;

    [Header("Slot Visuals")]
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] spinSprites;
    [HideInInspector] Sprite[] slotSymbol;

    [Header("Reward UI")]
    [SerializeField] private Image powerUpIcon;
    [SerializeField] private TMP_Text powerUpText;

    [Header("Spin Settings")]
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float[] spinDurations = { 1.5f, 2f, 2.5f };
    #endregion
    #region State
    private bool playerIsNear;
    private int[] spriteSlot = new int[3];
    private SlotSymbol[] finalResults = new SlotSymbol[3];

    private InputAction interactAction;

    private ButtonBehavior buttonBehavior;
    private PlayerMovement player;
    private Health playerHealth;
    #endregion
    #region Unity
    private void Start()
    {
        for (int i = 0; i < finalResults.Length; i++)
        {
            finalResults[i] = null;
        }

        interactAction = InputSystem.actions.FindAction("Interact");

        buttonBehavior = FindAnyObjectByType<ButtonBehavior>();
        player = FindFirstObjectByType<PlayerMovement>();
        playerHealth = FindFirstObjectByType<Health>();

        slotSymbol = new Sprite[3];
    }

    private void Update()
    {
        OpenGamblingMachine();
    }
    #endregion
    #region Animation
    public IEnumerator SpinRoutine()
    {
        StartCoroutine(SpinAnimation(0, spinDurations[0]));
        StartCoroutine(SpinAnimation(1, spinDurations[1]));
        StartCoroutine(SpinAnimation(2, spinDurations[2]));

        yield return new WaitForSecondsRealtime(Mathf.Max(spinDurations));

        Spin();
    }

    private void AnimateSlot(int slotIndex)
    {
        spriteSlot[slotIndex]++;

        if (spriteSlot[slotIndex] >= spinSprites.Length)
        {
            spriteSlot[slotIndex] = 0;
        }

        slotImages[slotIndex].sprite = spinSprites[spriteSlot[slotIndex]];
    }

    IEnumerator SpinAnimation(int slotIndex, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            AnimateSlot(slotIndex);

            yield return new WaitForSecondsRealtime(speed);

            timer += speed;
        }

        SlotSymbol result = symbols[Random.Range(0, symbols.Length)];

        finalResults[slotIndex] = result;

        slotImages[slotIndex].sprite = result.sprite;
    }

    void Spin()
    {
        if (finalResults[0] == null || finalResults[1] == null || finalResults[2] == null)
        {
            return;
        }

        SlotSymbol s1 = finalResults[0];
        SlotSymbol s2 = finalResults[1];
        SlotSymbol s3 = finalResults[2];

        int totalValue = s1.value + s2.value + s3.value;

        RewardTier reward = GetRewardTier(totalValue);

        if (reward != null && reward.powerUp != null)
        {
            ApplyPowerUp(reward.powerUp);

            ShowPopup(reward.powerUp);
        }

        slotSymbol[0] = s1.sprite;
        slotSymbol[1] = s2.sprite;
        slotSymbol[2] = s3.sprite;
    }
    #endregion
    #region Status Affects
    RewardTier GetRewardTier(int totalValue)
    {
        foreach (RewardTier tiers in rewards)
        {
            if (totalValue >= tiers.minValue && totalValue <= tiers.maxValue)
            {
                return tiers;
            }
        }

        return null;
    }

    private void ApplyPowerUp(PowerUp powerUp)
    {
        switch (powerUp.type)
        {
            case PowerUpType.Speed:
                player.currentSpeed *= powerUp.amount;
                break;

            case PowerUpType.Health:
                playerHealth.currentHealth += powerUp.amount;
                break;

            case PowerUpType.DashSpeed:
                player.dashSpeed *= powerUp.amount;
                break;

            case PowerUpType.DashLength:
                player.dashDistance += powerUp.amount;
                break;

            case PowerUpType.Slowness:
                player.currentSpeed *= powerUp.amount;
                break;
        }
    }

    void ShowPopup(PowerUp powerup)
    {
        powerUpIcon.enabled = true;

        powerUpIcon.sprite = powerup.icon;

        powerUpText.text = powerup.powerUpName + "\n" + powerup.description;
    }
    #endregion
    #region Interaction
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    void OpenGamblingMachine()
    {
        if (playerIsNear && interactAction != null && interactAction.WasPressedThisFrame())
        {
            buttonBehavior.SlotMachineMenuOn();
        }
    }
    #endregion
}