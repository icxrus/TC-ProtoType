using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibilityOfOutline : MonoBehaviour
{
    [SerializeField] private MouseOverEvent mouseOver;

    private void Awake()
    {
        mouseOver.SelectingCountryOnMapCallBack += ToggleVisibility;
        mouseOver.ExitingCountryOnMapCallBack += ToggleVisibilityOff;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void ToggleVisibility(GameObject o)
    {
        if (o.name == gameObject.name)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if (o.transform.childCount >= 2)
            {
                if (o.transform.GetChild(1).GetChild(0).GetComponent<UIAutoAnimation>())
                {
                    o.transform.GetChild(1).GetChild(0).GetComponent<UIAutoAnimation>().EntranceAnimation();
                }

            }
        }
    }

    private void ToggleVisibilityOff(GameObject o)
    {
        if (o.name == gameObject.name)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            if (o.transform.childCount >= 2)
            {
                if (o.transform.GetChild(1).GetChild(0).GetComponent<UIAutoAnimation>())
                {
                    o.transform.GetChild(1).GetChild(0).GetComponent<UIAutoAnimation>().ExitAnimation();
                }
            }
        }
    }
}
