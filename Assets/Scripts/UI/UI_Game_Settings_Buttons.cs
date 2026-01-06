using UnityEngine;
using UnityEngine.UI;

public class UI_Game_Settings_Buttons : MonoBehaviour
{
    [SerializeField] private string keyName;

    private Button thisButton;
    private Image thisButtonImage;

    public Color btnEnabledCol;
    public Color btnDisabledCol;

    public Image iconImage; 

    public Sprite unMuteIcon;
    public Sprite muteIcon;

    private void Start()
    {
        thisButton = gameObject.GetComponent<Button>();
        thisButtonImage = thisButton.GetComponent<Image>();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt(keyName) == 0)
        {
            thisButtonImage.color = btnEnabledCol;
            iconImage.sprite = unMuteIcon;
        }
        else
        {
            thisButtonImage.color = btnDisabledCol;
            iconImage.sprite = muteIcon;
        }
    }
}
