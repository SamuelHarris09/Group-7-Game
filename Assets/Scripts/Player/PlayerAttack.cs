using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject spear;
    [SerializeField] GameObject PlacedSpear;
    [SerializeField] float spearActiveTime = 0.5f;

    [Header("Bools")]
    public bool hasSpear = false;
    private bool canAttack = true;

    InputAction attackAction;

    public void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        spear.SetActive(false);
    }

    private void Update()
    {
        CheckScene();
        SpearAttack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerIndex = LayerMask.NameToLayer("PlacedSpear");

        if (other.gameObject.layer == layerIndex)
        {
            hasSpear = true;
            GameObject.Destroy(PlacedSpear);
        }
    }

    void CheckScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= 3)
        {
            hasSpear = true;
        }
    }

    public void SpearAttack()
    {
        if (attackAction.WasPressedThisFrame() && hasSpear && canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;

        spear.SetActive(true);

        yield return new WaitForSeconds(spearActiveTime);

        spear.SetActive(false);

        canAttack = true;
    }
}