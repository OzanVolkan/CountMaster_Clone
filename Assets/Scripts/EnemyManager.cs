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
    [Range(0f, 1f)] [SerializeField] private float distance, radius;

    void Start()
    {
        int randomCount = Random.Range(20, 81);
        Quaternion enemyRot = new Quaternion(0f, 180f, 0f, 1f);

        EventManager.Broadcast(GameEvent.OnGenerateStickman, randomCount, enemyStickman, transform,enemyRot);

        childCounter.text = CalculateCount().ToString();
        InvokeRepeating("PositionChecker", 0.1f, 0.1f);
    }

    void Update()
    {
        
    }

    int CalculateCount()
    {
        totalStickmen = transform.childCount - 1;
        return totalStickmen;
    }
    private void PositionChecker()
    {
        EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);
        counterMarkTrans.rotation = Quaternion.LookRotation(counterMarkTrans.position - Camera.main.transform.position);
    }
}
