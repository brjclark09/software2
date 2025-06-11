namespace Clark_ChinookEnhancedQueries.Services;

public class EnhancedQueryService : IEnhancedQuery
{
    private readonly ApplicationDbContext _context;

    public EnhancedQueryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, List<Customer>>> CustomersGroupedBySupportRepAsync()
    {
        var result = await _context.Customer
            .GroupBy(customer => customer.SupportRepId)
            .ToDictionaryAsync(
                group => group.Key.HasValue 
                    ? _context.Employee.Where(eemployee => employee.EmployeeId == group.Key.Value).Select(employee => employee.FirstName + " " + employee.LastName).FirstOrDefault() ?? "No Rep"
                    : "No Rep",
                g => group.ToList()
            );
        await _context.SaveChangesAsync();
        return result;
    }
}