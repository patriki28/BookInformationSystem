using BookInformationSystem.Models.Domain;
using BookInformationSystem.Repositories.Abstract;

namespace BookInformationSystem.Repositories.Implementation
{
    public class BookService : IBookService
    {
        private readonly DatabaseContext context;
        public BookService(DatabaseContext context)
        {
            this.context = context;
        }
        public bool Add(Book model)
        {
            try
            {
                context.Book.Add(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.FindById(id);
                if (data == null)
                    return false;
                context.Book.Remove(data);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Book FindById(int id)
        {
            return context.Book.Find(id);
        }

        public IEnumerable<Book> GetAll()
        {
            var books = from b in context.Book
                        join a in context.Author on b.AuthorId equals a.Id
                        join p in context.Publisher on b.PubhlisherId equals p.Id
                        join g in context.Genre on b.GenreId equals g.Id
                        select new Book
                        {
                            Id = b.Id,
                            AuthorId = b.AuthorId,
                            GenreId = b.GenreId,
                            Isbn = b.Isbn,
                            PubhlisherId = b.PubhlisherId,
                            Title = b.Title,
                            TotalPages = b.TotalPages,
                            GenreName = g.Name,
                            AuthorName = a.AuthorName,
                            PublisherName = p.PublisherName
                        };


            return books;
        }

        public bool Update(Book model)
        {
            try
            {
                context.Book.Update(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
