using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Red") && other.transform.parent.childCount > 1)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
