using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EMFUIController : MonoBehaviour
{
    [SerializeField] private GameObject _emfObject;
    [SerializeField] private List<Sprite> _sprites;

    private SpriteRenderer _spriteRenderer;
    private EMFDetectorController _emf;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _emf = _emfObject.GetComponent<EMFDetectorController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i = (int) (_emf.DangerLevel*_sprites.Count);
        if (i >= 0 && i < _sprites.Count) {
            _spriteRenderer.sprite = _sprites[i];
        }
    }
}
