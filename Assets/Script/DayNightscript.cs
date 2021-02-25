using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightscript : MonoBehaviour
{
    Light sun;
    public float speed = 1f;

    void Start()
    {
        sun = GetComponent<Light>();
    }

    void Update()
    {
        sun.transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}
