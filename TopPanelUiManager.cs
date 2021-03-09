using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelUiManager : MonoBehaviour
{
    [SerializeField]  Text butteries, money, gems;
    [SerializeField] GameObject topPanel;
    Wallet wallet;

    private void Start()
    {
        wallet = FindObjectOfType<Wallet>();
        Invoke("UpdateValues", 0.1f);
    }

    public void UpdateValues()
    {
        //butteries.text = butteries
        money.text = wallet.GetMoney().ToString();
        gems.text = wallet.GetSpecial().ToString();

    }
    public void HideTopPanel()
    {
        topPanel.SetActive(false);
    }
    public void ShowTopPanel()
    {
        topPanel.SetActive(true);
    }
}
