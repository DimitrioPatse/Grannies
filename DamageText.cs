using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText = null;

    public void SetValue(float amount)
    {
        damageText.text = amount.ToString();
    }
    public void SetValue(string text)
    {
        damageText.text = text;
    }
}
