using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : SingletonManager<GameManager>
{
    public bool isMoving;

    [SerializeField] GameObject stickman;
    [SerializeField] GameObject player;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGenerateStickman, new Action<int>(OnGenerateStickman));
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateStickman, new Action<int>(OnGenerateStickman));
    }
    void Start()
    {
        OnGenerateStickman(10);
    }

    void Update()
    {
        
    }

    void OnGenerateStickman(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Instantiate(stickman, player.transform.position, Quaternion.identity, player.transform);
        }
    }
}
