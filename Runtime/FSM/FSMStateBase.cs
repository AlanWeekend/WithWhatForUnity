namespace ZCCUtils.FSM
{
    /// <summary>
    /// 状态的基础类：给子类提供方法
    /// </summary>
    /// 
    public class FSMStateBase
    {

        //给每个状态设置一个ID
        public int ID { get; set; }

        //被当前机器所控制
        public FSMStateMachine machine;

        public FSMStateBase(int id)
        {
            this.ID = id;
        }

        //给子类提供方法
        public virtual void OnEnter(params object[] args) { }
        public virtual void OnStay(params object[] args) { }
        public virtual void OnExit(params object[] args) { }

    }
}