using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraCullingMask {EVERYTHING, PLAYER}
public class ManageCameraCullingMask : MonoBehaviour
{
    public CameraCullingMask cm;
    LayerMask everythingMask;
    LayerMask playerMask;

    void Start()
    {
        everythingMask = -1;
        playerMask = Camera.main.cullingMask;
        cm = CameraCullingMask.PLAYER;
    }

    public void ChangeCullingMask()
    {
        Camera.main.cullingMask = (cm == CameraCullingMask.EVERYTHING) ? playerMask : everythingMask;
        cm = (cm == CameraCullingMask.EVERYTHING) ? CameraCullingMask.PLAYER : CameraCullingMask.EVERYTHING;
    }
}
