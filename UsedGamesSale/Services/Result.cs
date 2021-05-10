namespace UsedGamesSale.Services
{
    public class Result
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public Result() { }

        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, string errorMessage) : this (success)
        {
            ErrorMessage = errorMessage;
        }
    }
}
