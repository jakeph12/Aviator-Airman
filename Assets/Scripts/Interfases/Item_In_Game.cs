using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_In_Game : MonoBehaviour
{
    public Action m_acOnPlayerEnter,m_acOnDestroy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            m_acOnPlayerEnter?.Invoke();
        }
        else if (collision.transform.tag == "Border")
        {
            m_acOnDestroy?.Invoke();
        }
    }
}
