namespace DataFlowAPI.Controllers
{
    public interface IDataflowService
    {
        Task<string> ProcessDataAsync(string filepath, string delimeter);
    }
}