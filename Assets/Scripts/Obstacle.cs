using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    IEnumerator OnObstacleHit()
    {
        yield return new WaitForSeconds(2.25f);
        EventManager.Broadcast(GameEvent.OnReplaceStickmen, 0.1f, 1f, GameManager.Instance.playerTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue"))
        {
            StartCoroutine(OnObstacleHit());
        }
    }
}
