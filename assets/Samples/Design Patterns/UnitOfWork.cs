/*
    The Unit of Work pattern is a design pattern used to manage database transactions in a way that ensures consistency and minimizes the number of database calls.
    It acts as a bridge between the application and the data persistence layer, allowing you to group multiple operations into a single transactional unit.
    
    Key Concept
    The Unit of Work is responsible for:
    
    Keeping track of all changes to the database entities during a transaction.
    Committing or rolling back changes as a single unit.
    Coordinating repositories so that they share the same database context (in most cases, an ORM context like Entity Framework).
*/

/*

    IDisposable anv�nds h�r f�r att hantera resurser effektivt, s�rskilt n�r det g�ller databasanslutningar. 
    N�r UnitOfWork �r f�rdigt med sitt arbete (t.ex. efter en transaktion) kan Dispose-metoden anropas f�r att frig�ra databaskopplingen och f�rhindra resursl�ckor. 
    Detta �r viktigt i applikationer som anv�nder datak�llor intensivt. 

 */

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    Task CommitAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public IProductRepository Products { get; }

    public UnitOfWork(DbContext context, IProductRepository products)
    {
        _context = context;
        Products = products;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        /*  
            Dispose-metoden st�dar upp och frig�r resurser som anv�nds av DbContext.
            Detta inkluderar att st�nga eventuella �ppna databasanslutningar och frig�ra minne som inte l�ngre beh�vs.
            Detta �r s�rskilt viktigt i applikationer som anv�nder m�nga resurser eller hanterar flera transaktioner samtidigt. 
        */
        _context.Dispose();
    }
}

using (var unitOfWork = new UnitOfWork(context, productRepository))
{
    var product = unitOfWork.Products.GetById(order.ProductId);
    product.Stock -= order.Quantity;

    unitOfWork.Orders.Add(order);

    await unitOfWork.CommitAsync();
}