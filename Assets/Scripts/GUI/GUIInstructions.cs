using UnityEngine;
using System.Collections;

public class GUIInstructions : MonoBehaviour {

    private float hScrollBarValue;
    public Vector2 scrollPosition = Vector2.zero;

    public GUISkin CTVR;
    GUIStyle greenButton;
    GUIStyle scrollView;
    GUIStyle titleFont;

    public Texture2D scrollViewInternalBG;
    public Texture2D instructionsTextEN;
    public Texture2D instructionsTextPT;
    Vector2 contentOffset;

    void OnGUI()
    {
        GUI.skin = CTVR;
        greenButton = GUI.skin.GetStyle("CTVRGreenButton");
        scrollView = GUI.skin.GetStyle("CTVRScrollView");
        titleFont = GUI.skin.GetStyle("CTVRTitleFont");
        titleFont.fontSize = Screen.width / 35;
        greenButton.fontSize = Screen.width / 65;
        contentOffset.y = Screen.height / 150;
        greenButton.contentOffset = contentOffset;

        //instructions title
        GUI.Label(new Rect(Screen.width * 0.09f, Screen.height * 0.14f, Screen.width * 0.4f, Screen.height * 0.1f), Language.instructions, titleFont);

        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", scrollView);
        GUI.DrawTexture(new Rect(Screen.width * 0.025f, Screen.height * 0.15f, Screen.width * 0.95f, Screen.height * 0.8f), scrollViewInternalBG);

        scrollPosition = GUI.BeginScrollView(new Rect(Screen.width * 0.025f, Screen.height * 0.17f, Screen.width * 0.95f, Screen.height * 0.76f), scrollPosition, new Rect(0, 0, Screen.width * 0.91f, 4 * Screen.height));

        Texture2D instructionsText;

        if (Language.lang == "EN")
        {
            instructionsText = instructionsTextEN;
        }
        else
        {
            instructionsText = instructionsTextPT;
        }

        GUI.DrawTexture(new Rect(0, 0, Screen.width * 0.95f, 4 * Screen.height), instructionsText);

        GUI.EndScrollView();

        //go back to main menu button
        if (GUI.Button(new Rect(Screen.width * 0.85f, Screen.height * 0.04f, Screen.width * 0.13f, Screen.height * 0.098f), Language.main, greenButton))
        {
            //Application.LoadLevel(0);
            this.gameObject.SetActive(false);
        }
        
    }
}
