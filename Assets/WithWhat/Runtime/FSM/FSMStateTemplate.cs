namespace WithWhat.FSM
{
    /// <summary>
    /// 状态拥有者
    /// </summary>
    public class FSMStateTemplete<T> : FSMStateBase
    {

        public T owner;   //拥有者(范型)

        public FSMStateTemplete(int id, T o) : base(id)
        {
            owner = o;
        }
    }
}