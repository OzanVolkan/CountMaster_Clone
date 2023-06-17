using TMPro;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class GateControl : MonoBehaviour
{
    [SerializeField] private Transform gateA, gateB;
    [SerializeField] private Vector3 direction;

    private LineRenderer _lineRenderer;

    private LineRenderer LineRenderer
    {
        get
        {
            if (_lineRenderer == null) return _lineRenderer = GetComponent<LineRenderer>();
            else return _lineRenderer;
        }
    }

    private TextMeshPro _textMeshPro;

    private TextMeshPro TextMeshPro
    {
        get
        {
            if (_textMeshPro == null) return _textMeshPro = GetComponentInChildren<TextMeshPro>();
            return _textMeshPro;
        }
    }
    
    private void Update()
    {
        if (gateA == null || gateB == null) return;
        
        LineRenderer.SetPosition(0, gateA.position);
        LineRenderer.SetPosition(1, gateB.position);
        var targetDirection = gateA.position - gateB.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up) * Quaternion.Euler(direction);
        TextMeshPro.transform.rotation = targetRotation;
        Vector3 centerPosition = (gateA.position + gateB.position) / 2f;
        TextMeshPro.transform.position = centerPosition + new Vector3(0,0,0.05f);
    }
}