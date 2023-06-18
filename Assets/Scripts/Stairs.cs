using UnityEngine;

public class Stairs : MonoBehaviour
{
    void Start()
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in childRenderers)
        {
            int randomIndex = Random.Range(0, GameManager.Instance.gameData.stairColors.Length);
            renderer.material.color = GameManager.Instance.gameData.stairColors[randomIndex];
        }
    }
}
