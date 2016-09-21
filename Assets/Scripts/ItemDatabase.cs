using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson; //Allows us to take JSON data and turn it into a c# object

public class Stat
{
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public Stat(Stat stats)
    {
        this.Power = stats.Power;
        this.Defence = stats.Defence;
        this.Vitality = stats.Vitality;
    }

    public Stat(JsonData data)
    {
        Power = (int)data["power"];
        Defence = (int)data["defence"];
        Vitality = (int)data["vitality"];
       
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public Stat Stats {get; set;}
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public Sprite Sprite { get; set; }
    public GameObject gameobject { get; set; }

    public Item()
    {
        //Set the ID to -1 indicating that the item has not been set
        ID = -1;
    }

    public Item(JsonData data)
    {
        ID = (int)data["id"];
        Title = data["title"].ToString();
        Value = (int)data["value"];
        Stats = new Stat(data["stats"]);
        Description = data["description"].ToString();
        Stackable = (bool)data["stackable"];
        Rarity = (int)data["rarity"];
        string fileName = data["sprite"].ToString();
        Sprite = Resources.Load<Sprite>("Sprites/Items/" + fileName);
    }

}

public class ItemDatabase : MonoBehaviour
{
    //Stores all the diff types of items in a database
    //Use this list to spawn multiple items
    private Dictionary<string,Item> database = new Dictionary<string, Item>();

    private JsonData itemData;

    private static ItemDatabase instance = null;
   
   void Awake()
    {
        //Check if instance = null
        if (instance == null)
        {
            //Set instance to current instance
            instance = this;
            //Obtain the file path for Items.json
            string jsonFilePath = Application.dataPath + "/StreamingAssets/Items.json";
            //Read entire file into string
            string jsonText = File.ReadAllText(jsonFilePath);
            //Load in the data through JsonMapper
            itemData = JsonMapper.ToObject(jsonText);
            //Construct the item database
            ConstructDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ConstructDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            JsonData data = itemData[i];
            Item newItem = new Item(data);
            database.Add(newItem.Title, newItem);
        }
    }

    public static Item GetItem(string itemName)
    {
        Dictionary<string, Item> database = instance.database;

        if(database.ContainsKey(itemName))
        {
            return database[itemName];
        }
        return null;
    }

    public static Dictionary<string,Item> GetDatabase()
    {
        return instance.database;
    }
}
