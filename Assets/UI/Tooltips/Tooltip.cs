using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

 [ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{

    public TextMeshProUGUI headerField;

    public TextMeshProUGUI costField;

    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string cost, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
            costField.gameObject.SetActive(false);
        }
        else
        {
           headerField.gameObject.SetActive(true);
           costField.gameObject.SetActive(true);
           headerField.text = header;
           costField.text = cost;
        }

        contentField.text = content;

    }
    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int costLength = costField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || costLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;        
        }

        Vector2 position = Input.mousePosition;
        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
