using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBox : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        HoverBoxManager._instance.SetAndShowToolTip(message);
    }

    private void OnMouseExit()
    {
        HoverBoxManager._instance.HideToolTip();
    }
}
