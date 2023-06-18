using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Stickman : MonoBehaviour
{
    private Animator animator;
    private float distance = 0.1f, radius = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private IEnumerator CheckIfWin()
    {
        yield return new WaitForSeconds(2f);
        EventManager.Broadcast(GameEvent.OnWin);
    }
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

            case "Obstacle":
                Destroy(gameObject);
                break;

            case "Jump":
                transform.DOLocalJump(transform.localPosition, 1f, 1, 1.25f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform.parent);
                });
                break;

            case "Stair":
                if (!GameManager.Instance.reachedStairs)
                {
                    GameManager.Instance.reachedStairs = true;
                    EventManager.Broadcast(GameEvent.OnFinishCamFollow);
                }
                
                transform.parent.parent = null; // for instance tower_0
                transform.parent = null; // stickman
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                GetComponent<Collider>().isTrigger = false;
                animator.SetBool("Run", false);

                if (GameManager.Instance.playerTransform.childCount == 2)
                {
                    other.GetComponent<Renderer>().material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                        .SetEase(Ease.Flash);

                    StartCoroutine(CheckIfWin());
                }
                break;
        }
    }
}
