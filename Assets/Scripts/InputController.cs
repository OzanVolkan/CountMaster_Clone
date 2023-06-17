using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InputController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    //[HideInInspector] public GameObject tutorialPanel;

    public int moveSpeed;
    public Transform playerTransform;

    [SerializeField] private float movementSensitivity;
    //public GameObject canvasUpgrade;
    [SerializeField] float leftMovementLimit;
    [SerializeField] float rightMovementLimit;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Start()
    {
        //tutorialPanel = GameObject.Find("TutorialPanel");
    }
    public void OnDrag(PointerEventData eventData)
    {
        int crowdedness = playerTransform.childCount;

        rightMovementLimit = 1.4f - ((crowdedness / 10f) * 0.11f);
        leftMovementLimit = -rightMovementLimit;

        Vector3 tempPosition = playerTransform.localPosition;
        tempPosition.x = Mathf.Clamp(tempPosition.x + (eventData.delta.x / movementSensitivity), leftMovementLimit, rightMovementLimit);
        playerTransform.localPosition = tempPosition;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.isMoving = true;
        EventManager.Broadcast(GameEvent.OnRunAnimation, GameManager.Instance.isMoving);
    }
    void Update()
    {
        if (GameManager.Instance.isMoving)
        {
            playerTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        }

    }
}
