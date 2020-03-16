using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public void BtnTeleportOption()
    {
        FindObjectOfType<FixedElementCanvasController>().TeleportOption(int.Parse(this.gameObject.name));
    }
}
