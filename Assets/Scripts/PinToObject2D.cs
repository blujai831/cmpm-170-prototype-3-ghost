using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinToObject2D : MonoBehaviour
{

    [SerializeField] public GameObject Target;
    [SerializeField] float Strength = 1.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(
            transform.position.x + (
                Target.transform.position.x - transform.position.x
            )*Strength,
            transform.position.y + (
                Target.transform.position.y - transform.position.y
            )*Strength,
            transform.position.z
        );
    }
}
