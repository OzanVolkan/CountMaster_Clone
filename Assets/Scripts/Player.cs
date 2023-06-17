using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
public class Player : MonoBehaviour
{

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
    }

    void Update()
    {
        ReplaceStickmen();
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

    private void ReplaceStickmen()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            float x = distance * MathF.Sqrt(i) * MathF.Cos(i * radius);
            float z = distance * MathF.Sqrt(i) * MathF.Sin(i * radius);

            Vector3 newPos = new Vector3(x, 0, z);

            transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lence"))
        {
            Lence lence = other.GetComponent<Lence>();
            //hata çýkartabilir, asýl enum randomLenceType olabilir.

            int operationNum = other.GetComponent<Lence>().randomNum;

            switch (lence.lenceType)
            {
                case Lence.LenceType.Add:
                    EventManager.Broadcast(GameEvent.OnGenerateStickman, operationNum);
                    break;

                case Lence.LenceType.Multiply:
                    EventManager.Broadcast(GameEvent.OnGenerateStickman, operationNum * totalStickmen);
                    break;
            }
            OnRunAnimation(GameManager.Instance.isMoving);
            ReplaceStickmen();
            //2 kere triggera girmemesi için buarada bir þeyler yaz
        }
        childCounter.text = CalculateCount().ToString();

    }

}
