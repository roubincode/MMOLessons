using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

[Serializable] public class UnityEventEntity : UnityEvent<Entity> { }
public abstract partial class Entity : MonoBehaviour
{
    public long UnitId;
    public string CNName;
    public string ClassName;

    [Header("Components")]
    public Animator animator;
    new public Collider collider;
    public AudioSource audioSource;

    GameObject _target;

    public Entity target
    {
        get { return _target != null ? _target.GetComponent<Entity>() : null; }
        set { _target = value != null ? value.gameObject : null; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void OnDeath(){
        Debug.Log("啊！我死了！");
        target = null;
    }
}
