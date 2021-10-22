
namespace Common.Domain.Interfaces
{
    public interface IContextMongoDb
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}