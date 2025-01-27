﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        transform.parent = null;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            aSource.Play();
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(4.5f);
        GameManager.instance.sceneC.LoadGameOver();
    }
}
