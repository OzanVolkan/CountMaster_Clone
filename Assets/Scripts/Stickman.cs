using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Stickman : MonoBehaviour
{
    private float distance = 0.1f, radius = 1f;
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Red":
                if (other.transform.parent.childCount > 1)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                break;

            case "Jump":
                transform.DOLocalJump(transform.localPosition, 1f, 1, 1.25f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform.parent);
                });
                break;
        }
    }
}
