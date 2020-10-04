using LiteDB;

namespace SIPVS.litedb
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}