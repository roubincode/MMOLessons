using System;
using UnityEngine;

[Serializable]
public struct Character
{
    public long CharaId{ get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Class { get; set; } 
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public int level { get; set; }
    public int health { get; set; }
    public int mana { get; set; }
    public long experience { get; set; } 
    public long skillExperience { get; set; }
    public int money { get; set; }
    public int mail { get; set; }
    public string title { get; set; }
    public long map { get; set; }
    public long region { get; set; }
    public bool gamemaster { get; set; }
    public string sex { get; set;}
    public bool online { get; set; }
    public DateTime lastsaved { get; set; }
    public int index {get;set;}
    public EquipInfo[] equipDatas {get;set;}
}

[Serializable]
public struct Account{
    public long accountId {get;set;}
    public string password {get;set;}
}

// 道具信息结构
[Serializable]
public struct ItemInfo
{
    public int id { get; set; }
    public string name { get; set; }
    public int amount { get; set; }
    public int durability { get; set; }
    public int index { get; set; }
}

// 装备信息结构
[Serializable]
public partial struct EquipInfo {
    public string CatType{get;set;}
    public int itemID{get;set;}
    public int endurance {get;set;}
    public int strength {get;set;}
    public int intellect {get;set;}
    public int agility {get;set;}
    public int amount { get; set; }
    public int spirit {get;set;}
    public int physical {get;set;}
}

// 装备格子信息结构
[Serializable]
public partial struct EquipSlotInfo
{
    public string requiredCategory;
    public string catName;
    public Transform location;
}

// 技能信息结构
[Serializable]
public struct SkillInfo {
    public string name{get;set;}
    public int skillId{get;set;}
    public float distance {get;set;}
    public int level {get;set;}
    public float castTime {get;set;}
    public float cooldown {get;set;}
}

[Serializable]
public struct Move
{
    public MoveState state;
    public Vector3 position; 
    public float yRotation;
    public float nSpeed;
    public float jumpLeg;
    public Move(MoveState state, Vector3 position, 
                float yRotation,float nSpeed,float jumpLeg)
    {
        this.state = state;
        this.position = position;
        this.yRotation = yRotation;
        this.nSpeed = nSpeed;
        this.jumpLeg = jumpLeg;
    }
}

[Serializable]
public struct UseSkill
{
    public int skillId;
    public int skillLevel;
    public bool send;
    public UseSkill(int skillId, int skillLevel)
    {
        this.skillId = skillId;
        this.skillLevel = skillLevel;
        this.send = false;
    }
}

public enum MoveState : byte { IDLE, RUNNING, AIRBORNE, SWIMMING, MOUNTED, MOUNTED_AIRBORNE, MOUNTED_SWIMMING, DEAD }