namespace MyRoomServer.Entities
{
    /// <summary>
    /// 数据访问对象接口
    /// </summary>
    public interface IAccessData<T> where T : IAccessData<T>
    {
        /// <summary>
        /// 更新数据访问对象
        /// </summary>
        /// <param name="obj"></param>
        public void Update(T obj);

        /// <summary>
        /// 获取传输对象
        /// </summary>
        public object TransferData { get; }
    }
}
