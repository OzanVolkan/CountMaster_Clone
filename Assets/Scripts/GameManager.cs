using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;
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
    public bool canDrag;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGenerateStickman, new Action<int, int, GameObject, Transform, Quaternion>(OnGenerateStickman));
        EventManager.AddHandler(GameEvent.OnReplaceStickmen, new Action<float, float, Transform>(OnReplaceStickmen));
        EventManager.AddHandler(GameEvent.OnFinish, new Action(OnFinish));
        EventManager.AddHandler(GameEvent.OnFinishCamFollow, new Action(OnFinishCamFollow));
        EventManager.AddHandler(GameEvent.OnSave, new Action(OnSave));
        EventManager.AddHandler(GameEvent.OnLoad, new Action(OnLoad));
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateStickman, new Action<int, int, GameObject, Transform, Quaternion>(OnGenerateStickman));
        EventManager.RemoveHandler(GameEvent.OnReplaceStickmen, new Action<float, float, Transform>(OnReplaceStickmen));
        EventManager.RemoveHandler(GameEvent.OnFinish, new Action(OnFinish));
        EventManager.RemoveHandler(GameEvent.OnFinishCamFollow, new Action(OnFinishCamFollow));
        EventManager.RemoveHandler(GameEvent.OnSave, new Action(OnSave));
        EventManager.RemoveHandler(GameEvent.OnLoad, new Action(OnLoad));
    }

    private void Awake()
    {
        OnLoad();
    }
    private void Start()
    {
        canDrag = true;
        string levelName = SceneManager.GetActiveScene().name;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelName);
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
        canDrag = false;
    }

    void OnFinishCamFollow()
    {
        Cinemachine.CinemachineVirtualCamera finCam = finishCam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        finCam.m_Follow = towerList[0].transform;
    }
    void OnSave()
    {
        SaveManager.SaveData(gameData);
    }

    void OnLoad()
    {
#if !UNITY_EDITOR
        SaveManager.LoadData(gameData);
#endif

    }
    #endregion
    public void OnApplicationQuit()
    {
        SaveManager.SaveData(gameData);
    }
    public void OnApplicationFocus(bool focus)
    {
        if (focus == false) SaveManager.SaveData(gameData);

    }
    public void OnApplicationPause(bool pause)
    {
        if (pause == true) SaveManager.SaveData(gameData);

    }
}
