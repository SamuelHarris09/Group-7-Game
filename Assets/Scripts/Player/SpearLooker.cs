using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class SpearLooker : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    private float spearAttackDelay = 1f;
    private bool isAttacking = false;

    InputAction attackAction;
    public void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        mainCam = Camera.main;
    }

    private void Update()
    {
        MoveMouse();
    }

    public void MoveMouse()
    {
        if (attackAction.WasPerformedThisFrame() && !isAttacking)
        {
            isAttacking = true;

            mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            StartCoroutine(DelayAction(spearAttackDelay));
        }
    }

    IEnumerator DelayAction(float spearAttackDelay)
    {
        yield return new WaitForSeconds(spearAttackDelay);
        isAttacking = false;
    }
}