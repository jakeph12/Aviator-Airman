using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_answer : MonoBehaviour
{

    [SerializeField]
    private Text m_txCoin;
    [SerializeField]
    private Button m_btHome,m_btRestart;


    public void Init(int coin,Action OnHome,Action OnRestart)
    {
        m_txCoin.text = coin.ToString();
        m_btHome.onClick.AddListener(() => 
        {
            OnHome?.Invoke();
            GetComponent<Main_Menu_Controller>().close();

        });
        m_btRestart.onClick.AddListener(() => 
        {
            OnRestart?.Invoke();
            GetComponent<Main_Menu_Controller>().close();
        });
    }

}
