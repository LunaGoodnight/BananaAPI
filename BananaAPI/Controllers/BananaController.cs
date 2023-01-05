using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BananaAPI.Controllers;


public class Employee
{
    public int EmployeeId { get; set; }
    public string LastName { get; set; }
}


[ApiController]
[Route("[controller]")]
public class BananaController : ControllerBase
{
    private readonly ILogger<BananaController> _logger;

    public BananaController(ILogger<BananaController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetEmployees")]
    public async Task<IEnumerable<Employee>> Get()
    {
        var sql = "select * from Employees";
        using IDbConnection conn = new SqlConnection(@"Data Source=.\NORTHWIND;Database=Northwind;Trusted_Connection=True;");
        conn.Open();

        try
        {
            var employees = await conn.QueryAsync<Employee>(sql);
            return employees.Select(x => new Employee { LastName = x.LastName, EmployeeId = x.EmployeeId });
        }
        catch (Exception e)
        {
            conn.Close();
            Console.WriteLine(e);
            throw;
        }
       
    }
}