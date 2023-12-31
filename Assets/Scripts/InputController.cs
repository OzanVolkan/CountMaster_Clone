using UnityEngine;
using UnityEngine.EventSystems;
public class InputController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public float moveSpeed;
    public Transform playerTransform;

    [SerializeField] GameObject startPanel, buttonPanel;
    [SerializeField] float movementSensitivity;
    [SerializeField] float leftMovementLimit;
    [SerializeField] float rightMovementLimit;
    public void OnDrag(PointerEventData eventData)
    {
        int crowdedness = playerTransform.childCount;

        rightMovementLimit = 1.4f - ((crowdedness / 10f) * 0.11f);
        leftMovementLimit = -rightMovementLimit;

        if (!GameManager.Instance.isAttacking && GameManager.Instance.canDrag)
        {
            Vector3 tempPosition = playerTransform.localPosition;
            tempPosition.x = Mathf.Clamp(tempPosition.x + (eventData.delta.x / movementSensitivity), leftMovementLimit, rightMovementLimit);
            playerTransform.localPosition = tempPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (startPanel.activeInHierarchy) startPanel.SetActive(false);
        if (buttonPanel.activeInHierarchy) buttonPanel.SetActive(false);

        GameManager.Instance.isMoving = true;

        Animator[] animators = playerTransform.GetComponent<Player>().animators;
        EventManager.Broadcast(GameEvent.OnRunAnimation, GameManager.Instance.isMoving,animators);
    }
    void Update()
    {
        moveSpeed = GameManager.Instance.isAttacking ? 0.25f : 2f;

        if (GameManager.Instance.isMoving)
        {
            playerTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
