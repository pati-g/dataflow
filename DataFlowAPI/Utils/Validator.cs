namespace DataFlowAPI.Utils
{
    public static class Validator
    {
        // Validate the filepath and the delimeter
        public static void ValidateParams(string filepath, char delimeter)
        {
            if (delimeter == default(char))
            {
                throw new ArgumentException($"Argument {nameof(delimeter)} has not been provided");
            }
            if (string.IsNullOrEmpty(filepath) || !Directory.Exists(filepath))
            {
                throw new ArgumentException($"No valid argument for {nameof(filepath)} has been provided");
            }
        }
    }
}
