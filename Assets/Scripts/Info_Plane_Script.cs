using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Info_Plane_Script : MonoBehaviour
{
    [SerializeField]
    private List<AirPlane> m_gLsPlanes = new List<AirPlane>();
    [SerializeField]
    private Image m_spMain, m_spLeft, m_spRight;
    [SerializeField]
    private Text m_txName,m_txLvl,m_txDescription;
    [SerializeField]
    private Sprite m_spCan, m_spCoudnt;
    [SerializeField]
    private GameObject m_Up;

    private int _m_inIndex = 0;
    private int m_inIndex
    {
        get => _m_inIndex;
        set
        {
            var t = value - _m_inIndex;
            _m_inIndex = value;

            if(_m_inIndex <0) _m_inIndex = m_gLsPlanes.Count-1;
            if(m_gLsPlanes.Count <= _m_inIndex) _m_inIndex = 0;

            var im = GetImg(_m_inIndex);




            m_spLeft.sprite = im[0];
            m_spMain.sprite = im[1];
            m_spRight.sprite = im[2];


            if (m_txName) m_txName.text = m_gLsPlanes[_m_inIndex].name.Split('.')[1];
            if (m_txLvl) m_txLvl.text = $"Level {_m_inIndex +1}";

            var cl = new Color32(15, 15, 15, 230);

            if (Ch(m_inIndex)) m_spMain.color = Color.white;
            else m_spMain.color = cl;
            if (Ch(m_inIndex + 1)) m_spRight.color = Color.white;
            else m_spRight.color = cl;
            if (Ch(m_inIndex - 1)) m_spLeft.color = Color.white;
            else m_spLeft.color = cl;

            if (m_inIndex <= Player_Menager.m_inCurLvl)
                Player_Menager.m_inIdOfPlane = m_inIndex;

        }
    }

    private void Start()
    {
        m_gLsPlanes = All_AirPlanes.m_lsAllIrPlanes;
        m_inIndex = Player_Menager.m_inIdOfPlane;

        if (Ch(m_inIndex)) m_spMain.transform.parent.GetComponent<Image>().sprite = m_spCan;
        else m_spMain.transform.parent.GetComponent<Image>().sprite = m_spCoudnt;

        if (Ch(m_inIndex + 1)) m_spRight.transform.parent.GetComponent<Image>().sprite = m_spCan;
        else m_spRight.transform.parent.GetComponent<Image>().sprite = m_spCoudnt;

        if (Ch(m_inIndex - 1)) m_spLeft.transform.parent.GetComponent<Image>().sprite = m_spCan;
        else m_spLeft.transform.parent.GetComponent<Image>().sprite = m_spCoudnt;

        if (m_txDescription) m_txDescription.text = m_gLsPlanes[_m_inIndex].m_strDescr;
    }

    public void Next()
    {
        GameObject[] all = new GameObject[3];
        all[0] = m_spLeft.gameObject;
        all[1] = m_spMain.gameObject;
        all[2] = m_spRight.gameObject;
        
        Move(all,0.8f);
    }
    public void Previous() 
    {
        GameObject[] all = new GameObject[3];
        all[0] = m_spRight.gameObject;
        all[1] = m_spMain.gameObject;
        all[2] = m_spLeft.gameObject;

        Move(all, 0.8f,true);

    }
    public Sprite[] GetImg(int index)
    {
        Sprite[] Main = new Sprite[3];


        if (index == 0)
        {
            Main[0] = m_gLsPlanes[m_gLsPlanes.Count - 1].m_spPlaneIco;
            Main[1] = m_gLsPlanes[index].m_spPlaneIco;
            Main[2] = m_gLsPlanes[index+1].m_spPlaneIco;
        }else if(index == m_gLsPlanes.Count - 1)
        {
            Main[0] = m_gLsPlanes[index -1].m_spPlaneIco;
            Main[1] = m_gLsPlanes[index].m_spPlaneIco;
            Main[2] = m_gLsPlanes[0].m_spPlaneIco;
        }
        else
        {
            Main[0] = m_gLsPlanes[index - 1].m_spPlaneIco;
            Main[1] = m_gLsPlanes[index].m_spPlaneIco;
            Main[2] = m_gLsPlanes[index+1].m_spPlaneIco;
        }




        return Main;
    }

    private bool Playing = false;

    private bool Init = false;
    public void Move(GameObject[] gm,float speed = 1,bool rev = false)
    {
        if (!Init)
        {
            Init = true;
            m_spRight.transform.parent.gameObject.SetActive(true);
            m_spLeft.transform.parent.gameObject.SetActive(true);
            GetComponent<Main_Menu_Controller>().m_acCallBackOnEnd += () =>
            {
                m_spRight.transform.parent.gameObject.SetActive(false);
                m_spLeft.transform.parent.gameObject.SetActive(false);
            };
        }
        if (Playing) return;
        Playing = true;

        var posleft = gm[0].transform.parent.localPosition;
        var posMain = gm[1].transform.parent.localPosition;
        var posRight = gm[2].transform.parent.localPosition;

        gm[0].transform.parent.localPosition = posRight;
        gm[1].transform.parent.DOLocalMoveX(posleft.x, speed).SetEase(Ease.Linear);
        gm[1].transform.parent.DOScale(0.5f, speed).SetEase(Ease.Linear);
        gm[2].transform.parent.DOScale(1f, speed).SetEase(Ease.Linear);
        gm[2].transform.parent.DOLocalMoveX(posMain.x, speed).SetEase(Ease.Linear).OnComplete(() => {
            Playing = false;
            if (rev)
            {
                m_spRight = gm[1].GetComponent<Image>();
                m_spLeft = gm[0].GetComponent<Image>();
                m_spMain = gm[2].GetComponent<Image>();




                m_inIndex--;


                if (Ch(m_inIndex - 1)) m_spLeft.transform.parent.GetComponent<Image>().sprite = m_spCan;
                else m_spLeft.transform.parent.GetComponent<Image>().sprite = m_spCoudnt;
            }
            else
            {
                m_spRight = gm[0].GetComponent<Image>();
                m_spLeft = gm[1].GetComponent<Image>();
                m_spMain = gm[2].GetComponent<Image>();


                m_inIndex++;
                if(Ch(m_inIndex+1)) m_spRight.transform.parent.GetComponent<Image>().sprite = m_spCan;
                else m_spRight.transform.parent.GetComponent<Image>().sprite = m_spCoudnt;




            }

            });

        if (rev)
        {
            if (m_txDescription)
                if(_m_inIndex - 1 >= 0)
                    Main_Tutor.Generate(m_gLsPlanes[_m_inIndex - 1].m_strDescr, m_txDescription, 0.5f).Forget();
                else
                    Main_Tutor.Generate(m_gLsPlanes[m_gLsPlanes.Count -1].m_strDescr, m_txDescription, 0.5f).Forget();

        }
        else
        {
            if (m_txDescription)
                if (_m_inIndex + 1 >= m_gLsPlanes.Count)           
                    Main_Tutor.Generate(m_gLsPlanes[0].m_strDescr, m_txDescription, 0.5f).Forget();
                else
                     Main_Tutor.Generate(m_gLsPlanes[_m_inIndex + 1].m_strDescr, m_txDescription, 0.5f).Forget();

        }

    }
    private bool Ch(int indexss)
    {
        if(indexss < 0)
        {
            if (m_gLsPlanes.Count - 1 <= Player_Menager.m_inCurLvl) return true;
            else return false;
        }else if(indexss > m_gLsPlanes.Count - 1) 
        {
            if (0 <= Player_Menager.m_inCurLvl) return true;
            else return false;
        }
        else
        {
            if (indexss <= Player_Menager.m_inCurLvl) return true;
            else return false;

        }

    }
    public void OnDestroy()
    {
    }

    public void GetUpdate()
    {
        if (Playing) return;
        var n = GetComponent<Main_Menu_Controller>().OnMenuBarClickC(m_Up);
        var c = n.GetComponent<Main_Upgrade>();
        c.m_bLoad = true;
        c.SetP(All_AirPlanes.m_lsAllIrPlanes[m_inIndex], true);
        n.GetComponent<Main_Menu_Controller>().m_Addition = true;
        c.Setlabel(m_txName.text,m_txLvl.text);

    }
}

