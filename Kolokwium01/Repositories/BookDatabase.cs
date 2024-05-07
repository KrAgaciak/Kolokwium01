using System.Data;
using Kolokwium01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace Kolokwium01.Repositories;

public class BookDatabase: IBookDatabase
{
    private string sqlConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStore;Integrated Security=True;";

    public Book GetBookAuthorsFromDB(int ID)
    {
        Book book;
        List<Author> listOfAuthors = new List<Author>();
        
        using SqlConnection connection = new SqlConnection(sqlConnection);
        {
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                using SqlCommand command = new SqlCommand();
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM BOOKS book" +
                                          " JOIN books_authors ba ON ba.FK_book = book.PK" +
                                          " WHERE book.PK = @ID";
                    command.Parameters.AddWithValue("@ID", ID);
                    
                    var givenBook = command.ExecuteReader();
                    if (givenBook.Read())
                    {
                        book = new Book(givenBook);
                        givenBook.Close();
                    }
                    else
                    {
                        givenBook.Close();
                        connection.Close();
                        return null;
                    }
                
                }
                
                using SqlCommand command2 = new SqlCommand();
                {
                    command2.Connection = connection;
                    command2.CommandText = "SELECT * FROM Authors a" +
                                           " JOIN books_authors ba ON ba.FK_author = a.PK" +
                                           " WHERE ba.FK_book = @bookID";
                    command2.Parameters.AddWithValue("@bookID", ID);

                    var givenAuthor = command2.ExecuteReader();
                    while(givenAuthor.Read())
                    {
                        Author author = new Author(givenAuthor);
                        
                        book.authorsIDList.Add(author);
                    }
                    givenAuthor.Close();
                }
                
                
                
                connection.Close();
                return book;
            }
            else
            {
                connection.Close();
                throw new Exception("Connection is closed");
            }
        }
    }
    
    
    
    public NewBook InsertBookToDB(NewBook newBook)
    {
        using SqlConnection connection = new SqlConnection(sqlConnection);
        {
            int bookID;
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                SqlTransaction transaction = connection.BeginTransaction();
                
                using SqlCommand command = new SqlCommand();
                {
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO Books output INSERTED.ID VALUES (@Title)";
                    command.Parameters.AddWithValue("@Title", newBook.title);
                    

                    try
                    {
                        bookID = (int)command.ExecuteScalar();
                        if (bookID == null)
                        {
                            transaction.Rollback();
                            connection.Close();
                            throw new Exception("Bad request.");
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        connection.Close();
                        throw e;
                    }
                }

                if (newBook.listofAuthors != null)
                {
                    foreach (Author author in newBook.listofAuthors)
                    {
                        using SqlCommand command2 = new SqlCommand();
                        {
                            command2.Connection = connection;
                            command2.Transaction = transaction;
                            command2.CommandText =
                                "INSERT INTO Authors VALUES (@FirstName, @LastName)";
                            command2.Parameters.AddWithValue("@FirstName", author.first_name);
                            command2.Parameters.AddWithValue("@LastName", author.last_name);

                        }
                    }
                }

                transaction.Commit();
                connection.Close();
                return newBook;
            }
            else
            {
                connection.Close();
                throw new Exception("Connection closed.");
            }
        }
    }
    


}