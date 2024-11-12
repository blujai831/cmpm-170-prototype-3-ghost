using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMeterController : MonoBehaviour
{
    [SerializeField] private GameObject _playerCharacter;
    [SerializeField] private GameObject _powerupIconSpriteMask;

    private PlayerCharacterController _pcController;
    private Vector3 _iconNeutralPosition;

    // Start is called before the first frame update
    void Start()
    {
        _pcController =
            _playerCharacter.GetComponent<PlayerCharacterController>();
        _iconNeutralPosition = _powerupIconSpriteMask.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _powerupIconSpriteMask.transform.localScale = new Vector3(
            1.0f, 1.0f - _pcController.PowerupAmountRemaining, 1.0f
        );
        _powerupIconSpriteMask.transform.localPosition = new Vector3(
            _iconNeutralPosition.x,
            _iconNeutralPosition.y +
                _pcController.PowerupAmountRemaining/2.0f,
            _iconNeutralPosition.z
        );
    }
}
