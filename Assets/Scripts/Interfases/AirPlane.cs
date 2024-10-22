using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New AirPlane", menuName = "Game/New Object/AitPlane")]
public class AirPlane : ScriptableObject, PlaneInfo
{
    [SerializeField]
    private Sprite m_spMainIco;
    [SerializeField]
    private string m_strDescript;
    public List<PlaneUpgrade> m_scUpgrades = new List<PlaneUpgrade>();



    public Sprite m_spPlaneIco { get => m_spMainIco; set => m_spMainIco = value; }
    public string m_strDescr { get=> m_strDescript; set => m_strDescript = value; }
}
public interface PlaneInfo
{
    public Sprite m_spPlaneIco { get; set; }
    public string m_strDescr { get; set; }
}

[System.Serializable]
public class PlaneUpgrade
{
    public string m_strName, m_strDescript;
    public int m_inCost;
}