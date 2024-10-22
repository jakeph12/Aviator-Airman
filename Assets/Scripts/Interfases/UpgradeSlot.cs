using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField]
    private Text m_txName,m_txDescr,m_txCoinPrise;
    public GameObject m_btBuy;


    public void SetLabel(string Name,string Descr,string Price)
    {
        m_txName.text = Name;
        m_txDescr.text = Descr;
        m_txCoinPrise.text = Price;
    }

}
