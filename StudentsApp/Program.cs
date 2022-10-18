using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq.Expressions;

#region User Manual
/* User Manual
 1. Exit
*/
#endregion
namespace StudentsApp
{
    
    public class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["StudentsDb"].ConnectionString;
        private static SqlConnection sqlConnection = null;
        static void Main(string[] args)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine("StudentsApp");

            SqlDataReader sqlDataReader = null;// обєкт за допомогою якого виборку баз даних 

            string command = String.Empty; // зберігати записану команду яку пользователь буде писати 

            while (true) // створили нескінченний цикл
            {
                try
                {

                    Console.Write("> "); // Write бо пользователь повинен написати в цій же строці 
                    command = Console.ReadLine(); // пользователь введе якусь команду 

                    #region Exit
                    if (command.ToLower().Equals("exit")) // якщо exit в нижньому регістрі і рівний "exit" то 
                    {
                        if (sqlConnection.State == ConnectionState.Open)// подключение 
                        {
                            sqlConnection.Close();// виключаем 
                        }
                        if (sqlDataReader != null)// і якщо sqlDataReader має ссилку 
                        {
                            sqlDataReader.Close();// закриваем 
                        }
                        break;// виходим з циклу 
                    }
                    #endregion

                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                    // SELECT * FROM [Students] WHERE Id = 1
                    switch (command.Split(' ')[0].ToLower())// створюєм массив відстрок з розділячем ' ' по індексу
                    {
                        case "select":

                            sqlDataReader = sqlCommand.ExecuteReader();// в базі даних за допомогою команди буде виборка і поверне 2мірний массив строк 

                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{sqlDataReader["Id"]}\t" +
                                                  $"{sqlDataReader["FIO"]}\t" +
                                                  $"{sqlDataReader["Birthday"]}\t" +
                                                  $"{sqlDataReader["University"]}\t" +
                                                  $"{sqlDataReader["Group_number"]}\t" +
                                                  $"{sqlDataReader["Course "]}\t" +
                                                  $"{sqlDataReader["Avarage_score"]}");
                                Console.WriteLine(new String('-', 80));
                            }
                            if (sqlDataReader != null)// і якщо sqlDataReader має ссилку 
                            {
                                sqlDataReader.Close();// закриваем 
                            }

                            break;

                        case "insert":

                            Console.WriteLine($"Додано {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;

                        case "update":

                            Console.WriteLine($"Змінено {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;

                        case "delete":

                            Console.WriteLine($"Видалено {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;

                       
                        default:
                            Console.WriteLine($"Команда {command} неккоректна! ");
                            break;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            Console.WriteLine("Для продовження натисніть любу клавішу ...");
            Console.ReadKey();
        }
    }
}