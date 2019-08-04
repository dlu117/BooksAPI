using bookAPI.Controllers;
using bookAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace UnitTestbookAPI
{
    [TestClass]
    public class WordsControllerUnitTests
    {

        // creates a configuration object / bookAPIContext object access memory database
        public static readonly DbContextOptions<bookAPIContext> options
        = new DbContextOptionsBuilder<bookAPIContext>()
        .UseInMemoryDatabase(databaseName: "testDatabase")
        .Options;


        // adding dummy data to test database 

        public static IList<Word> words = new List<Word>{
          new Word()
          {
            Word1 = "Harry"
          },
          new Word()
          {
            Word1 = "Potter"
          },
            new Word()
          {
            Word1 = "Rowling"
          }
          };


        // Words unit testing pretty generic as scaffolding will generally mean they will work
        // More interesting edge cases in 

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new bookAPIContext(options))
            {
                // populate the mock db
                context.Word.Add(words[0]);
                context.Word.Add(words[1]);
                context.Word.Add(words[2]);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new bookAPIContext(options))
            {
                // clear the db
                context.Word.RemoveRange(context.Word);
                context.SaveChanges();
            };
        }


        [TestMethod]
        public async Task TestGetWord()
        {
            using (var context = new bookAPIContext(options))
            {
                WordsController wordsController = new WordsController(context);
                ActionResult<IEnumerable<Word>> result = await wordsController.GetWord();
                Assert.IsNotNull(result);          // check not empty word list
                Assert.AreEqual(context.Word.Count(), 3);    // check 3 elements in word list
            }
        }


        [TestMethod]
        public async Task TestPutWord()
        {
            using (var context = new bookAPIContext(options))
            {
                string newWord = "Harry"; // this will not test duplicate words because its replacing
                Word newword1 = context.Word.Where(x => x.Word1 == words[0].Word1).Single();
                newword1.Word1 = newWord;  // replacing word

                WordsController wordsController = new WordsController(context);
                IActionResult result = await wordsController.PutWord(newword1.WordId, newword1) as IActionResult;  // making new controller object to test

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult)); // Put returns no content


            }
        }

        [TestMethod]
        public async Task TestDeleteWord()
        {
            using (var context = new bookAPIContext(options))
            {
                WordsController wordsController = new WordsController(context);
                ActionResult<Word> result = await wordsController.DeleteWord(words[0].WordId);
                Assert.IsNotNull(result);
                // not use Assert.AreEqual(4,words.Count); Words.Count will always be 3 whereas context.Word.Count() will check length of db of the context
                Assert.AreEqual(context.Word.Count(), 2);
            }
        }



        [TestMethod]
        public async Task TestPostWord()
        {
            using (var context = new bookAPIContext(options))
            {
                Word newword = new Word
                {
                    Word1 = "Spell"
                };
                WordsController wordsController = new WordsController(context);
                ActionResult<Word> result = await wordsController.PostWord(newword);
                Assert.IsNotNull(result);
                Assert.AreEqual(context.Word.Count(), 4);


            }
        }










    }

}