using BookstoreAdoNetApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BookstoreAdoNetApp.DataAccess
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // Connected Architecture using SqlDataReader
        public List<Book> GetAllBooksUsingReader()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_GetAllBooks", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            BookId = Convert.ToInt32(reader["BookId"]),
                            Title = reader["Title"].ToString()!,
                            Author = reader["Author"].ToString()!,
                            Price = Convert.ToDecimal(reader["Price"]),
                            PublicationYear = Convert.ToInt32(reader["PublicationYear"])
                        };

                        books.Add(book);
                    }
                }
            }

            return books;
        }

        public Book? GetBookById(int id)
        {
            Book? book = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_GetBookById", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@BookId", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        book = new Book
                        {
                            BookId = Convert.ToInt32(reader["BookId"]),
                            Title = reader["Title"].ToString()!,
                            Author = reader["Author"].ToString()!,
                            Price = Convert.ToDecimal(reader["Price"]),
                            PublicationYear = Convert.ToInt32(reader["PublicationYear"])
                        };
                    }
                }
            }

            return book;
        }

        public void AddBook(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_AddBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Parameterized queries prevent SQL Injection
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Price", book.Price);
                command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateBook(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@BookId", book.BookId);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Price", book.Price);
                command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBook(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@BookId", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Disconnected Architecture using SqlDataAdapter and DataSet
        public DataSet GetBooksUsingDataSet()
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT BookId, Title, Author, Price, PublicationYear FROM Books", connection);
                adapter.Fill(dataSet, "Books");
            }

            return dataSet;
        }

        // Example of DataTable usage in disconnected architecture
        public DataTable GetBooksUsingDataTable()
        {
            DataSet dataSet = GetBooksUsingDataSet();
            return dataSet.Tables["Books"]!;
        }
    }
}
