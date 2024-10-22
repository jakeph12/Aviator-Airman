using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Menu_Controller : MonoBehaviour
{
    private float m_flTime = 1;
    [HideInInspector]
    public Vector2 m_vcStartPos;
    [SerializeField] private GameObject m_gmFiledBlock;
    [SerializeField] private Text m_txCoin, m_txLvl, m_txPlane;
    public Action m_acCallBackOnEnd;
    [SerializeField]
    private Slider m_slUpgrade;
    [SerializeField] private Image m_imMainPlane;

    public bool m_Addition = false;
    public void Start()
    {
        m_vcStartPos = transform.localPosition;
        m_gmFiledBlock.SetActive(false);
        if (m_Addition) return;
        if (m_txCoin)
        {
            OnCoinCh(Player_Menager.m_inCoin);
            Player_Menager.m_evOnCoinChg += OnCoinCh;
            m_acCallBackOnEnd += () => Player_Menager.m_evOnCoinChg -= OnCoinCh;
        }


        if (m_slUpgrade)
        {
            Player_Menager.m_evOnAlreadyBoughtChg += OnUpCh;
            m_acCallBackOnEnd += () => Player_Menager.m_evOnAlreadyBoughtChg -= OnUpCh;
        }

        if (m_txLvl || m_txPlane || m_imMainPlane)
        {
            OnPlaneCh(Player_Menager.m_airCurPlane);
            Player_Menager.m_evOnPlaneChg += OnPlaneCh;
            m_acCallBackOnEnd += () => Player_Menager.m_evOnPlaneChg -= OnPlaneCh;
        }

    }

    public void OnUpCh(int value)
    {
        if (Player_Menager.m_inIdOfPlane < Player_Menager.m_inCurLvl) m_slUpgrade.value = 10;
        else if (Player_Menager.m_inIdOfPlane == Player_Menager.m_inCurLvl && Player_Menager.m_inCurLvl >= All_AirPlanes.m_lsAllIrPlanes.Count - 1) m_slUpgrade.value = 10;
        else m_slUpgrade.value = Player_Menager.m_inAlreadyBought;
    }

    private void OnCoinCh(int coin)
    {
        m_txCoin.text = coin.ToString();
    }
    public void OnPlaneCh(AirPlane plane)
    {
        var c = plane.name.Split('.');
        if (m_txLvl)
        {
            m_txLvl.text =$"Level {Convert.ToInt32(c[0]) +1}";
        }
        if (m_txPlane)
        {
            m_txPlane.text = $"{c[1]}";
        }
        if (m_imMainPlane)
        {
            m_imMainPlane.sprite = plane.m_spPlaneIco;
        }
        if (m_slUpgrade)
        {
            if (Player_Menager.m_inIdOfPlane < Player_Menager.m_inCurLvl) m_slUpgrade.value = 10;
            else if (Player_Menager.m_inIdOfPlane == Player_Menager.m_inCurLvl && Player_Menager.m_inCurLvl >= All_AirPlanes.m_lsAllIrPlanes.Count -1) m_slUpgrade.value = 10;
            else m_slUpgrade.value = Player_Menager.m_inAlreadyBought;
        }
    }


    public void OnMenuBarClickDif()
    {

    }

    public void OnMenuBarClick(GameObject Pannel)
    {
        if (m_gmFiledBlock) m_gmFiledBlock.SetActive(true);
        var p = transform.GetComponentInParent<RectTransform>();
        var n = Instantiate(Pannel, p.transform);
        float st = n.transform.localPosition.x;
        n.transform.DOLocalMove(Vector2.zero, m_flTime).SetEase(Ease.InOutBack);
        var str = n.GetComponent<Main_Menu_Controller>();

        if (str)
        {
            str.m_vcStartPos = m_vcStartPos;
            str.m_acCallBackOnEnd += () => m_gmFiledBlock.SetActive(false);
        }
    }

    public GameObject OnMenuBarClickC(GameObject Pannel)
    {
        if (m_gmFiledBlock) m_gmFiledBlock.SetActive(true);
        var p = transform.GetComponentInParent<RectTransform>();
        var n = Instantiate(Pannel, p.transform);
        float st = n.transform.localPosition.x;
        n.transform.DOLocalMove(Vector2.zero, m_flTime).SetEase(Ease.InOutBack);
        var str = n.GetComponent<Main_Menu_Controller>();

        if (str)
        {
            str.m_vcStartPos = m_vcStartPos;
            str.m_acCallBackOnEnd += () => m_gmFiledBlock.SetActive(false);
        }
        return n;
    }


    public void close()
    {
        if (m_gmFiledBlock) m_gmFiledBlock.SetActive(true);
        transform.DOLocalMove(m_vcStartPos, m_flTime / 2).SetEase(Ease.InOutBack).OnComplete(() => Destroy(gameObject));
        m_acCallBackOnEnd?.Invoke();
    }


    public void RateUpp() => Application.OpenURL("https://apps.apple.com/us/app/minecraft/id479516143");
}
