using Microsoft.Data.Analysis;

namespace DataFlowAPI.Services
{
    public class CsvReader : ICsvReader
    {
        public async Task<List<DataFrame>> ParseCsvFilesFromDirectory(string filepath, char delimeter)
        {
            var dfList = new List<DataFrame>();
            foreach (var file in Directory.EnumerateFiles(filepath, "*.csv"))
            {
                await LoadCsvFileToList(delimeter, dfList, file);
            }
            return dfList;
        }

        private static Task LoadCsvFileToList(char delimeter, List<DataFrame> dfList, string file)
        {
            return Task.Run(() =>
            {
                var dataFrame = DataFrame.LoadCsv(file, delimeter);
                dfList.Add(dataFrame);
            });
        }
    }
}
