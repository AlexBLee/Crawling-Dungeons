using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class DragInventory : MonoBehaviour, IDragHandler // required interface when using the OnPointerEnter method.
{
    public void OnDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.parent.position = Input.mousePosition;
    }    

}