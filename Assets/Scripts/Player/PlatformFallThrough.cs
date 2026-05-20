using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformFallThrough : MonoBehaviour
{
    [SerializeField] float fallCoolDown = 0.5f;

    private void Update()
    {
        StartCoroutine(PressFall());
    }

    IEnumerator PressFall()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else if (Keyboard.current.sKey.wasReleasedThisFrame)
        {
            yield return new WaitForSeconds(fallCoolDown);
            GetComponent<Collider2D>().enabled = true;
        }
    }
}