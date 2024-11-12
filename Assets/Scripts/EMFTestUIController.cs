using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EMFTestUIController : MonoBehaviour
{
    [SerializeField] private EMFDetectorController _emf;
    private TextMeshProUGUI _textMesh;

    void Start() {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        _textMesh.text =
            $"{_emf.Count} ghost(s) in range\n" +
            $"closest dist {_emf.ShortestDistance}";
    }
}
