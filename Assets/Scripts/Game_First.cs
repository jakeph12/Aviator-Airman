using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Game_First : MonoBehaviour
{
    private float m_flWay;
    [SerializeField]
    private float m_flSpeed;
    [SerializeField]
    private GameObject m_gmPlayer,m_gmLeft,m_gmRight;
    [SerializeField]
    private Text m_txCoin;
    [SerializeField]
    private List<GameObject> m_gmObject = new List<GameObject>();
    [SerializeField]
    private GameObject m_gmCoin,m_gmEnemy,m_gmFuel;
    [SerializeField]
    private GameObject m_gmAnswer;
    [SerializeField]
    private Slider m_slFule;


    private int _m_inCoin;
    private int m_inCoin
    {
        get => _m_inCoin;
        set
        {
            _m_inCoin = value;
            if(m_txCoin) m_txCoin.text = _m_inCoin.ToString();
        }
    }

    [SerializeField]
    private GameObject m_gmPannelOf_all_Obj;

    private int m_inAmountEnemy = 2,m_inAmountFuel = 1,m_inAmountCoin = 2;
    private int _m_inCurAmountEnemy,_m_inCurAmountFuel, _m_inCurAmountCoin;

    private int m_inCurAmountEnemy
    {
        get => _m_inCurAmountEnemy;
        set
        {
            _m_inCurAmountEnemy = value;
        }
    }
    private int m_inCurAmountFuel
    {
        get => _m_inCurAmountFuel;
        set
        {
            if (m_bEnd) return;
            _m_inCurAmountFuel = value;
            if (_m_inCurAmountFuel <= 0) OnEnd();
            if (_m_inCurAmountFuel > 100) _m_inCurAmountFuel = 100;
            m_slFule.value = _m_inCurAmountFuel;


        }
    }
    private int m_inCurAmountCoin
    {
        get => _m_inCurAmountCoin;
        set
        {
            if (m_bEnd) return;
            _m_inCurAmountCoin = value;
            m_txCoin.text = _m_inCurAmountCoin.ToString();
        }
    }

    public void Start()
    {
        StartGame().Forget();
    }

    private async UniTask StartGame()
    {
        m_bEnd = false;
        m_inCoin = 0;

        if (m_bPlayingFull)
        {
            Debug.Log("dd");
            _cts.Cancel();
            _cts.Dispose(); 
        }

        _cts = new CancellationTokenSource();
        m_inCurAmountFuel = 30;
        MinFuel(_cts.Token).Forget();

        m_gmPlayer.transform.localPosition = Vector2.zero;
        if(m_gmPannelOf_all_Obj.transform.childCount > 0)
            for(int c = 0; c < m_gmPannelOf_all_Obj.transform.childCount; c++)
            {
                    var t = c;
                    Destroy(m_gmPannelOf_all_Obj.transform.GetChild(t).gameObject);
            }
        m_gmPlayer.GetComponent<Image>().sprite = Player_Menager.m_airCurPlane.m_spPlaneIco;

        await UniTask.Delay(1200);
        for (int e = 0; e < m_inAmountEnemy; e++)
        {
            SpawnObj(m_gmEnemy, () => m_inCurAmountFuel -= 10);
            await UniTask.Delay(100);

        }

        for (int f = 0; f < m_inAmountFuel; f++)
        {
            SpawnObj(m_gmFuel, () => m_inCurAmountFuel += 20);
            await UniTask.Delay(100);

        }

        for (int i = 0; i < m_inAmountCoin; i++)
        {
            SpawnObj(m_gmCoin, () => m_inCoin += 10);
            await UniTask.Delay(100);

        }
    }

    public void SpawnObj(GameObject obj,System.Action callBack)
    {

        var o = Instantiate(obj, m_gmPannelOf_all_Obj.transform);
        o.GetComponent<Item_In_Game>().m_acOnPlayerEnter = () =>
        {
            if (!m_bEnd)
            {
                callBack?.Invoke();
                SpawnObj(obj, callBack);
            }
            Destroy(o);
        };
        var x = Random.Range(m_gmLeft.transform.localPosition.x, m_gmRight.transform.localPosition.x);


        o.GetComponent<Item_In_Game>().m_acOnDestroy = () =>
        {
            if(!m_bEnd)
                SpawnObj(obj, callBack);
            Destroy(o);
        };

        o.transform.localPosition = new Vector3(x, m_gmLeft.transform.localPosition.y);



        o.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1000),ForceMode2D.Impulse);

    }
    private bool m_bEnd;
    private void OnEnd()
    {
        m_bEnd = true;
        if (m_gmPannelOf_all_Obj.transform.childCount > 0)
            for (int c = 0; c < m_gmPannelOf_all_Obj.transform.childCount; c++)
            {
                var t = c;
                Destroy(m_gmPannelOf_all_Obj.transform.GetChild(t).gameObject);
            }
        var x = GetComponent<Main_Menu_Controller>().OnMenuBarClickC(m_gmAnswer);
        x.GetComponent<Game_answer>().Init(m_inCoin, 
        () => 
        {
            GetComponent<Main_Menu_Controller>().close();
            Player_Menager.m_inCoin += m_inCoin;
        },
        () =>
        {
            m_bEnd = false;
            StartGame().Forget();
            Player_Menager.m_inCoin += m_inCoin;
        });
    }
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


    private CancellationTokenSource _cts;

    private bool m_bPlayingFull = false;

    async UniTask MinFuel(CancellationToken tk)
    {
        m_bPlayingFull = true;
        while (m_inCurAmountFuel > 0)
        {
            await UniTask.Delay(1000,cancellationToken: tk);
            if (tk.IsCancellationRequested) // Проверка отмены
            {
                m_bPlayingFull = false;
                break; // Выход из цикла
            }
            m_inCurAmountFuel -= 5;
        }
        m_bPlayingFull = false;


    }

    public void FixedUpdate()
    {
        m_gmPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(m_flWay * m_flSpeed * 1000, 0);
    }
}
