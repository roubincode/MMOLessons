using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Entity
{
    public string nickName;
    public Sprite classIcon;
    public Sprite portraitIcon;
    public static Player localPlayer;
    GameObject _nextTarget;
    public Entity nextTarget
    {
        get { return _nextTarget != null ? _nextTarget.GetComponent<Entity>() : null;}
        set { _nextTarget = value != null ? value.gameObject : null;}
    }
    // Start is called before the first frame update
    void Start()
    {
        localPlayer = this;

        Animator ani = GetComponent<Animator>();
        foreach (var p in ani.parameters)
        {
            Debug.Log(p.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate()
    {

    }
    public override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("光荣战死！");
    }
}
