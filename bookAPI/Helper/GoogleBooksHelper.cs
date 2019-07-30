using bookAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace bookAPI.Helper
{
    public class GoogleBooksHelper
    {
        public static void testProgram()
        {
            //Console.WriteLine(GetBookInfo(GetbookIdFromURL("https://books.google.co.nz/books?id=jk87_y-ubE0C&printsec=frontcover&dq=harry+potter&hl=en&sa=X&ved=0ahUKEwip37CggNrjAhWOA3IKHcyJBIgQ6AEIKjAA#v=onepage&q=harry%20potter&f=false"), "https://books.google.co.nz/books?id=jk87_y-ubE0C&printsec=frontcover&dq=harry+potter&hl=en&sa=X&ved=0ahUKEwip37CggNrjAhWOA3IKHcyJBIgQ6AEIKjAA#v=onepage&q=harry%20potter&f=false"));

            Console.WriteLine(GetWords(GetbookIdFromURL("https://books.google.co.nz/books?id=iO5pApw2JycC&printsec=frontcover&dq=harry+potter&hl=en&sa=X&ved=0ahUKEwj4g4vw7tvjAhUSheYKHbmxAqwQ6AEIMDAB#v=onepage&q=harry%20potter&f=false")));
            // Pause the program execution
            Console.ReadLine();
        }

        public static String GetbookIdFromURL(String bookURL)
        {
            // TODO - Extract the book id from the book link.
            int indexOfFirstId = bookURL.IndexOf("&");
            String bookstr = bookURL.Substring(0, indexOfFirstId);
            int indexOfSecondId = bookURL.IndexOf("=") + 1;
            String bookId = bookstr.Substring(indexOfSecondId);
            return bookId;
        }

        public static Book GetBookInfo(String bookId, String bookURL)
        {
            String APIKey = "AIzaSyA2_5QAyF3QBgVzm5SCE2EyH7s6W6Z-KbA";
            String GoogleBooksAPIURL = "https://www.googleapis.com/books/v1/volumes/" + bookId + "?key=" + APIKey;

            //Use an http client to grab the JSON string from the web.
            String bookInfoJSON = new WebClient().DownloadString(GoogleBooksAPIURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(bookInfoJSON);

            // Extract information from the dynamic object.
            String title = jsonObj["volumeInfo"]["title"];
            String author = jsonObj["volumeInfo"]["authors"][0];
            String thumbnailURL = jsonObj["volumeInfo"]["imageLinks"]["medium"];
            String pageString = jsonObj["volumeInfo"]["pageCount"];
            String bookUrl = bookURL;
            int bookpage = XmlConvert.ToInt32(pageString);
            Book book = new Book
            {
                BookTitle = title,
                BookAuthor = author,
                BookPages = bookpage,
                WebUrl = bookUrl,
                ThumbnailUrl = thumbnailURL,
                IsRead = false
            };
            return book;
        }

        public static String CleanDescription(String description)
        {
            description = description.Replace(",", "");
            description = description.Replace(".", "");
            description = description.Replace("</p>", "");
            description = description.Replace("<p>", "");
            description = description.Replace("<i>", "");
            description = description.Replace("</i>", "");
            description = description.Replace("<br>", "");
            description = description.Replace("</br>", "");  
            return description;
        }

        public static String GetDescriptionString(String bookId)
        {
            String APIKey = "AIzaSyA2_5QAyF3QBgVzm5SCE2EyH7s6W6Z-KbA";
            String GoogleBooksAPIURL = "https://www.googleapis.com/books/v1/volumes/" + bookId + "?key=" + APIKey;
            //Use an http client to grab the JSON string from the web.
            String bookInfoJSON = new WebClient().DownloadString(GoogleBooksAPIURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(bookInfoJSON);

            // Extract information from the dynamic object.
            String description = jsonObj["volumeInfo"]["description"];
            description = CleanDescription(description);
            return description;

        }

        public static List<Word> GetWords(String bookId)
       {
            String descriptionstring = GetDescriptionString(bookId);

            List<Word> words = new List<Word>();
            String[] descriptions = descriptionstring.Split(" ");
            for(int i=0; i < descriptions.Length; i++)
            {
                if(descriptions[i] != "")
                {
                    Word word = new Word {
                        Word1 = descriptions[i]
                    };
                 words.Add(word);
                }
               
            }
            return words;
       }


    }
}
