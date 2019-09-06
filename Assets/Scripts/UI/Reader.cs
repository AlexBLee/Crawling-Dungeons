using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class Reader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler// required interface when using the OnPointerEnter method.
{
    private int listOffset = 20;
    private int itemLimit = 19;
    public int number;
    [SerializeField]
    private InventoryDisplay inventoryDisplay;
    private Vector2 oldPos;
    public ItemDisplay itemDisplay;
    private Player player;
    public GameObject parentObject;
    public GameObject originalObject;
    [SerializeField]
    private CharacterPanel charPanel;
    public static bool pointerOnSlot;

    private void Start() 
    {
        player = GameManager.instance.playerObject;
    }

    // Equipping selected item
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(inventoryDisplay.items[GameManager.instance.uiIndexNumber] != null)
            {
                player.EquipItem(inventoryDisplay.items[GameManager.instance.uiIndexNumber], GameManager.instance.uiIndexNumber);
            }
        }
    }

    // Display Item info when cursor overtop
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOnSlot = true;
        GameManager.instance.uiIndexNumber = number;
        //transform.parent.parent.SetSiblingIndex(0);

        if(inventoryDisplay.items[number] != null)
        {
            inventoryDisplay.DisplayItemInfo(number, transform.position);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDisplay.gameObject.SetActive(false);
        pointerOnSlot = false;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // The icon will show behind other icons (supposed to be infront), but the item has to be first in hierarchy for it to work.. TODO at some point..
        oldPos = transform.position;
        transform.SetSiblingIndex(0);
    }

    // Drag item to cursor position
    public void OnDrag(PointerEventData eventData)
    {
        if(inventoryDisplay.items[number] != null)
        {
            eventData.pointerDrag.transform.position = Input.mousePosition;
        }


        // temporary fix to get the icon infront most of the ui at the right time..
        // will currently be disabled >> need to find a better solution to ui
        if(eventData.pointerEnter.name == "empty")
        {
            if(parentObject.activeInHierarchy)
            {
                transform.SetParent(parentObject.transform);
                transform.SetSiblingIndex(0);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // UI index number is set to default to the clicked slot number if no other slot is hovered 
        if(GameManager.instance.uiIndexNumber == number)
        {
            player.RemoveItem(number);
            transform.position = oldPos;
        }

        if(GameManager.instance.uiIndexNumber >= itemLimit && charPanel.items[GameManager.instance.uiIndexNumber - listOffset] == null)
        {
            // with the way equip item works, it'll just move to the right slot on its own. may or may not need a change?
            player.EquipItem(player.itemList[number]);
            Debug.Log("@@");
        }
        else if(GameManager.instance.uiIndexNumber <= itemLimit && inventoryDisplay.items[GameManager.instance.uiIndexNumber] != null)
        {
            eventData.pointerDrag.transform.position = oldPos;
            player.SwapItem(number, GameManager.instance.uiIndexNumber);
        }
        else if(inventoryDisplay.items[GameManager.instance.uiIndexNumber] == null)
        {
            // blank slot adding
            player.AddItem(player.itemList[number], GameManager.instance.uiIndexNumber);
            player.RemoveItem(number);
        }


        transform.SetParent(originalObject.transform);
        transform.SetSiblingIndex(0);
        eventData.pointerDrag.transform.position = oldPos;

    }



    

}