using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class All_AirPlanes
{
    private static List<AirPlane> _m_lsAllIrPlanes = new List<AirPlane>();
    public static List<AirPlane> m_lsAllIrPlanes
    {
        get{
            Init();
            return _m_lsAllIrPlanes;
        }
        private set
        {
            Init();
            m_lsAllIrPlanes = value;
        }
    }
    private static bool m_bInit = false;

    private static void Init()
    {
        if (m_bInit) return;
        m_bInit = true;
        m_lsAllIrPlanes.AddRange(Resources.LoadAll("AirPlane/", typeof(AirPlane)));
    }


}
