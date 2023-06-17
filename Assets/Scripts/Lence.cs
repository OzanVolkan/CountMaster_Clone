using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lence : MonoBehaviour
{
    public enum LenceType
    {
        Add,
        Multiply
    }

    public LenceType lenceType;
    public TextMeshProUGUI lenceNum;
    public int randomNum;

    private void OnEnable()
    {
        GenerateOperation();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void GenerateOperation()
    {
        switch (lenceType)
        {
            case LenceType.Add:
                randomNum = Random.Range(10, 101);
                randomNum = randomNum - (randomNum % 5);
                lenceNum.text = "+" + randomNum;
                break;

            case LenceType.Multiply:
                randomNum = Random.Range(2, 6);
                lenceNum.text = "X" + randomNum;
                break;
        }
    }
}
