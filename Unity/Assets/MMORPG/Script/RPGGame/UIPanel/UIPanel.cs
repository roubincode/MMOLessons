using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public UIState uIState;
    public virtual void ExitPanel(){
        gameObject.SetActive(false);
    }

    public virtual void EnterPanel(){
        gameObject.SetActive(true);
    }
}
