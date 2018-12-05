using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    [Tooltip("等幾秒後執行")]
    [SerializeField]
    public float WaitSecond;
    [Tooltip("幾秒後強制切下個動作(即使還沒移動到目的地)")]
    [SerializeField]
    public float MaxProcessingTime;
    [Tooltip("目的地座標是相對於哪個物件")]
    [SerializeField]
    public RelativeTo RelativeToTarget;
    [Tooltip("移動目的地")]
    [SerializeField]
    public Vector2 Destination;
    public bool ExpandFolder = true;
    [Tooltip("移動速度")]
    [SerializeField]
    public float MoveSpeed;
    [Tooltip("設定為True代表只會被隨機事件呼叫時執行")]
    [SerializeField]
    public bool ActiveOnlyByEvent;
    [Tooltip("動作標籤(隨機動作事件用，沒用到就不用填)")]
    [SerializeField]
    public string NodeTag;
    public bool ExpandRandomNodes = true;

    [Tooltip("多個技能施放的間隔")]
    [SerializeField]
    public float SpellInterval = 0.5f;


    [Tooltip("動作類型")]
    [SerializeField]
    public ActionType Type;
    [Tooltip("自身特效(跟隨腳色)")]
    [SerializeField]
    public ParticleSystem LocoParticle;
    [Tooltip("世界特效(不會跟隨腳色)")]
    [SerializeField]
    public ParticleSystem WorldPartilce;
    [Tooltip("填腳色Animation Trigger名稱(沒用到就不用填)")]
    [SerializeField]
    public string RoleAniTriggerName;
    [Tooltip("填攝影機Animation Trigger名稱(沒用到就不用填)")]
    [SerializeField]
    public string CamAniTriggerName;

    [Tooltip("註解")]
    [SerializeField]
    public string Description;
    [Tooltip("召喚間隔秒數")]
    [SerializeField]
    public float SpawnIntervalTime;
    [Tooltip("衝刺方向")]
    [SerializeField]
    public RushDirect MyRushDirect;
    [Tooltip("衝刺力道")]
    [SerializeField]
    public Vector2 RushForce;
    [Tooltip("衝刺力道")]
    [SerializeField]
    public float RushForce2;
    [Tooltip("是否開啟隨機動作事件，可以依造權重跳轉至隨機的動作標籤")]
    [SerializeField]
    public bool ToRandomNode;
    [Tooltip("執行隨機動作事件後，是否繼續執行當前動作順序")]
    public bool KeepNextNode;
    public enum RushDirect { Custom, Player };


    //以下需深複製
    public List<KeyWeight> GoToNodes = new List<KeyWeight>();
    public List<Skill> SkillList = new List<Skill>();
    [Tooltip("召喚怪物")]
    [SerializeField]
    public List<SpawnEnemyData> SpawnEnemyList;
    [Tooltip("音效清單")]
    [SerializeField]
    public List<AudioClip> SoundList;

    public Node GetMemberwiseClone()
    {
        Node data = this.MemberwiseClone() as Node;
        data.GoToNodes = new List<KeyWeight>(this.GoToNodes);
        data.SkillList = new List<Skill>(this.SkillList);
        SpawnEnemyList = new List<SpawnEnemyData>(this.SpawnEnemyList);
        SoundList = new List<AudioClip>(this.SoundList);
        return data;
    }
    public string GetNodeKeyFromWeight()
    {
        if (GoToNodes == null)
            return "";
        if (GoToNodes.Count == 0)
            return "";
        List<int> weightList = new List<int>();
        for (int i = 0; i < GoToNodes.Count; i++)
        {
            weightList.Add(GoToNodes[i].Weight);
        }
        string key = GoToNodes[ProbabilityGetter.GetFromWeigth(weightList)].Key;
        return key;
    }
}
