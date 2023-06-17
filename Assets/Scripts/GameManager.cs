using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class GameManager : SingletonManager<GameManager>
{
    public bool isMoving;
    public bool isAttacking;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGenerateStickman, new Action<int, GameObject, Transform,Quaternion>(OnGenerateStickman));
        EventManager.AddHandler(GameEvent.OnReplaceStickmen, new Action<float, float, Transform>(OnReplaceStickmen));
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateStickman, new Action<int, GameObject, Transform,Quaternion>(OnGenerateStickman));
        EventManager.RemoveHandler(GameEvent.OnReplaceStickmen, new Action<float, float, Transform>(OnReplaceStickmen));
    }
    void Start()
    {
    }

    void Update()
    {

    }

    #region EVENTS

    void OnGenerateStickman(int number, GameObject stickmanType, Transform parent,Quaternion quaternion)
    {
        for (int i = 0; i < number; i++)
        {
            Instantiate(stickmanType, parent.transform.position, quaternion, parent.transform);
        }
    }

    void OnReplaceStickmen(float distance, float radius, Transform transform)
    {
        if (transform.childCount > 2)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                float x = distance * MathF.Sqrt(i) * MathF.Cos(i * radius);
                float z = distance * MathF.Sqrt(i) * MathF.Sin(i * radius);

                Vector3 newPos = new Vector3(x, 0, z);

                transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
            }
        }

    }

    #endregion
}
