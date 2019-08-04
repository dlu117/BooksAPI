using bookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookAPI.DAL
{
    public interface IBookRepository:IDisposable
    {
        IEnumerable<Book> GetBooks();
        Book GetBookByID(int BookId);
        void InsertBook(Book book);
        void DeleteBook(int BookId);
        void UpdateBook(Book book);
        void Save();
    }


}
