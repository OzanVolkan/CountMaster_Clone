using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
public class Player : MonoBehaviour
{
    public Animator[] animators;

    [SerializeField] Transform counterMarkTrans;
    [SerializeField] Transform enemyTransform;
    [SerializeField] GameObject stickman;
    [SerializeField] TextMeshProUGUI childCounter;
    [SerializeField] int totalStickmen;
    [Range(0f, 1f)] [SerializeField] float distance, radius;

    private float rotateSpeed = 3f;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRunAnimation, new Action<bool, Animator[]>(OnRunAnimation));
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRunAnimation, new Action<bool, Animator[]>(OnRunAnimation));
    }
    void Start()
    {
        childCounter.text = CalculateCount().ToString();
        animators = GetComponentsInChildren<Animator>();
        InvokeRepeating("PositionChecker", 0.1f, 0.1f);
    }

    void Update()
    {
        if (GameManager.Instance.isAttacking)
        {
            Vector3 enemyDirection = new Vector3(enemyTransform.position.x, transform.position.y, enemyTransform.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * rotateSpeed);
            }

            if (enemyTransform.childCount > 1)
            {
                for (int i = 1; i < transform.childCount; i++)
                {
                    var Distance = enemyTransform.position - transform.GetChild(i).position;

                    if (Distance.magnitude < 2.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(enemyTransform.GetChild(1).position.x, transform.GetChild(i).position.y,
                                enemyTransform.GetChild(1).position.z), Time.deltaTime);
                    }
                }

                if (transform.childCount <= 1)
                {
                    EventManager.Broadcast(GameEvent.OnFail);
                }
            }
            else
            {
                SpriteRenderer area = enemyTransform.parent.GetChild(1).GetComponent<SpriteRenderer>();
                Color noAlpha = new Color(area.color.r, area.color.g, area.color.b, 0f);
                area.DOColor(noAlpha,0.4f);

                Vector3 scaledArea = new Vector3(0.4f, 0.4f, 0.4f);
                enemyTransform.parent.DOScale(scaledArea, 0.4f).OnComplete(()=> {
                    enemyTransform.parent.gameObject.SetActive(false);
                });

                GameManager.Instance.isAttacking = false;
                EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);

                for (int i = 1; i < transform.childCount; i++)
                    transform.GetChild(i).rotation = Quaternion.identity;

            }

            childCounter.text = CalculateCount().ToString();
        }
    }

    void OnRunAnimation(bool isMoving, Animator[] animators)
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
            string lenceText = lence.GetComponentInChildren<TextMeshProUGUI>().text;
            EventManager.Broadcast(GameEvent.OnLence, lenceText);

            switch (lence.lenceType)
            {
                case Lence.LenceType.Add:
                    EventManager.Broadcast(GameEvent.OnGenerateStickman, totalStickmen, operationNum + totalStickmen, stickman, transform, Quaternion.identity);
                    break;

                case Lence.LenceType.Multiply:
                    EventManager.Broadcast(GameEvent.OnGenerateStickman, totalStickmen, operationNum * totalStickmen, stickman, transform, Quaternion.identity);
                    break;
            }
            GameObject lenceMatObj = lence.transform.GetChild(2).gameObject;
            lenceMatObj.SetActive(false);
            OnRunAnimation(GameManager.Instance.isMoving, animators);
            EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform);
        }

        if (other.CompareTag("Enemy"))
        {
            enemyTransform = other.transform;
            GameManager.Instance.isAttacking = true;
        }


        if (other.CompareTag("Finish"))
        {
            EventManager.Broadcast(GameEvent.OnFinish);
            EventManager.Broadcast(GameEvent.OnCreateTower, transform.childCount - 1);
            counterMarkTrans.gameObject.SetActive(false);
        }
    }
    private void PositionChecker()
    {
        childCounter.text = CalculateCount().ToString();
        counterMarkTrans.rotation = Quaternion.LookRotation(counterMarkTrans.position - Camera.main.transform.position);
    }

}
