﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;

    private float iniPosY;
    private float randomPos;

    void Start()
    {
        iniPosY = transform.position.y;
        randomPos = Random.Range(0, 1.5f);
    }


    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        transform.position = new Vector3(transform.position.x, iniPosY + Mathf.PingPong(Time.time + randomPos, 1.5f), transform.position.z);

    }
}
