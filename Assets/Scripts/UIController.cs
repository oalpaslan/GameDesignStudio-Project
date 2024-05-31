using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider slider;
    public Image border, vampSpd, vampVis;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ManageBloodUI();
        ManagePowerUI();
    }

    private void ManageBloodUI()
    {
        if (slider != null)
        {

            slider.value = PlayerController2.instance.bloodAmount;
        }
    }

    private void ManagePowerUI()
    {
        if (PlayerController2.instance.isVampSpdEnabled)
        {

            border.enabled = true;
            vampSpd.enabled = true;
            vampVis.enabled = false;
        }
        else if (PlayerController2.instance.isVampVisEnabled)
        {
            border.enabled = true;
            vampVis.enabled = true;
            vampSpd.enabled = false;
        }
        else
        {
            border.enabled = false;
            vampSpd.enabled = false;
            vampVis.enabled = false;
        }

    }
}
