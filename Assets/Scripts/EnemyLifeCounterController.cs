using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeCounterController : MonoBehaviour
{
    [SerializeField] private GameObject _firstIcon;

    private List<GameObject> _icons;
    private bool _initialized = false;
    private int _lastKnownCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_initialized) {
            Initialize();
        } else if (_lastKnownCount != EnemyController.Count) {
            UpdateIcons();
        }
    }

    private void Initialize() {
        _icons = new List<GameObject>();
        _icons.Add(_firstIcon);
        _lastKnownCount = EnemyController.Count;
        for (int i = 1; i < _lastKnownCount; i++) {
            var icon = Instantiate(
                _firstIcon,
                _firstIcon.transform.position,
                Quaternion.identity
            );
            var spriteRenderer = icon.GetComponent<SpriteRenderer>();
            icon.transform.SetParent(_firstIcon.transform.parent);
            icon.transform.position = _firstIcon.transform.position;
            icon.transform.position += i*spriteRenderer.size.x*Vector3.right;
            icon.transform.localScale = _firstIcon.transform.localScale;
            _icons.Add(icon);
        }
        _initialized = true;
    }

    private void UpdateIcons() {
        for (int i = EnemyController.Count; i < _lastKnownCount; i++) {
            var spriteRenderer = _icons[i].GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.black;
        }
        _lastKnownCount = EnemyController.Count;
    }
}
