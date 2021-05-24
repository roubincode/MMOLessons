using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public UIState uIState;
    public void ExitPanel(){
        gameObject.SetActive(false);
    }

    public void EnterPanel(){
        gameObject.SetActive(true);
    }
}
