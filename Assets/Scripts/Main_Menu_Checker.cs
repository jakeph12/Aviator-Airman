using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu_Checker : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gmTutor;
    void Start()
    {
        var P = PlayerPrefs.GetInt("TutorP", 0);
        if (P == 0)
        {

            GetComponent<Main_Menu_Controller>().OnMenuBarClick(m_gmTutor);
            PlayerPrefs.SetInt("TutorP", 1);

        }
    }
}
