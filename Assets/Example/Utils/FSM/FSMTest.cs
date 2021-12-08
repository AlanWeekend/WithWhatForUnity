using UnityEngine;
using WithWhat.FSM;

public class FSMTest : MonoBehaviour
{
    /// <summary>
    /// 状态枚举
    /// </summary>
    enum State
    { 
        /// <summary>
        /// 站立状态
        /// </summary>
        idle,
        /// <summary>
        /// 攻击状态
        /// </summary>
        attack
    }

    /// <summary>
    /// 站立状态
    /// </summary>
    class IdleState : FSMStateTemplete<FSMTest>
    {
        public IdleState(int id, FSMTest o) : base(id, o)
        {
        }

        public override void OnEnter(params object[] args)
        {
            Debug.Log("站立");
        }
    }

    /// <summary>
    /// 攻击状态
    /// </summary>
    class AttackState : FSMStateTemplete<FSMTest>
    {
        public AttackState(int id, FSMTest o) : base(id, o)
        {
        }

        public override void OnEnter(params object[] args)
        {
            Debug.Log("攻击");
            // 攻击后切换到站立状态
            owner.playerStateMachine.TranslateState((int)State.idle);
        }
    }

    private FSMStateMachine playerStateMachine;
    private IdleState _idleState;
    private AttackState _attackState;

    private void Awake()
    {
        _idleState = new IdleState((int)State.idle, this);
        _attackState = new AttackState((int)State.attack, this);
        playerStateMachine = new FSMStateMachine(_idleState);
        playerStateMachine.AddState(_attackState);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStateMachine.TranslateState((int)State.attack);
        }
    }
}