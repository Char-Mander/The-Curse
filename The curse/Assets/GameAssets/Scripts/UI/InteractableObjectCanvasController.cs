using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectCanvasController : WorldSpaceCanvasController
{
    [SerializeField]
    GameObject interactionPanel;

    public void Start()
    {
        interactionPanel.SetActive(false);
    }

    public void ShowOrHidePanel(bool value)
    {
        interactionPanel.SetActive(value);
    }
}
