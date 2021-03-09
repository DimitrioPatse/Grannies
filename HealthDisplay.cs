using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] RectTransform foreground = null;
    [SerializeField] Text healthText;

    Health health;
    void Start()
    {
        health = GetComponentInParent<Health>();
        Invoke("UpdateBarValues",0.1f);
    }

    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
    public void UpdateBarValues()
    {
        healthText.text = String.Format("{0:0}", health.GetHP());
        foreground.localScale = new Vector3(health.GetHealthPersentage() / 100, foreground.localScale.y, foreground.localScale.z);
    }
}