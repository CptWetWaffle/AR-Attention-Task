using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;

public class GUIProfiles : MonoBehaviour
{
    public GameObject profilePrefab, profilesUI;
    public float height = 50;
    public Text levelTitle;

    List<string> users = new List<string>();

    public static int totalProfiles = 0;

    //bool first = true;

    public static bool usersFound = false;
    public static bool hideUsersScroll = false;

    IEnumerator Start()
    {
        if (Directory.Exists(Application.dataPath + @"/Attention_Task_Log/"))
        {
            DirectoryInfo di = new DirectoryInfo(Application.dataPath + @"/Attention_Task_Log/");

            DirectoryInfo[] folders = di.GetDirectories();
            //totalProfiles = 0;
            foreach (DirectoryInfo fi in folders)
            {
                yield return fi.Name;
                //Debug.Log(fi.Name);
                users.Add(fi.Name);
                totalProfiles++;
            }

            ListButtons();
        }
    }

    void ListButtons()
    {
        if (totalProfiles > 0)
            usersFound = true;

        for(int i = 0; i< users.Count; i ++)
        {
            GameObject newItem = Instantiate(profilePrefab) as GameObject;
            newItem.name = gameObject.name + users[i];

            newItem.transform.SetParent(gameObject.transform);

            Button btn = newItem.GetComponentInChildren<Button>();
            Text btnText = btn.GetComponentInChildren<Text>();

            btn.onClick.AddListener(() => { Main_Menu.uid = btnText.text; hideUsersScroll = true; Main_Menu.textField = false; });

            btnText.text = users[i];
        }

        float scrollHeight = height * totalProfiles;
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
        RectTransform scrollableRectTransform = gameObject.GetComponentInParent<RectTransform>();
        Vector2 width = scrollableRectTransform.sizeDelta;
        containerRectTransform.sizeDelta = new Vector2(width.x, scrollHeight);
        hideUsersScroll = true;
    }
}