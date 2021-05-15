using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Mirror
{
    public abstract class NetworkBehaviour : MonoBehaviour
    {
        /// 通过netIdentity实例的isLocalPlayer属性判断是不是当前角色
        public bool isLocalPlayer => true;

        //  public bool isLocalPlayer => netIdentity.isLocalPlayer;

        /// 缓存netIdentity
        
    }
}