using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIMAnager : MonoBehaviour
{
    [SerializeField] Button fxdJoystickButton;

    // Update is called once per frame
    void Start()
    {
        SetValues();
    }
    public void InvertFixedJoystick()
    {
        PlayerPrefsManager.InvertFixedJoystick();
        fxdJoystickButton.image.color = (PlayerPrefsManager.GetFixedJoystick() == 1) ? Color.green : Color.red;
    }

    void SetValues()
    {
        fxdJoystickButton.image.color = (PlayerPrefsManager.GetFixedJoystick() == 1) ? Color.green : Color.red;
    }
}
