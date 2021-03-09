using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] RectTransform panelHolder;

    [SerializeField] float moveX = 900;
    [SerializeField] float movementTime = 0.2f;
    [SerializeField] LeanTweenType type;

    TopPanelUiManager topPanel;

    void Start()
    {
        topPanel = GetComponent<TopPanelUiManager>();
    }
    public void MoveToPanel(int panelPlaceOrder)
    {
        if(panelPlaceOrder == -2 || panelPlaceOrder == 2)
        {
            topPanel.HideTopPanel();
        }
        else
        {
            topPanel.ShowTopPanel();
        }
        LeanTween.moveX(panelHolder, moveX * -panelPlaceOrder, movementTime).setEase(type);
    }
}
