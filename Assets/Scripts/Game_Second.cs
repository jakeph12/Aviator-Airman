using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Second : MonoBehaviour
{
    private float m_flWay;

    [SerializeField]
    private Text m_txCoin,m_txQuestion;
    [SerializeField]
    private float m_flSpeed;
    [SerializeField]
    private GameObject m_gmPlayer, m_gmLeft,m_gmMiddle, m_gmRight;
    [SerializeField]
    private GameObject m_gmAnswer,m_gmAwnswered;
    [SerializeField]
    private GameObject m_gmPannelOf_all_Obj;


    private int _m_inCoin;
    private int m_inCoin
    {
        get => _m_inCoin;
        set
        {
            _m_inCoin = value;
            if (m_txCoin) m_txCoin.text = _m_inCoin.ToString();
        }
    }

    void Start()
    {
        StartPlay().Forget();
    }

    public async UniTask StartPlay()
    {
        m_gmPlayer.transform.localPosition = new Vector2(0, m_gmPlayer.transform.localPosition.y);
        m_bEnd = false;
        m_inCoin = 0;
        await GetAnswer();
       

    }

    public async UniTask GetAnswer()
    {
        if (m_gmPannelOf_all_Obj.transform.childCount > 0)
            for (int c = 0; c < m_gmPannelOf_all_Obj.transform.childCount; c++)
            {
                var t = c;
                Destroy(m_gmPannelOf_all_Obj.transform.GetChild(t).gameObject);
            }
        m_gmPlayer.GetComponent<Image>().sprite = Player_Menager.m_airCurPlane.m_spPlaneIco;

        var Ra = Random.Range(0, 4);

        var f = Random.Range(0, 100);
        var d = Random.Range(0, 100);
        var ff = 0;
        var ss = 0;

        if (f >= d)
        {
            ff = f;
            ss = d;
        }
        else
        {
            ff = d;
            ss = f;
        }
        string snw = ff.ToString();
        int Answerrt = 0;

        switch (Ra)
        {
            case 0:
                snw += "+";
                Answerrt = ff + ss;
                break;

            case 1:
                snw += "-";
                Answerrt = ff - ss;
                break;

            case 2:
                snw += "*";
                Answerrt = ff * ss;
                break;

            case 3:
                snw += "/";
                Answerrt = ff / ss;
                break;

        }
        var fr = Answerrt - 12;
        var sr = Answerrt + 12;

        List<int> tre = new List<int>();
        tre.Add(Answerrt);
        tre.Add(fr);
        tre.Add(sr);

        List<int> xp = new List<int>();

        xp.Add((int)m_gmLeft.transform.localPosition.x);
        xp.Add((int)m_gmMiddle.transform.localPosition.x);
        xp.Add((int)m_gmRight.transform.localPosition.x);

        snw += ss.ToString();


        m_txQuestion.text = snw;

        await UniTask.Delay(2000);

        var er = Random.Range(0,xp.Count);
        var p = xp[er];
        xp.RemoveAt(er);

        SpawnObj(m_gmAnswer, () =>
        {
            m_inCoin += 20;
            GetAnswer().Forget();
        },
        p,
        Answerrt
        );
        er = Random.Range(0, xp.Count);
        p = xp[er];
        xp.RemoveAt(er);

        SpawnObj(m_gmAnswer, () =>
        {
            OnEnd();
        },
        p,
        fr
        );
        er = Random.Range(0, xp.Count);
        p = xp[er];
        xp.RemoveAt(er);

        SpawnObj(m_gmAnswer, () =>
        {
            OnEnd();
        },
        p,
        sr
        );
    }


    public void SpawnObj(GameObject obj, System.Action callBack,float x,int number)
    {

        var o = Instantiate(obj, m_gmPannelOf_all_Obj.transform);
        o.GetComponent<Item_In_Game>().m_acOnPlayerEnter = () =>
        {
            if (!m_bEnd)
            {
                callBack?.Invoke();
            }
            Destroy(o);
        };

        o.GetComponent<Item_In_Game>().m_acOnDestroy = () =>
        {
            Destroy(o);
        };

        o.transform.localPosition = new Vector3(x, m_gmLeft.transform.localPosition.y);

        o.GetComponent<Text>().text = number.ToString();

        o.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -400), ForceMode2D.Impulse);

    }

    private void OnEnd()
    {
        m_bEnd = true;
        if (m_gmPannelOf_all_Obj.transform.childCount > 0)
            for (int c = 0; c < m_gmPannelOf_all_Obj.transform.childCount; c++)
            {
                var t = c;
                Destroy(m_gmPannelOf_all_Obj.transform.GetChild(t).gameObject);
            }
        var x = GetComponent<Main_Menu_Controller>().OnMenuBarClickC(m_gmAwnswered);
        x.GetComponent<Game_answer>().Init(m_inCoin,
        () =>
        {
            GetComponent<Main_Menu_Controller>().close();
            Player_Menager.m_inCoin += m_inCoin;
        },
        () =>
        {
            m_bEnd = false;
            StartPlay().Forget();
            Player_Menager.m_inCoin += m_inCoin;
        });
    }


    private bool m_bEnd;

    void Update()
    {
        if (m_bEnd) return;
        m_flWay = 0;
        if (Input.touchCount <= 0) return;
        var f = Input.GetTouch(0);
        var s = Screen.width / 2;

        if (f.position.x >= s)
            m_flWay = 1;
        else if (f.position.x < s)
            m_flWay = -1;
    }


    // Update is called once per frame
    public void FixedUpdate()
    {
        m_gmPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(m_flWay * m_flSpeed * 1000, 0);
    }
}
