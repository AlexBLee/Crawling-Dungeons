using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class PanelReader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler// required interface when using the OnPointerEnter method.
{
    public int number;
    public int listOffset = 20;
    public int itemListLimit = 19;
    [SerializeField]
    private CharacterPanel charPanel;
    private Vector2 oldPos;
    public ItemDisplay itemDisplay;
    [SerializeField]
    public InventoryDisplay invDisplay;
    public GameObject parentObject;
    public GameObject originalObject;
    private Player player;

    public static bool pointerOnSlot;

    private void Start() 
    {
        player = GameManager.instance.playerObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(charPanel.items[GameManager.instance.uiIndexNumber - listOffset] != null)
            {
                player.UnequipItem(charPanel.items[GameManager.instance.uiIndexNumber - listOffset]);
                charPanel.RemoveItem(GameManager.instance.uiIndexNumber - listOffset);
            }
        }
    }

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOnSlot = true;
        GameManager.instance.uiIndexNumber = number;

        if(charPanel.items[number - listOffset] != null)
        {
            charPanel.DisplayItemInfo(number - listOffset, transform.position);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDisplay.gameObject.SetActive(false);
        pointerOnSlot = false;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPos = transform.position;

        if(parentObject.activeInHierarchy)
        {
            transform.SetParent(parentObject.transform);
            transform.SetSiblingIndex(0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(charPanel.items[number - listOffset] != null)
        {
            eventData.pointerDrag.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // UI index number is set to default to the clicked slot number if no other slot is hovered 
        if(GameManager.instance.uiIndexNumber == number)
        {
            // item is thrown out
            player.UnequipItemAndDrop(charPanel.items[GameManager.instance.uiIndexNumber - listOffset]);
            charPanel.RemoveItem(GameManager.instance.uiIndexNumber - listOffset);
        }

        if(GameManager.instance.uiIndexNumber <= itemListLimit && player.itemList[GameManager.instance.uiIndexNumber] != null)
        {
            // if item is compatible:
            if(charPanel.items[number - listOffset].GetType() == player.itemList[GameManager.instance.uiIndexNumber].GetType())
            {
                // Compatible item swapping
                Item itemToEquip = player.itemList[GameManager.instance.uiIndexNumber];
                player.UnequipItem(charPanel.items[number - listOffset], GameManager.instance.uiIndexNumber);
                charPanel.RemoveItem(number - listOffset);
                player.EquipItem(itemToEquip);
            }
        }
        else if(GameManager.instance.uiIndexNumber <= itemListLimit)
        {
            // blank slot adding to inventory
            player.UnequipItem(charPanel.items[number - listOffset], GameManager.instance.uiIndexNumber);
            charPanel.RemoveItem(number - listOffset);
        }
        
        transform.SetParent(originalObject.transform);
        transform.SetSiblingIndex(0);
        eventData.pointerDrag.transform.position = oldPos;
            
        
    }



    

}