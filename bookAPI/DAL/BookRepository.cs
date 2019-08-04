using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bookAPI.Model;

namespace bookAPI.DAL
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private bookAPIContext context;

        public BookRepository(bookAPIContext context)
        {
            this.context = context;
        }

        public IEnumerable<Book> GetBooks()
        {
            return context.Book.ToList();
        }

        public Book GetBookByID(int id)
        {
            return context.Book.Find(id);
        }

        public void InsertBook(Book book)
        {
            context.Book.Add(book);                    // Note to self have used before
        }

        public void DeleteBook(int bookId)
        {
            Book book = context.Book.Find(bookId);
            context.Book.Remove(book);
        }

        public void UpdateBook(Book book)
        {
            context.Entry(book).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
