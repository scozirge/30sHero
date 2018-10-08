using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class EnemyRole : Role
{
    [SerializeField]
    GameObject HealthObj;
    [SerializeField]
    float PlayMotionDuration = 0.5f;


    public int ID { get; protected set; }
    public string Name { get; protected set; }
    public int DebutFloor { get; protected set; }
    public EnemyType Type { get; protected set; }
    protected const float FrictionDuringTime = 1;
    protected float FrictionDuringTimer = FrictionDuringTime;
    protected bool StartVelocityDecay;

    AIRoleMove MyAIMove;
    PlayerRole Target;
    Sprite[] MotionSprite = new Sprite[2];
    Image RoleImg;
    MyTimer MotionTimer;


    public void SetEnemyData(EnemyData _data)
    {
        ID = _data.ID;
        Name = _data.Name;
        DebutFloor = _data.DebutFloor;
        Type = _data.Type;
    }
    protected override void Start()
    {
        base.Start();
        MyAIMove = GetComponent<AIRoleMove>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
            Target = go.GetComponent<PlayerRole>();
        if (Target && Health <= Target.Damage)
            HealthObj.SetActive(false);
        InitMotionPic();
    }
    void InitMotionPic()
    {
        RoleImg = transform.Find("Role/body").GetComponent<Image>();
        if (RoleImg != null)
        {
            string folderName = RoleImg.sprite.name.TrimEnd("_r".ToCharArray());
            string rPicName = folderName + "_r";
            string aPicName = folderName + "_a";
            MotionSprite[0] = Resources.Load<Sprite>(string.Format("Images/Role/{0}/{1}", folderName, rPicName));
            MotionSprite[1] = Resources.Load<Sprite>(string.Format("Images/Role/{0}/{1}", folderName, aPicName));
            MotionTimer = new MyTimer(PlayMotionDuration, ToReadyMotion, false, false);
            ToReadyMotion();
        }
    }
    void ToReadyMotion()
    {
        if (RoleImg == null)
            return;
        if (MotionSprite[0] == null)
            return;
        RoleImg.sprite = MotionSprite[0];
    }
    void ToAttackMotion()
    {
        MotionTimer.StartRunTimer = true;
        if (RoleImg == null)
            return;
        if (MotionSprite[1] == null)
            return;
        RoleImg.sprite = MotionSprite[1];
    }
    void SetEnemyDirection()
    {
        if (!Target)
            return;
        Vector2 dir = Target.transform.position - transform.position;

        if (dir.x >= 0)
            DirectX = Direction.Right;
        else
            DirectX = Direction.Left;

        if (dir.y >= 0)
            DirectY = Direction.Top;
        else
            DirectY = Direction.Bottom;
    }
    protected override void Move()
    {
        FaceTarget();
    }
    void FaceTarget()
    {
        if (!Target)
            return;
        if (transform.position.x > Target.transform.position.x)
        {
            RoleTrans.localScale = Vector2.one;
        }
        else
        {
            RoleTrans.localScale = new Vector2(-1, 1);
        }
    }
    public override void Attack()
    {
        base.Attack();
        AniPlayer.PlayTrigger("Attack", 0);
        ToAttackMotion();
    }
    public override void PreAttack()
    {
        base.PreAttack();
        AniPlayer.PlayTrigger_NoPlayback("PreAttack", 0);
    }
    public override void BeAttack(int _dmg, Vector2 _force)
    {
        AniPlayer.PlayTrigger("BeAttack", 0);
        base.BeAttack(_dmg, _force);
    }
    protected override void SelfDestroy()
    {
        BattleManage.RemoveEnemy(this);
        base.SelfDestroy();
        Drop();
    }

    protected override void Update()
    {
        base.Update();
        SetEnemyDirection();
        MotionTimer.RunTimer();
    }
    public override void AddBuffer(BufferData _buffer)
    {
        base.AddBuffer(_buffer);
        if (_buffer.Type == RoleBuffer.Stun)
            if (MyAIMove)
                MyAIMove.SetCanMove(false);
    }
    public override void RemoveBuffer(BufferData _buffer)
    {
        base.RemoveBuffer(_buffer);
        if (_buffer.Type == RoleBuffer.Stun)
            if (MyAIMove)
                MyAIMove.SetCanMove(false);
    }
    public EnemyRole GetMemberwiseClone()
    {
        EnemyRole role = this.MemberwiseClone() as EnemyRole;
        return role;
    }
}
