using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel, failPanel;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnWin, new Action(OnWin));
        EventManager.AddHandler(GameEvent.OnFail, new Action(OnFail));
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnWin, new Action(OnWin));
        EventManager.RemoveHandler(GameEvent.OnFail, new Action(OnFail));
    }

    #region EVENTS

    void OnWin()
    {
        winPanel.SetActive(true);
    }

    void OnFail()
    {
        if (!failPanel.activeInHierarchy)
        {
            failPanel.SetActive(true);
            GameManager.Instance.isMoving = false;
        }
    }

    #endregion

    #region BUTTONS

    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
