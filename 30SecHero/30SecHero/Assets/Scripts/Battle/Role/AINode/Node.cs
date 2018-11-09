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
    [Tooltip("是否開啟隨機動作事件，可以依造權重跳轉至隨機的動作標籤")]
    [SerializeField]
    public bool ToRandomNode;
    public List<KeyWeight> GoToNodes = new List<KeyWeight>();

    public List<Skill> SkillList = new List<Skill>();

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
    [Tooltip("音效清單")]
    [SerializeField]
    public List<AudioClip> SoundList;
    [Tooltip("註解")]
    [SerializeField]
    public string Description;
    [Tooltip("召喚怪物")]
    [SerializeField]
    public List<SpawnEnemyData> SpawnEnemyList;
    [Tooltip("召喚間隔秒數")]
    [SerializeField]
    public float SpawnIntervalTime;
    [Tooltip("衝刺力道")]
    [SerializeField]
    public Vector2 RushForce;

    public Node GetMemberwiseClone()
    {
        Node data = this.MemberwiseClone() as Node;
        return data;
    }
    public void Spell()
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            SkillList[i].LaunchAIAttack();
        }
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
