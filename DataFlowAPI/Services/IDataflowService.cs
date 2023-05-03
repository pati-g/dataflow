namespace DataFlowAPI.Services
{
    public interface IDataflowService
    {
        Task<string> ProcessDataAsync(string filepath, char delimeter);
    }
}