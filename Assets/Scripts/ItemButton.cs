using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;


public class ItemButton : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    private PlayerController player;
    public int buttonID;
    private Item thisItem;

    public Tooltips tooltip;
    private Vector2 position;

    private Item GetThisItem()
    {
        for(int i = 0; i < GameManager.instance.items.Count; i++)
        {
            if(buttonID == i)
            {
                thisItem = GameManager.instance.items[i];
            }
        }

        return thisItem;
    }

    public void CloseButton()
    {
        GameManager.instance.RemoveItem(GetThisItem());
    }

    public void UseButton()
    {
        GameManager.instance.UseItem(GetThisItem());
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        GetThisItem();

        if(thisItem != null)
        {
            tooltip.ShowTooltip();
            //tooltip.UpdateTooltip(thisItem.GetInformation());
            tooltip.UpdateTooltip(GetDetailText(thisItem));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, Input.mousePosition, null, out position);
            tooltip.SetPosition(position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (thisItem != null)
        {
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("");
        }

    }

    private string GetDetailText(Item _item)
    {
        if(_item == null)
        {
            return "";
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<color=black><size=40>ÉAÉCÉeÉÄñº:</size></color> <color=Maroon><size=50>{0}</size></color>\n\n ", _item.GetItemName());
            stringBuilder.AppendFormat("<color=black><size=40>ê‡ñæ:</size> <size=50>{0}</size></color>", _item.GetInformation());

            return stringBuilder.ToString();
        }
    }
}
