using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class Main_Tutor : MonoBehaviour
{
    [SerializeField]
    private Text m_txName, m_txDescr, m_txBt;
    [SerializeField]
    private Image m_mgMain;
    [SerializeField]
    private Sprite[] m_spMain;
    [SerializeField]
    private GameObject m_gmStartPos;


    private string[] m_strDescr =
    {
        "Earn points by completing missions and use them to unlock powerful aircraft and advanced upgrades.",
        "Test your skills in high-speed aerial chases and intense dogfights against enemy fighters.",
        "Think fast and make quick decisions to outsmart your enemies and keep flying longer."
    };

    private string[] m_strName =
    {
        "Upgrade Your Fleet",
        "Engage in Thrilling Battles",
        "Stay Sharp",
    };

    void Start()
    {
        
    }
    private bool m_bPlay;
    private int m_inIndex = 0;
    public void Next()
    {
        if (m_bPlay) return;
        if (m_inIndex == m_strDescr.Length - 1)
        {
            GetComponent<Main_Menu_Controller>().close();
            return;
        }

        if(m_inIndex == m_strDescr.Length - 2)
        {
            m_txBt.text = "Start Flying";
        }
        else
        {
            m_txBt.text = "Continue";
        }

        var New = Instantiate(m_mgMain, m_mgMain.transform.parent.transform);

        New.GetComponent<Image>().sprite = m_spMain[m_inIndex];

        New.transform.localPosition = m_gmStartPos.transform.localPosition;

        New.transform.DOLocalMove(m_mgMain.transform.localPosition,1).OnComplete(() => 
        { 
            Destroy(m_mgMain);
            m_mgMain = New;
        });
        m_mgMain.transform.DOLocalMove(New.transform.localPosition,1).OnComplete(() => m_bPlay = false);
        Generate(m_strDescr[m_inIndex], m_txDescr, 0.5f).Forget();
        Generate(m_strName[m_inIndex], m_txName, 0.5f).Forget();

        m_inIndex++;
        m_bPlay = true;
    }


    public static async UniTask Generate(string st,Text tx,float second)
    {
        var c = st.ToCharArray();
        int t = (int)((second * 1000) / c.Length);
        tx.text = "";
        foreach (char cr in c)
        {
            tx.text += cr;
            await UniTask.Delay(t);
        }

    }
}
