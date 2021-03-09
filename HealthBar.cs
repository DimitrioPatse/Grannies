using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Health healthComponent = null;
    [SerializeField] RectTransform foreground = null;
    Canvas rootCanvas;

    private void Start()
    {
        healthComponent = GetComponentInParent<Health>();
        rootCanvas = GetComponent<Canvas>();
        UpdateHealthBar();
    }
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void UpdateHealthBar()
    {
        if (Mathf.Approximately(healthComponent.GetHealthPersentage(), 0) || Mathf.Approximately(healthComponent.GetHealthPersentage(), 100))
        {
            rootCanvas.enabled = false;
            return;
        }

        rootCanvas.enabled = true;
        foreground.localScale = new Vector3(healthComponent.GetHealthPersentage() / 100, foreground.localScale.y, foreground.localScale.z);
    }
}
