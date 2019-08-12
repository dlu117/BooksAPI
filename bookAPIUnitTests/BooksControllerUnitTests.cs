using bookAPI.Controllers;
using bookAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AutoMapper;

namespace UnitTestbookAPI
{
    [TestClass]
    public class BooksControllerUnitTests
    {
        IMapper mapper;
        // BooksController.URLDTO data; //null data with no string url to test
        BooksController.URLDTO data;
       
     
        // creates a configuration object / bookAPIContext object access memory database
        public static readonly DbContextOptions<bookAPIContext> options
        = new DbContextOptionsBuilder<bookAPIContext>()
        .UseInMemoryDatabase(databaseName: "testDatabase")
        .Options;


        // adding dummy data to test database 

        public static IList<Book> books = new List<Book>{
         new Book()
         {
         BookTitle = "The Ivory Tower and Harry Potter",  // Book ID 1
         BookAuthor = "Lana A. Whited",
         BookPages = 418,
         WebUrl = "https://books.google.co.nz/books?id=iO5pApw2JycC&printsec=frontcover&dq=harry+potter&hl=en&sa=X&ved=0ahUKEwiA6OeCwfrjAhWu6XMBHS-6DysQ6AEINDAC#v=onepage&q=harry%20potter&f=false",
         ThumbnailUrl = "http://books.google.com/books/content?id=iO5pApw2JycC&printsec=frontcover&img=1&zoom=3&edge=curl&imgtk=AFLRE730SgWtunm0XBnRpekSTqoTwJo4IlsTU-AvOl_gZUNOPBH7iS_jvzlihHnfa9dN619Eyj98FOIw8Hp2B2NpFYZlTi9-KaGrXsRGaI_2exS7djYFQKr9EgsxB2TocGGFEhtJP6Ci&source=gbs_api"
         },
         new Book()                                      // Book ID 2
         {
         BookTitle = "Harry Potter and the Philosopher's Stone",
         BookAuthor = "J.K. Rowling",
         BookPages = 353,
         WebUrl = "https://books.google.co.nz/books?id=39iYWTb6n6cC&printsec=frontcover&dq=harry+potter&hl=en&sa=X&ved=0ahUKEwiA6OeCwfrjAhWu6XMBHS-6DysQ6AEILjAB#v=onepage&q=harry%20potter&f=false",
         ThumbnailUrl = "http://books.google.com/books/content?id=39iYWTb6n6cC&printsec=frontcover&img=1&zoom=3&edge=curl&imgtk=AFLRE70-FUbvwDFUKegCF6SWsfDgTDF5Hyw-4ZeBV80hDjx8vDbTtwWAd04J6LLFdUZ7lVYJLhYDBHjVdw8Vuw3wpmwoBKllQtQIDAHCeQlCulVPYdgoRNpOqW97nuC1Y8vZdkYWJikn&source=gbs_api"
         }
         };

        public string URL { get; private set; }

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new bookAPIContext(options))
            {
                // populate the mock db
                context.Book.Add(books[0]);
                context.Book.Add(books[1]);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new bookAPIContext(options))
            {
                // clear the db
                context.Book.RemoveRange(context.Book);
                context.SaveChanges();
            };
        }

 
        [TestMethod]
        public async Task TestGetBook()
        {
            using (var context = new bookAPIContext(options))
            {

                BooksController booksController = new BooksController(context, mapper);
                ActionResult<IEnumerable<Book>> result = await booksController.GetBook();
                Assert.IsNotNull(result);                    // check not empty word list
                Assert.AreEqual(context.Book.Count(),2);    
            }
        }

        [TestMethod]
        public async Task TestPostBook()
        {
            using (var context = new bookAPIContext(options))
            {
                BooksController booksController = new BooksController(context, mapper);
               
                ActionResult<Book> result = await booksController.PostBook(data); // declared data object but it is null, check null string
                Assert.AreEqual(context.Book.Count(), 2);   
            }
        }


        [TestMethod]
        public async Task TestDeleteBook()
        {
            using (var context = new bookAPIContext(options))
            {
                BooksController booksController = new BooksController(context, mapper);
                ActionResult<Book> result = await booksController.DeleteBook(1);  
                Assert.AreEqual(context.Book.Count(), 1);    // 1 book should remain after deleting book id 1
            }
        }

       
    }

}