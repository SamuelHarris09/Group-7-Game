using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BatAtatck : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform[] attackWayPoints;

    [SerializeField] float attackSpeed = 5f;
    [SerializeField] public bool canAttack = true;
    int attackPointIndex = 0;


    void Start()
    {
        player = GetComponent<Transform>();
        canAttack = true;
    }

    void Update()
    {
        batAttackCheck();
    }

    void batAttackCheck()
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
