using Kolokwium01.Models;

namespace Kolokwium01.Repositories;

public interface IBookDatabase
{
    public Book GetBookAuthorsFromDB(int ID);
    public NewBook InsertBookToDB(NewBook newBook);
}