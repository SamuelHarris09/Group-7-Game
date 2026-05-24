using System.Collections;
using UnityEngine;

public class BatAtatck : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform[] attackWayPoints;

    [SerializeField] float attackSpeed = 5f;
    [SerializeField] public bool canAttack = true;
    int attackPointIndex = 0;

    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Transform>();
        canAttack = true;

        StartCoroutine(SwitchToIdle());
    }

    void Update()
    {
        BatAttackCheck();
    }

    IEnumerator SwitchToIdle()
    {
        yield return new WaitForSeconds(2f);

        animator.SetBool("isIdle", true);
    }

    void BatAttackCheck()
    {
        if(canAttack == true)
        {
            StartCoroutine(attackPlayer());
        }
    }

    IEnumerator attackPlayer()
    {
        canAttack = false;
        //graah Im Gonna Attack You graaaaahhhh
        if (attackPointIndex < attackWayPoints.Length)
        {
            //now i gon aa ttack y uo
            Vector3 targetPosition = attackWayPoints[attackPointIndex].position;
            float moveDelta = attackSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDelta);

            if (transform.position == targetPosition)
            {
                attackPointIndex++;
            }
            if (attackPointIndex == attackWayPoints.Length)
            {
                attackPointIndex = 0;
                yield return new WaitForSeconds(1);
            }
        }
        canAttack = true;
        yield return null;
    }
}