using Kolokwium01.Models;
using Kolokwium01.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;



namespace Kolokwium01.Controllers
{
    [ApiController]
    [Route("api/animals")]

    public class BookController: ControllerBase
    {
        
        private IBookDatabase db;

        public BookController(IBookDatabase db)
        {
            this.db = db;
        }

        [HttpGet("{ID}/Author")]
        public IActionResult GetBookAuthor(int ID)
        {
            Book book;
            try
            {
                book = db.GetBookAuthorsFromDB(ID);
                if (book is null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(book);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        
    }
}