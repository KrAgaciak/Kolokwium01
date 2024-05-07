using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;

namespace Kolokwium01.Models;

public class Author
{
    [JsonIgnore]
    public int PK {get;set;}
    public string first_name {get;set;}
    public string last_name {get;set;}
    
    public Author(SqlDataReader reader)
    {
        this.PK = reader.GetInt32(reader.GetOrdinal("PK"));
        this.first_name = reader.GetString(reader.GetOrdinal("first_name"));
        this.last_name = reader.GetString(reader.GetOrdinal("last_name"));
    }

}