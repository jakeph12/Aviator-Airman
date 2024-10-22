using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class Main_Daily_Reward : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gmListOfSots;
    [SerializeField]
    private Sprite m_spDone, m_SpClaim;
    [SerializeField]
    private Button m_btMain;

    private int[] m_inPrice =
    {
        200,
        500,
        1000,
        1500,
        2000,
        2500,
        3000,
        3500,
        4000,
        5000,
    };


    void Start()
    {
        var dateCurrentH = DateTime.Now.Hour;
        var dateCurrentD = DateTime.Now.Day;
        var dateCurrentM = DateTime.Now.Month;
        var curlvl = PlayerPrefs.GetInt("PlayerDailyR", 0);
        var dateAldH = PlayerPrefs.GetInt("PlayerH", dateCurrentH);
        var dateAldD = PlayerPrefs.GetInt("PlayerD", dateCurrentD -1);
        var dateAldM = PlayerPrefs.GetInt("PlayerM", dateCurrentM);

        if(curlvl != 0)
            for (int i = 0;i < curlvl;i++)
            {
                var t = i;
                m_gmListOfSots.transform.GetChild(t).GetComponent<Image>().sprite = m_spDone;
            }
        m_gmListOfSots.transform.GetChild(curlvl).GetComponent<Image>().sprite = m_SpClaim;


        var rr = dateCurrentD - dateAldD;

        if (rr < 1) return;
        m_btMain.interactable = true;
        m_btMain.onClick.AddListener(() =>
        {

            Player_Menager.m_inCoin += m_inPrice[curlvl];
            m_gmListOfSots.transform.GetChild(curlvl).GetComponent<Image>().sprite = m_spDone;
            curlvl++;
            m_gmListOfSots.transform.GetChild(curlvl).GetComponent<Image>().sprite = m_SpClaim;
            PlayerPrefs.SetInt("PlayerDailyR", curlvl);
            PlayerPrefs.SetInt("PlayerD", dateCurrentD);
            m_btMain.interactable = false;

        });



    }
}
