using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManager.Models
{
    public class AuthorRepositoryAdo : IAuthorRepository
    {
        private string connStr = "Server=(localdb)\\mssqllocaldb;Database=aspnet-AuthorManager-49678168-872E-4274-9F14-86E90C13317A;Trusted_Connection=True;MultipleActiveResultSets=true";

        private string selectQuery = "SELECT Id, FName, LName, Email\n" +
            "FROM Authors\n";

        private string selectByIdClause = "WHERE Id = @id\n";

        private string orderByName = "ORDER BY FName asc, LName\n";

        private string insertAuthorQuery = "INSERT INTO Authors\n" +
            "(FName,LName,Email)\n" +
            "values(@fname,@lname,@email)\n";

        public List<Author> ListAll()
        {
            List<Author> authors = new List<Author>();

            using (SqlConnection conn = new SqlConnection(connStr)) // This should implement IDisposable
            {
                SqlCommand command = new SqlCommand(selectQuery + orderByName, conn);

                try
                {
                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        Author newAuthor = new Author
                        {
                            Id = int.Parse(reader[0].ToString()),
                            FName = reader[1].ToString(),
                            LName = reader[2].ToString(),
                            Email = reader[3].ToString()
                        };

                        authors.Add(newAuthor);
                    }
                }
                catch (Exception ex)
                {
                    // TODO add logging
                    throw;
                }

            }

            return authors;
        }

        public Author GetById(int id)
        {
            Author author = new Author();

            using (var conn = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(selectQuery + selectByIdClause, conn);

                command.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        author = new Author
                        {
                            Id = int.Parse(reader[0].ToString()),
                            FName = reader[1].ToString(),
                            LName = reader[2].ToString(),
                            Email = reader[3].ToString()
                        };
                    }
                }
                catch (Exception ex)
                {
                    // TODO Add Logging

                    throw;
                }
            }

            return author;
        }

        public void AddAuthor(Author newAuthor)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    var command = new SqlCommand(insertAuthorQuery, conn);

                    command.Parameters.AddWithValue("@fname", newAuthor.FName);
                    command.Parameters.AddWithValue("@lname", newAuthor.LName);
                    command.Parameters.AddWithValue("@email", newAuthor.Email);

                    conn.Open();

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // TODO Add logging

                    throw;
                }
            }
        }
    }
}
