using MongoDB.Driver;


namespace Contract
{
    public interface IMongoTeleMedicineDBContext
    {
        IMongoCollection<User> GetCollection<User>(string name);
    }
}
