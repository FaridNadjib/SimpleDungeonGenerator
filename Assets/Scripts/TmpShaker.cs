using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpShaker : MonoBehaviour
{

    [SerializeField] float horizontalAmplitude = 1f;
    [SerializeField] float horizontalFrequency = 1f;
    [SerializeField] float verticalAmplitude = 1f;
    [SerializeField] float verticalFrequency = 1f;
    Vector3 tmpPos = new Vector3();
    Vector2 startingPos = new Vector2();

    private void Awake()
    {
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.z;
        tmpPos.y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        tmpPos.x = startingPos.x + Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude;
        tmpPos.z = startingPos.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = tmpPos;
    }
}
