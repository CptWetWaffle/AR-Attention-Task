using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public GameObject _loadingScreen;
	public Text Name;
	public Text Warning;
	private AsyncOperation ao;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void LoadLevel()
	{
		if (Name.text == "")
		{
			Warning.text = "Insert a name please: ";
			return;
		}
	    SaveExport.Instance.Filename = Name.text + ".csv";
		_loadingScreen.SetActive(true);
		StartCoroutine(PlayLevel());
	}

	private IEnumerator PlayLevel()
	{
		yield return new WaitForSeconds(1);

		ao = Application.LoadLevelAsync("L1");

		while (!ao.isDone)
		{
			yield return null;
		}
	}
}
