using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;
public class GUICategories : MonoBehaviour {

    public GameObject itemPrefab;
    public float height = 85;
    public static string categFolderName;
    string description, descricao;
    public static int totalCategories;
    public Text catTitle;

    List<Toggle> catToggles = new List<Toggle>();
    public static List<string> catChoices = new List<string>();

    public static bool manualCategory = false;
    public static int chosenFolder;

    void Update()
    {
        CheckToggleOn();
    }
    
    void Start()
    {
        catTitle.text = Language.categories;

        string filepath = Application.dataPath + @"/Settings/Categories.xml";
        XmlDocument xmlDoc = new XmlDocument();

        //loads the file to create the list of categories
        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);
            XmlNodeList categories = xmlDoc.GetElementsByTagName("Categories");

            foreach (XmlNode catNumber in categories)
            {
                XmlNodeList catContent = catNumber.ChildNodes;

                totalCategories = 0;

                foreach (XmlNode catNum in catContent)
                {
                    foreach (XmlNode xmlsettings in catNum)
                    {
                        if (xmlsettings.Name == "folderName")
                            categFolderName = xmlsettings.InnerText;
                        if (xmlsettings.Name == "description")
                            description = xmlsettings.InnerText;
                        if (xmlsettings.Name == "descricao")
                            descricao = xmlsettings.InnerText;
                    }

                    totalCategories++;

                    GameObject newItem = Instantiate(itemPrefab) as GameObject;
                    newItem.name = gameObject.name + catNumber;
                    newItem.transform.SetParent(gameObject.transform);
                    Toggle tgl = newItem.GetComponentInChildren<Toggle>();

                    tgl.group = this.gameObject.GetComponent<ToggleGroup>();
                    Text tglText = tgl.GetComponentInChildren<Text>();
                    tglText.text = categFolderName;
                    Text descriptionText = newItem.transform.Find("CatDescription").GetComponent<Text>();

                    if (Language.lang == "EN")
                        descriptionText.text = description;
                    else if (Language.lang == "PT")
                        descriptionText.text = descricao;

                    if (SpawnTiles.folder == categFolderName)
                        tgl.isOn = true;

                    catToggles.Add(tgl);
                    catChoices.Add(categFolderName);
                }

                float scrollHeight = height * totalCategories;
                RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
                RectTransform scrollableRectTransform = gameObject.GetComponentInParent<RectTransform>();
                Vector2 width = scrollableRectTransform.sizeDelta;
                containerRectTransform.sizeDelta = new Vector2(width.x, scrollHeight);
            }
        }   
    }

    void CheckToggleOn()
    {
        for (int i = 0; i < totalCategories; i++)
        {
            if (catToggles[i].isOn)
            {
                manualCategory = true;
                chosenFolder = i;
                SpawnTiles.folder = catChoices[i];
            }
        }
    }
}
