using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform counterMarkTrans;
    [SerializeField] GameObject enemyStickman;
    [SerializeField] TextMeshProUGUI childCounter;
    [SerializeField] int totalStickmen;
    [Range(0f, 1f)] [SerializeField] float distance, radius;

    private Animator[] animators;
    private Transform playerTransform;
    private bool canAttack;
    private float rotateSpeed = 3f;

    void Start()
    {
        int randomCount = Random.Range(20, 51);
        Quaternion enemyRot = new Quaternion(0f, 180f, 0f, 1f);

        EventManager.Broadcast(GameEvent.OnGenerateStickman, 0, randomCount, enemyStickman, transform, enemyRot);
        EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);

        childCounter.text = CalculateCount().ToString();
        InvokeRepeating("PositionChecker", 0.1f, 0.1f);

        animators = GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        if (canAttack)
        {
            Vector3 playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(playerDirection, Vector3.up), Time.deltaTime * rotateSpeed);
            }

            if (playerTransform.childCount > 1)
            {

                for (int i = 1; i < transform.childCount; i++)
                {
                    var Distance = playerTransform.position - transform.GetChild(i).position;

                    if (Distance.magnitude < 2.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(playerTransform.GetChild(1).position.x, transform.GetChild(i).position.y,
                                playerTransform.GetChild(1).position.z), Time.deltaTime);
                    }
                }
            }

            childCounter.text = CalculateCount().ToString();
        }
    }

    int CalculateCount()
    {
        totalStickmen = transform.childCount - 1;
        return totalStickmen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            canAttack = true;

            foreach (Animator animator in animators)
            {
                if (animator != null)
                {
                    animator.SetBool("Run", canAttack ? true : false);
                }
            }
        }

        //childCounter.text = CalculateCount().ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;

            if (playerTransform.childCount <= 1) StopAttacking();
        }
    }
    private void StopAttacking()
    {
        canAttack = false;

        foreach (Animator animator in animators)
        {
            if (animator != null)
            {
                animator.SetBool("Run", canAttack ? true : false);
            }
        }
    }
    private void PositionChecker()
    {
        counterMarkTrans.rotation = Quaternion.LookRotation(counterMarkTrans.position - Camera.main.transform.position);
    }
}
