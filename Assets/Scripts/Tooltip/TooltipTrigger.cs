using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Content;
    public string Header;
    public BaseUnit UnitPrefab;
    public float Delay = 2f;

    void Awake() {
        Content = UnitPrefab.Description();
        Header = UnitPrefab.Title();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipSystem.Hide();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(Delay);
        TooltipSystem.Show(Content, Header);
    }
}
