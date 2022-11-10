namespace Contract
{
    public interface ISmsService
    {
        Task<bool> SendTextMessageAsync(string text, string phoneNumber);
    }
}
