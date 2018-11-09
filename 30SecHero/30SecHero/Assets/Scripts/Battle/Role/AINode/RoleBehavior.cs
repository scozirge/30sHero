using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyRole))]
public class RoleBehavior : MonoBehaviour
{
    public List<Node> Nodes;
    public AnimationPlayer MyAni;

    Node CurNode;
    Rigidbody2D MyRigid;
    AIRoleMove MyAIRoleMove;
    EnemyRole MyRole;
    int CurNodeIndex;
    bool StartMove;
    Coroutine ForceStopCoroutine;
    Vector2 Destination = Vector2.zero;
    Vector2 SapwnPos;
    //招怪
    Transform EnemyParent;
    int CurSpawnCount;
    [Tooltip("此腳色最多可以同時在場上有幾隻召喚怪")]
    public int MaxSpawnCount;
    [Tooltip("動作幾輪")]
    public int ActionRoundCount;
    int CurRoundCount;
    List<EnemyRole> SpawnList = new List<EnemyRole>();
    int CurSpawnIndex;

    void Start()
    {
        EnemyParent = GameObject.FindGameObjectWithTag("EnemyParent").transform;
        SapwnPos = transform.position;
        MyRigid = GetComponent<Rigidbody2D>();
        MyAIRoleMove = GetComponent<AIRoleMove>();
        MyRole = GetComponent<EnemyRole>();
        if (InitBehavior())
        {
            if (Nodes[CurNodeIndex].ActiveOnlyByEvent)
                DoNextAction();
            else
                StartCoroutine(WaitToAction(Nodes[CurNodeIndex]));
        }
    }
    bool InitBehavior()
    {
        bool available = false;
        if (Nodes == null || Nodes.Count == 0)
            return available;
        for (int i = 0; i < Nodes.Count; i++)
        {
            if (!Nodes[i].ActiveOnlyByEvent)
            {
                available = true;
                break;
            }
        }
        return available;
    }
    void DoAction(Node _node)
    {

        StartMove = false;
        switch (_node.Type)
        {
            case ActionType.Move:
                MyRigid.velocity = Vector2.zero;
                Destination = GetRelativeDestination(_node);
                StartMove = true;
                if (_node.MaxProcessingTime > 0)
                    ForceStopCoroutine = StartCoroutine(ForceDoNextAction(_node.MaxProcessingTime));
                break;
            case ActionType.Spawn:
                StartSpawn(_node);
                break;
            case ActionType.Rush:
                MyRole.Rush(_node.RushForce);
                CheckRandomNode();
                break;
            case ActionType.Spell:
                Spell(_node);
                CheckRandomNode();
                break;
            case ActionType.Teleport:
                Destination = GetRelativeDestination(_node);
                transform.position = Destination;
                if (Nodes[CurNodeIndex].LocoParticle != null)
                    EffectEmitter.EmitParticle(Nodes[CurNodeIndex].LocoParticle, Vector2.zero, Vector2.zero, transform);
                if (Nodes[CurNodeIndex].WorldPartilce != null)
                    EffectEmitter.EmitParticle(Nodes[CurNodeIndex].WorldPartilce, transform.position, Vector2.zero, null);
                CheckRandomNode();
                break;
            case ActionType.Perform:
                if (Nodes[CurNodeIndex].LocoParticle != null)
                    EffectEmitter.EmitParticle(Nodes[CurNodeIndex].LocoParticle, Vector2.zero, Vector2.zero, transform);
                if (Nodes[CurNodeIndex].WorldPartilce != null)
                    EffectEmitter.EmitParticle(Nodes[CurNodeIndex].WorldPartilce, transform.position, Vector2.zero, null);
                if (Nodes[CurNodeIndex].CamAniTriggerName != "")
                    CameraController.PlayMotion(Nodes[CurNodeIndex].CamAniTriggerName);
                if (MyAni && Nodes[CurNodeIndex].RoleAniTriggerName != "")
                    MyAni.PlayTrigger(Nodes[CurNodeIndex].RoleAniTriggerName, 0);
                if (Nodes[CurNodeIndex].SoundList != null)
                {
                    for (int i = 0; i < Nodes[CurNodeIndex].SoundList.Count; i++)
                    {
                        if (Nodes[CurNodeIndex].SoundList[i] == null)
                            continue;
                        AudioPlayer.PlaySound(Nodes[CurNodeIndex].SoundList[i]);
                    }
                }
                CheckRandomNode();
                break;
            default:
                CheckRandomNode();
                break;
        }
    }
    Vector2 GetRelativeDestination(Node _node)
    {
        Vector2 pos = Vector2.zero;
        switch (_node.RelativeToTarget)
        {
            case RelativeTo.Self:
                pos = (Vector2)transform.position + _node.Destination;
                break;
            case RelativeTo.SpawnPosition:
                pos = SapwnPos + _node.Destination;
                break;
            case RelativeTo.PlayerRole:
                if (BattleManage.BM.MyPlayer)
                    pos = (Vector2)BattleManage.BM.MyPlayer.transform.position + _node.Destination;
                break;
            case RelativeTo.PlayerRole_Continued:
                if (BattleManage.BM.MyPlayer)
                    pos = (Vector2)BattleManage.BM.MyPlayer.transform.position + _node.Destination;
                break;
            case RelativeTo.Camera_Continued:
                pos = (Vector2)BattleManage.MyCameraControler.transform.position + _node.Destination;
                break;
            case RelativeTo.Camera:
                pos = (Vector2)BattleManage.MyCameraControler.transform.position + _node.Destination;
                break;
        }
        return pos;
    }
    void Spell(Node _node)
    {
        _node.Spell();
    }
    void CheckRandomNode()
    {
        if (Nodes[CurNodeIndex].ToRandomNode)
        {
            string nodeTag = Nodes[CurNodeIndex].GetNodeKeyFromWeight();
            if (nodeTag != "")
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[i].NodeTag == nodeTag)
                    {
                        CurNodeIndex = i;
                        StartCoroutine(WaitToAction(Nodes[CurNodeIndex]));
                        return;
                    }
                }
            }
        }
        DoNextAction();
    }
    void DoNextAction()
    {
        CurNodeIndex++;
        if (CurNodeIndex > Nodes.Count - 1)
        {
            CurNodeIndex = 0;
            if (ActionRoundCount>0)
            {
                CurRoundCount++;
                if (CurRoundCount >= ActionRoundCount)
                    return;
            }
        }

        if (Nodes[CurNodeIndex].ActiveOnlyByEvent)
            DoNextAction();
        else
            StartCoroutine(WaitToAction(Nodes[CurNodeIndex]));
    }

    void FixedUpdate()
    {
        if (Nodes[CurNodeIndex].RelativeToTarget == RelativeTo.PlayerRole_Continued ||
            Nodes[CurNodeIndex].RelativeToTarget == RelativeTo.Camera_Continued)
        {
            MoveTo(GetRelativeDestination(Nodes[CurNodeIndex]), Nodes[CurNodeIndex].MoveSpeed);
        }
        else
            MoveTo(Destination, Nodes[CurNodeIndex].MoveSpeed);
    }

    void MoveTo(Vector2 _pos, float _moveSpeed)
    {
        if (!StartMove)
            return;
        Vector2 targetVel = (_pos - (Vector2)transform.position).normalized * _moveSpeed;
        MyRigid.velocity = Vector2.Lerp(MyRigid.velocity, targetVel, 0.1f);
        if (Vector2.Distance(transform.position, _pos) < 10)
        {
            if (MyAIRoleMove)
                MyAIRoleMove.SetHereToDestination();
            transform.position = _pos;
            MyRigid.velocity = Vector2.zero;
            StartMove = false;
            if (ForceStopCoroutine != null)
                StopCoroutine(ForceStopCoroutine);
            CheckRandomNode();
        }
    }
    IEnumerator WaitToAction(Node _node)
    {
        yield return new WaitForSeconds(_node.WaitSecond);
        DoAction(_node);
    }
    IEnumerator ForceDoNextAction(float _time)
    {
        yield return new WaitForSeconds(_time);
        if (MyAIRoleMove)
            MyAIRoleMove.SetHereToDestination();
        CheckRandomNode();
    }

    void StartSpawn(Node _node)
    {
        CurSpawnIndex = 0;
        SpanwEnemy(_node);
    }
    void SpanwEnemy(Node _node)
    {
        if (ReachMaxSpawnCount())
        {
            CheckRandomNode();
            return;
        }
        CurSpawnCount++;
        EnemyRole er = Instantiate(_node.SpawnEnemyList[CurSpawnIndex].Enemy, Vector3.zero, Quaternion.identity) as EnemyRole;
        er.transform.SetParent(EnemyParent);
        AIRoleMove ai = er.GetComponent<AIRoleMove>();
        Vector2 offset = Vector2.zero;
        switch (_node.SpawnEnemyList[CurSpawnIndex].SpawnPosRelateTo)
        {
            case PosRelateTo.Self:
                er.transform.position = (Vector2)transform.position + _node.SpawnEnemyList[CurSpawnIndex].SpawnPosition;
                break;
            case PosRelateTo.PlayerRole:
                er.transform.position = (Vector2)BattleManage.BM.MyPlayer.transform.position + _node.SpawnEnemyList[CurSpawnIndex].SpawnPosition;
                break;
            case PosRelateTo.Camera:
                er.transform.position = (Vector2)BattleManage.MyCameraControler.transform.position + _node.SpawnEnemyList[CurSpawnIndex].SpawnPosition;
                break;
        }
        if (_node.SpawnEnemyList[CurSpawnIndex].LifeTime > 0)
            er.SetLifeTime(_node.SpawnEnemyList[CurSpawnIndex].LifeTime);
        BattleManage.AddEnemy(er);
        SpawnList.Add(er);
        CurSpawnIndex++;
        if (CurSpawnIndex < _node.SpawnEnemyList.Count)
            StartCoroutine(WaitToSpawnEnemy(_node));
        else
            CheckRandomNode();
    }
    bool ReachMaxSpawnCount()
    {
        SpawnList.RemoveAll(item => item == null || !item.isActiveAndEnabled);
        if (SpawnList.Count >= MaxSpawnCount)
            return true;
        else
            return false;
    }
    IEnumerator WaitToSpawnEnemy(Node _node)
    {
        yield return new WaitForSeconds(_node.SpawnIntervalTime);
        SpanwEnemy(_node);
    }
}
