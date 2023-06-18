using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Stickman : MonoBehaviour
{
    [SerializeField] GameObject[] splashes;
    [SerializeField] GameObject hitVfxObj, meshObj;
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

    private void DestroyParticle(bool isRed)
    {
        Destroy(GetComponent<Collider>());
        hitVfxObj.SetActive(true);
        hitVfxObj.transform.SetParent(null);

        if (isRed)
        {
            int ranInd = Random.Range(0, splashes.Length);
            splashes[ranInd].SetActive(true);
            splashes[ranInd].transform.SetParent(null);
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Red":
                if (other.transform.parent.childCount > 1)
                {
                    Destroy(other.gameObject);
                    DestroyParticle(true);
                }
                break;

            case "Obstacle":
                if (GameManager.Instance.playerTransform.childCount <= 2)
                    EventManager.Broadcast(GameEvent.OnFail);

                DestroyParticle(false);
                break;

            case "Jump":
                transform.DOLocalJump(transform.localPosition, 1f, 1, 1.25f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform.parent);
                });
                break;

            case "Save":
                Destroy(other.gameObject);
                EventManager.Broadcast(GameEvent.OnSaveUs);
                EventManager.Broadcast(GameEvent.OnReplaceStickmen, distance, radius, transform.parent);
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
