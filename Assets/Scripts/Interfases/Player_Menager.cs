using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Menager
{
    const string PlayerPropertyCoin = "Player_coin";
    const string PlayerPropertyPlane = "Player_curent_plane";
    const string PlayerPropertyPlaneBought = "Player_bought_plane";
    const string PlayerPropertyPlaneUpgradeBought = "Player_bought_upgrade_plane";

    private static int _m_inCoin;
    public static int m_inCoin
    {
        get
        {
            Init();
            return _m_inCoin;
        }
        set
        {
            Init();
            var t = value - m_inCoin;
            _m_inCoin = value;

            if(t != 0)
                PlayerPrefs.SetInt(PlayerPropertyCoin, _m_inCoin);

            m_evOnCoinChg?.Invoke(_m_inCoin);
        }
    }
    private static int _m_inAlreadyBought;
    public static int m_inAlreadyBought
    {
        get 
        {
            Init();
            return _m_inAlreadyBought;
        } 
        set
        {
            Init();
            _m_inAlreadyBought = value;
            if (_m_inAlreadyBought >= 10)
            {
                m_inCurLvl++;
                _m_inAlreadyBought = 0;
                m_inIdOfPlane = m_inCurLvl;
                PlayerPrefs.SetInt(PlayerPropertyPlaneBought, m_inCurLvl);
            }
            m_evOnAlreadyBoughtChg?.Invoke(_m_inAlreadyBought);
            PlayerPrefs.SetInt(PlayerPropertyPlaneUpgradeBought, _m_inAlreadyBought);


        }
    }

    public delegate void OnAlreadyBoughtCh(int up);
    public static event OnAlreadyBoughtCh m_evOnAlreadyBoughtChg;

    private static int _m_inIdOfPlane =-1;
    private static AirPlane _m_airCurPlane;
    public static AirPlane m_airCurPlane
    {
        get{
            Init();
            return _m_airCurPlane;
        }
        private set
        {
            Init();
            _m_airCurPlane = value;
            m_evOnPlaneChg?.Invoke(_m_airCurPlane);
        }
    }
    public static int m_inIdOfPlane
    {
        get
        {
            Init();
          return  _m_inIdOfPlane;
        }
        set
        {
            Init();
            if (value >= All_AirPlanes.m_lsAllIrPlanes.Count) return;

            var t = value - _m_inIdOfPlane;
            _m_inIdOfPlane = value;
            if(t != 0)
            {
                m_airCurPlane = All_AirPlanes.m_lsAllIrPlanes[_m_inIdOfPlane];
                PlayerPrefs.SetInt(PlayerPropertyPlane, _m_inIdOfPlane);
            }


        }
    }
    public static int _m_inCurLvl = -1;
    public static int m_inCurLvl
    {
        get
        {
            Init();
            return _m_inCurLvl;
        }
        set
        {
            Init();
            var t = value - _m_inCurLvl;
            _m_inCurLvl = value;
            if (_m_inCurLvl > All_AirPlanes.m_lsAllIrPlanes.Count - 1) _m_inCurLvl = All_AirPlanes.m_lsAllIrPlanes.Count - 1;
            //if (t != 0)

        }
    }



    public delegate void OnCoinCh(int coin);
    public static event OnCoinCh m_evOnCoinChg;

    public delegate void OnPlaneCh(AirPlane plane);
    public static event OnPlaneCh m_evOnPlaneChg;


    private static bool m_bInit = false;

    private static void Init()
    {
        if (m_bInit) return;
        m_bInit = true;
        m_inCoin = PlayerPrefs.GetInt(PlayerPropertyCoin, 0);
        m_inIdOfPlane = PlayerPrefs.GetInt(PlayerPropertyPlane, 0);
        m_inCurLvl = PlayerPrefs.GetInt(PlayerPropertyPlaneBought, 0);
        m_inAlreadyBought = PlayerPrefs.GetInt(PlayerPropertyPlaneUpgradeBought, 0);


    }


}
