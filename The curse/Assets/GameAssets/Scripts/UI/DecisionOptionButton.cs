using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionOptionButton : MonoBehaviour
{

    public void BtnChosenOption()
    {
        FindObjectOfType<FixedElementCanvasController>().ChooseAnOption(int.Parse(this.gameObject.name));
    }
}
