using UnityEngine;

public class QuitGame : PopupWithPrompt
{
    // Checks
    private static bool quitPopupToggled = false;

    // Getters
    public static bool GetQuitStatus() {
        return quitPopupToggled;
    } 

    // Setters
    public static void SetQuitStatus(bool boolean) {
        quitPopupToggled = boolean;
    }

    // Update is called once per frame
    void Update() { 
        if (Input.GetKeyUp(KeyCode.Escape) && gameObject.CompareTag("Mainmenu")) {
            SetQuitStatus(true);
        }       

        setToggle(quitPopupToggled);
        setYes(Input.GetKeyUp(KeyCode.Y));
        setNo(Input.GetKeyUp(KeyCode.N));

        if (!MainMenuButtons.GetNewGamePressed()) {
            ToggleUI();
        }
    }

    override public void YesAction() {
        Application.Quit();

        // Ensure it stops playing in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        base.YesAction();
    }

    public override void NoAction()
    {
        SetQuitStatus(false);
        base.NoAction();
    }
}
