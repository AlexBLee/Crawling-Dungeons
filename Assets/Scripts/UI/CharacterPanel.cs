using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    public List<EquippableItem> items;

    // Specific types of items are locked to specific indices for item list
    // 0 - Helmet
    // 1 - Upper
    // 2 - Left
    // 3 - Right
    // 4 - Lower
    [HideInInspector]
    public int helmetNumber = 0;
    [HideInInspector]
    public int upperNumber = 1;
    [HideInInspector]
    public int leftNumber = 2;
    [HideInInspector]
    public int rightNumber = 3;
    [HideInInspector]
    public int lowerNumber = 4;

    public List<Image> images;
    public Sprite blankImage;
    
    public Player player;

    public GameObject characterPanel;
    
    public ItemDisplay itemDisplay;

    public TextMeshProUGUI level;
    public TextMeshProUGUI expText;


    private void Start() 
    {
        characterPanel.SetActive(false);
        player = GameManager.instance.playerObject;
        
        UpdateLevel(player.level);
        UpdateXP(player.exp, player.expThreshold);

    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.C) && !characterPanel.activeSelf)
        {
            characterPanel.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.C) && characterPanel.activeSelf)
        {
            characterPanel.SetActive(false);
        }
    }

    public void AddItem(EquippableItem item, int index)
    {
        items[index] = item;
        images[index].sprite = item.image;
    }

    public void RemoveItem(int index)
    {
        items[index] = null;
        images[index].sprite = blankImage;        
    }

    public void SwapItem(int indexA, int indexB)
    {
        Sprite imgTemp = images[indexA].sprite;
        images[indexA].sprite = images[indexB].sprite;
        images[indexB].sprite = imgTemp;
    }

    public void DisplayItemInfo(int index, Vector3 position)
    {
        itemDisplay.gameObject.SetActive(true);
        itemDisplay.nameOfItem.text = items[index].itemName;
        itemDisplay.description.text = items[index].description;
        itemDisplay.gameObject.transform.parent.position = position;
    }
    
    public void UpdateLevel(int number)
    {
        level.text = number.ToString();
    }

    public void UpdateXP(float xp, float totalExp)
    {
        expText.text = xp.ToString() + " / " + totalExp.ToString();

    }


}
