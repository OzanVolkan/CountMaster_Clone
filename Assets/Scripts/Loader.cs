using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loader : MonoBehaviour
{
    [SerializeField] GameData gameData;
    void Start()
    {
        if (gameData.levelIndex < 4)
        {
            SceneManager.LoadScene(gameData.levelIndex);
        }
        else
        {
            int remain = gameData.levelIndex % 3;
            if (remain == 0)
                SceneManager.LoadScene(3);
            else
                SceneManager.LoadScene(remain);
        }
    }
}
