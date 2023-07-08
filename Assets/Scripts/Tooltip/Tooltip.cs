using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI HeaderField;

    public TextMeshProUGUI ContentField;

    public LayoutElement LayoutElement;

    public int CharacterWrapLimit;

    public void SetText(string Content, string Header = "")
    {
        if (string.IsNullOrEmpty(Header))
        {
            HeaderField.gameObject.SetActive(false);
        }
        else
        {
            HeaderField.gameObject.SetActive(true);
            HeaderField.text = Header;
        }

        ContentField.text = Content;

        int HeaderLength = HeaderField.text.Length;
        int ContentLength = ContentField.text.Length;

        LayoutElement.enabled = (HeaderLength > CharacterWrapLimit || ContentLength > CharacterWrapLimit) ? true : false;
    }

    private void Update()
    {
        Vector2 Position = Input.mousePosition;
        transform.position = Position;
    }
}
