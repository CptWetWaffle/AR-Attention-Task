using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;
using Assets.Scripts.GameLogic;
using GameLogic;

public class General_Settings : MonoBehaviour {
	
	public GameObject LArm, RArm;
	public static bool enable = false, loadCalibValues = false;

	void Awake()
	{
		LoadConfig();
		
	}

	void Update()
	{
		if (enable)
		{
			/*ikLimbLeft.IsEnabled = Main_Menu.LArm;
			ikLimbRight.IsEnabled = Main_Menu.RArm;

			if (ikLimbLeft.IsEnabled)
			{
				LArm.transform.Rotate(0, 90, 0);
				RArm.transform.Rotate(0, 0, 0);
			}
			else if (ikLimbRight.IsEnabled)
			{
				RArm.transform.Rotate(0, 270, 0);
				LArm.transform.Rotate(0, 0, 0);
			}
			else
			{
				RArm.transform.Rotate(0, 0, 0);
				LArm.transform.Rotate(0, 0, 0);
			}*/

			enable = false;
			
		}


		
	}

	public static void LoadConfig()
	{
		string filepath = Application.dataPath + @"/Settings/config.xml";
		XmlDocument xmlDoc = new XmlDocument();

		if (File.Exists(filepath))
		{
			xmlDoc.Load(filepath);
			
			XmlNodeList configList = xmlDoc.GetElementsByTagName("Config");

			foreach (XmlNode configInfo in configList)
			{
				XmlNodeList xmlcontent = configInfo.ChildNodes;

				foreach (XmlNode xmlsettings in xmlcontent)
				{
					if (xmlsettings.Name == "Session")
						CountSessionTime.sessionTime = float.Parse(xmlsettings.InnerText);
				}//foreach xmlcontent
			}//foreach configList	
		}//if file exists 
	}


	
}