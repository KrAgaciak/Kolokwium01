using Microsoft.Data.SqlClient;

namespace Kolokwium01.Models;

public class Book
{
    public int PK {get;set;}
    public string title {get;set;}
    public List<Author> authorsIDList;

    public Book(int PK_,string title_)
    {
        this.PK = PK_;
        this.title = title_;
    }
    
    
    public Book(SqlDataReader reader)
    {
        this.PK = reader.GetInt32(reader.GetOrdinal("PK"));
        this.title = reader.GetString(reader.GetOrdinal("title"));
    }
    
}