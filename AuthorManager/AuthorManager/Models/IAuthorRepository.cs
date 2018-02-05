using System.Collections.Generic;

namespace AuthorManager.Models
{
    public interface IAuthorRepository
    {
        void AddAuthor(Author newAuthor);
        Author GetById(int id);
        List<Author> ListAll();
    }
}