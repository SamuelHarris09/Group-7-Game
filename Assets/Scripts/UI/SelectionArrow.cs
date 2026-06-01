using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;

    [Header("Music & SFX")]
    [SerializeField] AudioClip changeSound;
    [SerializeField, Range(0, 1)] float changeVolume;

    [SerializeField] AudioClip interactSound;
    [SerializeField, Range(0, 1)] float interactVolume;

    private RectTransform rect;
    private int currentPosition;

    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            ChangePosition(-1);
        }

        if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            ChangePosition(1);
        }
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
        {
            //AudioSource.PlayClipAtPoint(changeSound, transform.position, changeVolume);
        }

        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition > options.Length - 1)
        {
            currentPosition = 0;
        }

        // the position of the arrow
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }
}