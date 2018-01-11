using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionsNavigation : MonoBehaviour {

    public GameObject[] panelsEN = new GameObject[7];
    public GameObject[] panelsPT = new GameObject[7];
    public GameObject nextBtn, prevBtn;
    public Text instructionsTitle;

    int currentPanel = 0;

    void Update()
    {
        instructionsTitle.text = Language.instructions;

        if(Language.lang == "EN")
            panelsEN[currentPanel].SetActive(true);
        else
            panelsPT[currentPanel].SetActive(true);

        if (currentPanel == 0)
            prevBtn.SetActive(false);
        else
            prevBtn.SetActive(true);

        if (currentPanel == 6)
            nextBtn.SetActive(false);
        else
            nextBtn.SetActive(true);
    }
	
    public void NextPanel(int panel)
    {
        if (currentPanel < 6)
        {
            currentPanel = currentPanel + panel;
            SetPanel();    
        }   
    }

    public void PreviousPanel(int panel)
    {
        if (currentPanel > 0)
        {
            currentPanel = currentPanel - panel;
            SetPanel();    
        }
    }

    void SetPanel()
    {
        for (int i = 0; i < panelsEN.Length; i++)
        {
            if (i != currentPanel)
            {
                if (Language.lang == "EN")
                {
                    if (panelsEN[i].activeSelf)
                        panelsEN[i].SetActive(false);
                }
                else
                {
                    if (panelsPT[i].activeSelf)
                        panelsPT[i].SetActive(false);
                }
            }
        }
    }
}
