using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class Slot
{
    public GameObject gameObject;
    public Item item;
    public Slot(GameObject gameObject, Item item)
    {
        this.gameObject = gameObject;
        this.item = item;
    }
}




public class InventoryScript : MonoBehaviour
{
    [Header("UI")]
    //Prefab of an individual slot
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    // Amount of slots to spawn
    public GameObject slotPanel;
    //Parent of all slots
    public int slotAmount;

    //Private
    private ItemDatabase itemDatabase;

    [Header("Items / Slots")]

    public List<Item> items = new List<Item>();
    public List<Slot> slots = new List<Slot>();


    void Start()
    {

        itemDatabase = GetComponent<ItemDatabase>();

        for (int i = 0; i < slotAmount; i++)
        {
            // clone the slot
            GameObject clone = Instantiate(slotPrefab);
            // Set slot's parent to be slot panel
            clone.transform.SetParent(slotPanel.transform);
            // Create a new slot
            Slot slot = new Slot(clone, null);

            //Get slot data
            SlotData slotData = clone.GetComponent<SlotData>();
            slotData.inventory = this;
            slotData.slot = slot;
            //Add that new slot to the list
            slots.Add(slot);
        }
 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddItem("Steel Gloves", 1);
        }
    }

    public void AddItem(string itemName, int itemAmount = 1)
    {
        Item newItem = ItemDatabase.GetItem(itemName); // Find the item name
        Slot newSlot = GetEmptySlot(); // Find empty slot

        if(newItem != null && newSlot != null)
        {
            if(HasStacked(newItem, itemAmount))
            {
                return;
            }
            // Set the empty slot
            newSlot.item = newItem;
            //Create a new item instance
            GameObject item = Instantiate(itemPrefab);
            item.transform.position = newSlot.gameObject.transform.position;
            item.transform.SetParent(newSlot.gameObject.transform);
            item.name = newItem.Title;
            //Set the item's gameobject
            newItem.gameobject = item;
            //Get the image component from the item
            Image image = item.GetComponent<Image>();
            image.sprite = newItem.Sprite;
            ItemData itemData = item.GetComponent<ItemData>();
            itemData.item = newItem;
            itemData.slot = newSlot;
            
        }

    }


    //Finds an empty slot in the list and returns it
    public Slot GetEmptySlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].item == null)
            {
                return slots[i];
            }
        }
        print("No empty slot has been found!");
        return null;
    }

    bool HasStacked(Item itemToAdd, int itemAmount = 1)
    {
        if(itemToAdd.Stackable)
        {
            Slot occupiedSlot = GetSlotWithItem(itemToAdd);
            if(occupiedSlot != null)
            {
                //Get reference to item in occupied slot
                Item item = occupiedSlot.item;
                // Obtain the script attached to that item
                ItemData itemData = item.gameobject.GetComponent<ItemData>();
                //Increases the item amount
                itemData.amount += itemAmount;
                //Set its text element
                Text textElement = item.gameobject.GetComponentInChildren<Text>();
                textElement.text = itemData.amount.ToString();
                return true;
            }
        }
        return false;
    }

    Slot GetSlotWithItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Item currentItem = slots[i].item;
            //Check if slot is empty AND check if item is the same
            if(currentItem != null && currentItem.Title == item.Title)
            {
                return slots[i];
            }
        }
        return null;
    }
}
