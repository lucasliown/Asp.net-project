namespace MvcBankAdmin.Models.Repository;

//Data repository which is used for the respoitory design for each model.
public interface IDataRepository<TEntity, TKey> where TEntity : class
{
    IEnumerable<TEntity> GetAll();
    TEntity Get(TKey id);
    TKey Add(TEntity item);
    TKey Update(TKey id, TEntity item);
    TKey Delete(TKey id);
}
