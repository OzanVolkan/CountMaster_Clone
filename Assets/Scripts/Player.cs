using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
public class Player : MonoBehaviour
{
    [SerializeField] Transform counterMarkTrans;
    [SerializeField] GameObject stickman;
    [SerializeField] TextMeshProUGUI childCounter;
    [SerializeField] int totalStickmen;
    [Range(0f, 1f)] [SerializeField] private float distance, radius;

    private Animator[] animators;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRunAnimation, new Action<bool>(OnRunAnimation));
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRunAnimation, new Action<bool>(OnRunAnimation));
    }
    void Start()
    {
        childCounter.text = CalculateCount().ToString();
        animators = GetComponentsInChildren<Animator>();
        InvokeRepeating("PositionChecker", 0.1f, 0.1f);
    }

    void Update()
    {

    }

    void OnRunAnimation(bool isMoving)
    {
        animators = GetComponentsInChildren<Animator>();

        foreach (Animator animator in animators)
        {
            if (animator != null)
            {
                animator.SetBool("Run", isMoving ? true : false);
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
        if (other.CompareTag("Lence"))
        {
            Lence lence = other.GetComponent<Lence>();

            int operationNum = other.GetComponent<Lence>().randomNum;

            switch (lence.lenceType)
            {
                case Lence.LenceType.Add:
                    EventManager.Broadcast(GameEvent.OnGenerateStickman, operationNum, stickman, transform, Quaternion.identity);
                    break;

                case Lence.LenceType.Multiply:
                    EventManager.Broadcast(GameEvent.OnGenerateStickman, operationNum * totalStickmen, stickman, transform, Quaternion.identity);
                    break;
            }
            OnRunAnimation(GameManager.Instance.isMoving);
            EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);
            //2 kere triggera girmemesi için buarada bir þeyler yaz
        }
        childCounter.text = CalculateCount().ToString();

    }

    private void PositionChecker()
    {
        EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);
        counterMarkTrans.rotation = Quaternion.LookRotation(counterMarkTrans.position - Camera.main.transform.position);
    }

}
