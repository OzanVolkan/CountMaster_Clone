using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class GameManager : SingletonManager<GameManager>
{
    public List<GameObject> towerList = new List<GameObject>();
    public GameData gameData;
    public GameObject finishCam;
    public Transform playerTransform;
    public bool isMoving;
    public bool isAttacking;
    public bool hasFinished;
    public bool reachedStairs;

    private Transform target;
    private float followOffset = 2.75f;
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGenerateStickman, new Action<int, int, GameObject, Transform, Quaternion>(OnGenerateStickman));
        EventManager.AddHandler(GameEvent.OnReplaceStickmen, new Action<float, float, Transform>(OnReplaceStickmen));
        EventManager.AddHandler(GameEvent.OnFinish, new Action(OnFinish));
        EventManager.AddHandler(GameEvent.OnFinishCamFollow, new Action(OnFinishCamFollow));
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateStickman, new Action<int, int, GameObject, Transform, Quaternion>(OnGenerateStickman));
        EventManager.RemoveHandler(GameEvent.OnReplaceStickmen, new Action<float, float, Transform>(OnReplaceStickmen));
        EventManager.RemoveHandler(GameEvent.OnFinish, new Action(OnFinish));
        EventManager.RemoveHandler(GameEvent.OnFinishCamFollow, new Action(OnFinishCamFollow));
    }

    #region EVENTS

    void OnGenerateStickman(int totalStickmen, int number, GameObject stickmanType, Transform parent, Quaternion quaternion)
    {
        for (int i = totalStickmen; i < number; i++)
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
    void OnFinish()
    {
        finishCam.SetActive(true);
        hasFinished = true;
    }

    void OnFinishCamFollow()
    {
        Cinemachine.CinemachineVirtualCamera finCam = finishCam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        finCam.m_Follow = towerList[0].transform;
    }

    #endregion
}
