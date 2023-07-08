using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem Current;

    public Tooltip Tooltip;

    public void Awake()
    {
        Current = this;
        Current.Tooltip.gameObject.SetActive(false);
    }

    public static void Show(string Content, string Header = "")
    {
        Current.Tooltip.SetText(Content, Header);
        Current.Tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Current.Tooltip.gameObject.SetActive(false);
    }

}
