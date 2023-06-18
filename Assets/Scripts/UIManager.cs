using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel, failPanel;
    [SerializeField] GameObject lenceAmount;
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnWin, new Action(OnWin));
        EventManager.AddHandler(GameEvent.OnFail, new Action(OnFail));
        EventManager.AddHandler(GameEvent.OnLence, new Action<string>(OnLence));
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnWin, new Action(OnWin));
        EventManager.RemoveHandler(GameEvent.OnFail, new Action(OnFail));
        EventManager.RemoveHandler(GameEvent.OnLence, new Action<string>(OnLence));
    }

    #region EVENTS

    void OnWin()
    {
        winPanel.SetActive(true);
        GameManager.Instance.finishCam.GetComponentInChildren<ParticleSystem>().Play();
    }

    void OnFail()
    {
        if (!failPanel.activeInHierarchy)
        {
            failPanel.SetActive(true);
            GameManager.Instance.isMoving = false;
        }
    }

    void OnLence(string lenceText)
    {
        Vector2 spawnPos = new Vector2(0f, 400f);

        GameObject lenceEffect = Instantiate(lenceAmount, transform, false);
        lenceEffect.GetComponent<RectTransform>().localPosition = spawnPos;
        lenceEffect.transform.GetComponent<TextMeshProUGUI>().text = lenceText;
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
