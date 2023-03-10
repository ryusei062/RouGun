using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltips : MonoBehaviour
{
    public Text detailText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);

    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);

    }

    public void UpdateTooltip(string _detailText)
    {
        detailText.text = _detailText;
    }

    public void SetPosition(Vector2 _pos)
    {
        transform.localPosition = _pos;
    }
}
