using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookAPI.Model;
using bookAPI.Helper;
using bookAPI.DAL;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace bookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly bookAPIContext _context;
        private readonly IMapper _mapper;
        private IBookRepository bookRepository;

        public BooksController(bookAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.bookRepository = new BookRepository(new bookAPIContext());
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            return await _context.Book.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        //PUT with PATCH to handle isFavourite
        [HttpPatch("update/{id}")]
        public BookDTO Patch(int id, [FromBody]JsonPatchDocument<BookDTO> bookPatch)
        {
            //get original book object from the database
            Book originVideo = bookRepository.GetBookByID(id);
            //use automapper to map that to DTO object
            BookDTO bookDTO = _mapper.Map<BookDTO>(originVideo);
            //apply the patch to that DTO
            bookPatch.ApplyTo(bookDTO);
            //use automapper to map the DTO back ontop of the database object
            _mapper.Map(bookDTO, originVideo);
            //update book in the database
            _context.Update(originVideo);
            _context.SaveChanges();
            return bookDTO;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public class URLDTO {
            public String URL { get; set; }
        }

        
        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody]URLDTO data)
        {
            Book book;
            String bookURL;
            String bookId;
            try
            {
                bookURL = data.URL;
                bookId = GoogleBooksHelper.GetbookIdFromURL(bookURL);
                book = GoogleBooksHelper.GetBookInfo(bookId, bookURL);
            }
            catch
            {
                return BadRequest("Invalid Book URL");
            }

            //Add this book object to the database
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            // Get the primary key of the newly created book record in the database
            int id = book.BookId;
            //Construct seperate thread
            bookAPIContext tempcontext = new bookAPIContext();
            WordsController wordscontroller = new WordsController(tempcontext);

            //Executed in background
            Task addWords = Task.Run(async () =>
            {   
                //Get list of words from GoogleHelper
                List<Word> words = new List<Word>();

                words = GoogleBooksHelper.GetWords(bookId);


                for (int i = 0; i < words.Count; i++)
                {
                    Word word = words.ElementAt(i);
                    word.BookId = id;
                    await wordscontroller.PostWord(word);                
                }

            });
      
            //Return success code and the info on the video object
            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        //Search by Word

        // GET api/Bookss/SearchByWordss/
        [HttpGet("SearchByWords/{searchString}")]
        public async Task<ActionResult<IEnumerable<Book>>> Search(string searchString)
        {
             if (String.IsNullOrEmpty(searchString))
            {
                return BadRequest("Search string cannot be null or empty.");
            }

            // Choose descriptions that has the word 
            var books = await _context.Book.Include(book => book.Word).Select(book => new Book {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                BookAuthor = book.BookAuthor,
                WebUrl = book.WebUrl,
                BookPages = book.BookPages,
                ThumbnailUrl = book.ThumbnailUrl,
                IsRead = book.IsRead,
                Word = book.Word.Where(dec => dec.Word1.Contains(searchString)).ToList()
            }).ToListAsync();

            // Removes all videos with empty transcription
            books.RemoveAll(book => book.Word.Count == 0);
            return Ok(books);
        }


        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
