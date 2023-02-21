namespace SystemServiceAPI.Bo.Interface
{
    public interface IAdminConfig
    {
        object GetAllTable();
        object GetColumns(string tableName);
        object ExcuteQuery(string cmd);
    }
}