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

    private Transform playerTransform;
    private bool canAttack;

    void Start()
    {
        int randomCount = Random.Range(20, 81);
        Quaternion enemyRot = new Quaternion(0f, 180f, 0f, 1f);

        EventManager.Broadcast(GameEvent.OnGenerateStickman, randomCount, enemyStickman, transform,enemyRot);
        EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);

        childCounter.text = CalculateCount().ToString();
        InvokeRepeating("PositionChecker", 0.1f, 0.1f);
    }

    void Update()
    {
        if (canAttack)
        {
            Vector3 playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(playerDirection, Vector3.up), Time.deltaTime);
            }

            if (playerTransform.childCount > 1)
            {
                print("dsf");

                for (int i = 1; i < transform.childCount; i++)
                {
                    var Distance = playerTransform.position - transform.GetChild(i).position;

                    if (Distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(playerTransform.GetChild(1).position.x, transform.GetChild(i).position.y,
                                playerTransform.GetChild(1).position.z), Time.deltaTime);
                    }
                }
            }
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

        }

        childCounter.text = CalculateCount().ToString();
    }
    private void PositionChecker()
    {
        counterMarkTrans.rotation = Quaternion.LookRotation(counterMarkTrans.position - Camera.main.transform.position);
    }
}
