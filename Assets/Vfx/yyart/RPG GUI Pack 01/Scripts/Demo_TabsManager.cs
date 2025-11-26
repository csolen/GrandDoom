using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace yyart
{
    public class TabsManager : MonoBehaviour
    {
        public GameObject[] Tabs;
        public Image[] TabButtons;
        public Sprite InactiveTabBG, ActiveTabBG;

        public void SwitchToTab(int TabID)
        {
            foreach (GameObject go in Tabs)
            {
                go.SetActive(false);
            }
            Tabs[TabID].SetActive(true);
            
            foreach (Image im in TabButtons)
            {
                im.sprite = InactiveTabBG;
            }
            TabButtons[TabID].sprite = ActiveTabBG;
        }
    }
}
