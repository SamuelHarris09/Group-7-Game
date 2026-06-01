using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformFallThrough : MonoBehaviour
{
    [SerializeField] float fallCoolDown = 0.5f;

    private Collider2D collider2d;

    private void Start()
    {
        collider2d = GetComponent<Collider2D>();
    }
    private void Update()
    {
        StartCoroutine(PressFall());
    }

    IEnumerator PressFall()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            collider2d.enabled = false;
        }
        else if (Keyboard.current.sKey.wasReleasedThisFrame)
        {
            yield return new WaitForSeconds(fallCoolDown);
            collider2d.enabled = true;
        }
    }
}