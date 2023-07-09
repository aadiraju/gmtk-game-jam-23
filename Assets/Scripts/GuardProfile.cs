using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdProfile : MonoBehaviour
{
    public BaseGuard ProfileGaurd;
    public bool IsSelected = false;
    public GameObject Highlight;

    void Start()
    {
        Highlight.SetActive(false);
    }

    public void ToggleSelected()
    {
        IsSelected = !IsSelected;
        Highlight.SetActive(IsSelected);
    }

    void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        if (!IsSelected)
        {
            Highlight.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        ToggleSelected();
        GameManager.Instance.SelectGaurdProfile(IsSelected ? this : null);
    }

}
