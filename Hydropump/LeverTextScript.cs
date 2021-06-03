using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTextScript : MonoBehaviour
{
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
