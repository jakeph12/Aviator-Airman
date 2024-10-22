using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Upgrade : MonoBehaviour
{
    private AirPlane m_plPlane;
    [SerializeField]
    private GameObject m_gmPannelOfAllSlots;
    public bool m_bLoad = false;
    [SerializeField]
    private Text m_txName,m_txLvl;

    void Start()
    {
        if(!m_bLoad)
            Rel(Player_Menager.m_airCurPlane);
        Player_Menager.m_evOnPlaneChg += Rel;
    }
    void Rel(AirPlane ple)=> SetP(ple);

    public void SetP(AirPlane ple,bool tes = false)
    {
        m_plPlane = ple;
        for (int i = 0; i < m_gmPannelOfAllSlots.transform.childCount; i++)
        {
            var t = i;
            var c = m_gmPannelOfAllSlots.transform.GetChild(t).GetComponent<UpgradeSlot>();
            var pl = m_plPlane.name.Split('.')[1];

            if (m_plPlane.m_scUpgrades.Count > t)
            {
                var up = m_plPlane.m_scUpgrades[t];
                var answer = PlayerPrefs.GetInt($"{pl}.{up.m_strName}", 0);



                if (up != null)
                    c.SetLabel(up.m_strName, up.m_strDescript, up.m_inCost.ToString());

                if (answer == 1 || tes)
                {
                    c.m_btBuy.SetActive(false);
                    continue;
                }
                else
                {
                    c.m_btBuy.SetActive(true);
                }

                c.m_btBuy.GetComponent<Button>().onClick.RemoveAllListeners();

                c.m_btBuy.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (up.m_inCost > Player_Menager.m_inCoin) return;
                    c.m_btBuy.SetActive(false);
                    PlayerPrefs.SetInt($"{pl}.{up.m_strName}", 1);
                    Player_Menager.m_inCoin -= up.m_inCost;
                    Player_Menager.m_inAlreadyBought++;
                });
            }
        }

    }
    public void Setlabel(string n, string lvl)
    {
        m_txName.text = n;
        m_txLvl.text = lvl;
    }
    private void OnDestroy()
    {
        Player_Menager.m_evOnPlaneChg -= Rel;
    }
}
