using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using GameAnalyticsSDK;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] GameObject winPanel, failPanel, buttonsPanel;
    [SerializeField] GameObject lenceAmount;
    [SerializeField] Button unitsButton, incomeButton;
    [SerializeField] TextMeshProUGUI totalMoney, unitsAmount, unitsLvl, incomeAmount, incomeLvl;
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnWin, new Action(OnWin));
        EventManager.AddHandler(GameEvent.OnFail, new Action(OnFail));
        EventManager.AddHandler(GameEvent.OnLence, new Action<string>(OnLence));
        EventManager.AddHandler(GameEvent.OnSaveUs, new Action(OnSaveUs));

    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnWin, new Action(OnWin));
        EventManager.RemoveHandler(GameEvent.OnFail, new Action(OnFail));
        EventManager.RemoveHandler(GameEvent.OnLence, new Action<string>(OnLence));
        EventManager.RemoveHandler(GameEvent.OnSaveUs, new Action(OnSaveUs));

    }

    private void Start()
    {
        InvokeRepeating("UICheck", 0.1f, 0.1f);
    }

    private void UICheck()
    {
        if (buttonsPanel.activeInHierarchy)
        {
            totalMoney.text = gameData.totalMoney.ToString();
            unitsAmount.text = gameData.unitsAmount.ToString();
            unitsLvl.text = gameData.unitsLevel + " LVL";
            incomeAmount.text = gameData.incomeAmount.ToString();
            incomeLvl.text = gameData.incomeLevel + " LVL";

            if (gameData.totalMoney >= gameData.unitsAmount)
                unitsButton.interactable = true;
            else
                unitsButton.interactable = false;

            if (gameData.totalMoney >= gameData.incomeAmount)
                incomeButton.interactable = true;
            else
                incomeButton.interactable = false;
        }
    }

    #region EVENTS

    void OnWin()
    {
        winPanel.SetActive(true);
        GameManager.Instance.finishCam.GetComponentInChildren<ParticleSystem>().Play();
        string levelName = SceneManager.GetActiveScene().name;
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelName);
    }

    void OnFail()
    {
        if (!failPanel.activeInHierarchy)
        {
            failPanel.SetActive(true);
            GameManager.Instance.isMoving = false;
            string levelName = SceneManager.GetActiveScene().name;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelName);
        }
    }

    void OnLence(string lenceText)
    {
        Vector2 spawnPos = new Vector2(0f, 400f);

        GameObject lenceEffect = Instantiate(lenceAmount, transform, false);
        lenceEffect.GetComponent<RectTransform>().localPosition = spawnPos;
        lenceEffect.transform.GetComponent<TextMeshProUGUI>().text = lenceText;
    }

    void OnSaveUs()
    {
        Vector2 spawnPos = new Vector2(0f, 400f);

        GameObject lenceEffect = Instantiate(lenceAmount, transform, false);
        lenceEffect.GetComponent<RectTransform>().localPosition = spawnPos;
        lenceEffect.transform.GetComponent<TextMeshProUGUI>().text = "+5";
    }
    #endregion

    #region BUTTONS

    public void Next()
    {
        gameData.levelIndex++;

        if (SceneManager.GetActiveScene().buildIndex != 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        else SceneManager.LoadScene(1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Units()
    {
        EventManager.Broadcast(GameEvent.OnAddUnits);
        EventManager.Broadcast(GameEvent.OnReplaceStickmen, 0.1f, 1f, GameManager.Instance.playerTransform);
        gameData.totalMoney -= gameData.unitsAmount;
        gameData.unitsAmount += 50;
        gameData.unitsLevel++;
        EventManager.Broadcast(GameEvent.OnSave);
    }
    public void Income()
    {
        gameData.totalMoney -= gameData.incomeAmount;
        gameData.incomeAmount += 50;
        gameData.incomeLevel++;
        EventManager.Broadcast(GameEvent.OnSave);
    }

    #endregion
}
