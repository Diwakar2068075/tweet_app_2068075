using System;
using System.Data.SqlClient;
namespace TweetApp
{
    class Program
    {
        static readonly string connectionString = "Server=DESKTOP-H75F7CD;Database=TweetApp;Trusted_Connection=true";
        static  SqlConnection sqlConnection;// new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            string displayTop = "Tweet App";
            Console.SetCursorPosition((Console.WindowWidth - displayTop.Length) / 2, Console.CursorTop);
            Console.WriteLine(displayTop);
            Console.WriteLine();
            int choice;
            top:
            do
            {
                Console.WriteLine();
                Console.WriteLine(" 1. Sign up / Register ");
                Console.WriteLine(" 2. Login ");
                Console.WriteLine(" 3. Forgot Password ");
                Console.WriteLine(" 4. Exit ");
                Console.WriteLine();
                Console.Write(" Please Enter your Choice : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 2:
                        Console.WriteLine();
                        Login();
                        goto top ;
                    case 1:
                        Console.WriteLine();
                        Register();
                        return;
                    case 3:
                        Console.WriteLine();
                        Forget();
                        return;
                    case 4:
                        Console.WriteLine("Good Bye Hope You enjoyed it!");
                        return;
                    default:
                        Console.WriteLine("Please enter the correct choice");
                        break;
                }
            } while (choice != 4);
            static void Login()
            {
                string userId = null, passWord = null;
                Console.WriteLine();
                Console.Write(" Enter UserID : ");
                userId = Console.ReadLine();

                if (userId != string.Empty)
                {
                    Console.Write(" Enter Password : ");
                    passWord = Console.ReadLine();
                }
                Console.WriteLine();
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand("select count(*) from UserData where UserId=@userid and Password=@pass", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@userid", userId);
                        sqlCommand.Parameters.AddWithValue("@pass", passWord);
                        int result = (int)sqlCommand.ExecuteScalar();
                        SqlCommand sqlCommand1 = new SqlCommand("select UserName from UserData where UserId=@userid and Password=@pass", sqlConnection);
                        sqlCommand1.Parameters.AddWithValue("@userid", userId);
                        sqlCommand1.Parameters.AddWithValue("@pass", passWord);
                        string userName =(string)sqlCommand1.ExecuteScalar();
                        sqlConnection.Close();
                        if (result > 0)
                        {
                            Console.WriteLine("Logged In Successfully");
                            LoggedMenu(userName);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Credentials");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                   
                }
            }
            static void LoggedMenu(string userName)
            {
                Console.WriteLine();
                Console.WriteLine(" Hello "+ userName + " Welcome to Tweet App ");
                int loggedChoice;
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("1. Post a tweet");
                    Console.WriteLine("2. View my tweets");
                    Console.WriteLine("3. View all tweets");
                    Console.WriteLine("4. View all users");
                    Console.WriteLine("5. Reset Password");
                    Console.WriteLine("6. Exit");
                    Console.WriteLine();
                    Console.Write("Enter Your Choice : ");
                    loggedChoice = Convert.ToInt32(Console.ReadLine());

                    switch (loggedChoice) 
                    {
                        case 1:
                            Console.WriteLine();
                            PostTweet(userName);
                            break;
                        case 2:
                            Console.WriteLine("View my tweets");
                            ViewMyTweets(userName);
                            break;
                        case 3:
                            Console.WriteLine("View all tweets");
                            ViewAllTweets();
                            break;
                        case 4:
                            Console.WriteLine("View all users");
                            ViewAllUsers();
                            break;
                        case 5:
                            Console.WriteLine("Reset Password");
                            ResetPassword(userName);
                            break;
                        case 6:
                            Console.WriteLine("Bye Bye " + userName + " Have a Nice Day! ");
                            Console.WriteLine();
                            break;
                         //   goto top;
                           
                        default:
                            Console.WriteLine("Please Enter correct choice ");
                            break;
                    }
                } while (loggedChoice != 6);
            }
            static void PostTweet(string userName)
            {
                string tweetMessage;

                Console.Write(" Enter your tweet : ");
                tweetMessage = Console.ReadLine();
                Console.WriteLine();
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        string query = "Insert into TweetData(UserName,Tweets) Values('" + userName + "','"  + tweetMessage + "')";
                        SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
                        da.SelectCommand.ExecuteNonQuery();
                        //sqlConnection.Close();
                        Console.WriteLine(" Your Tweet has been posted Successfully");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                    Console.WriteLine();
                }


            }
            static void ViewMyTweets(string userName)
            {
                Console.WriteLine();
                Console.WriteLine(" Retriving Your tweets......");
                System.Threading.Thread.Sleep(1000);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                       // Console.WriteLine("Connection successful!");
                        SqlCommand command = new SqlCommand("SELECT * FROM TweetData WHERE UserName = @UserName", sqlConnection);
                        command.Parameters.AddWithValue("@UserName",userName);
                        command.Parameters.Add(new SqlParameter("0", 1));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("\nUserName\tMessage\t");
                            while (reader.Read())
                            {
                                Console.WriteLine(String.Format("{0} \t | {1} \t ", reader[1], reader[2]));
                            }
                        }
                       // sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally { sqlConnection.Close(); }
                }
                Console.WriteLine();
                
            }
            static void ViewAllTweets() 
            {
                Console.WriteLine();
                Console.WriteLine("Retriving all tweets....");
                System.Threading.Thread.Sleep(1500);

                using (sqlConnection = new SqlConnection(connectionString)) 
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand("SELECT * FROM TweetData",sqlConnection);
                        command.Parameters.Add(new SqlParameter("0", 1));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("\nUserId\t\t\tMessage\t");
                            while (reader.Read())
                            {
                                Console.WriteLine(String.Format("{0} \t | {1} \t ",reader[1], reader[2]));
                            }
                        }
                        Console.WriteLine();
                       // sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally { sqlConnection.Close(); }
                }
                Console.WriteLine();
            }
            static void ViewAllUsers() 
            {
                Console.WriteLine();
                Console.WriteLine("Retriving all users.......");
                System.Threading.Thread.Sleep(1000);
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand("SELECT * FROM UserData", sqlConnection);
                        command.Parameters.Add(new SqlParameter("0", 1));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("\nUserName\tUserId\t");
                            while (reader.Read())
                            {
                                Console.WriteLine(String.Format("{0} \t | {1} \t ", reader[4], reader[3]));
                            }
                        }
                        Console.WriteLine();
                       // sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally { sqlConnection.Close(); }
                }
                Console.WriteLine();

            }
            static void ResetPassword(string userName)
            {
                Console.WriteLine();
                retry:
                string oldPassword;
                Console.Write(" Enter your current password : ");
                oldPassword = Console.ReadLine();
                using (sqlConnection = new SqlConnection(connectionString)) 
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCommand1 = new SqlCommand("select Password from UserData where UserName=@username ", sqlConnection);
                        sqlCommand1.Parameters.AddWithValue("@username", userName);
                        string currentPassword = (string)sqlCommand1.ExecuteScalar();
                        if (currentPassword == oldPassword)
                        {
                            Console.Write("Enter New Password : ");
                            string newPassword = Console.ReadLine();
                            SqlCommand UpdateCommand = new SqlCommand("UPDATE UserData SET Password = @Password WHERE UserName = @UserName", sqlConnection);
                            UpdateCommand.Parameters.Add(new SqlParameter("Password", newPassword));
                            UpdateCommand.Parameters.Add(new SqlParameter("UserName", userName));
                            UpdateCommand.ExecuteNonQuery();
                            // sqlConnection.Close();
                            Console.WriteLine("New Password set successfully. ");

                        }
                        else
                        {
                            Console.WriteLine("Password is not correct.");
                            goto retry;
                            // sqlConnection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally { sqlConnection.Close(); }
                }
                Console.WriteLine();
                
            }
            //static void Logout() 
            //{
            //    sqlConnection.Close();
            //    Console.WriteLine("Logout Executed");
            //}
            static void Register()
            {

                string firstName,lastName;
                char gender;
                string email;
                string userName = null, passWord = null, confirmPassword = null;

                Console.Write("Enter First Name : ");
                firstName = Console.ReadLine();

                Console.Write("Enter Last Name (optional) : ");
                lastName = Console.ReadLine();

                Console.Write("Enter Gender (M/F) : ");
                gender = Convert.ToChar(Console.ReadLine());
                

                Console.Write("Enter your Email (Used for Login) : ");
                email = Console.ReadLine();

                Console.Write("Enter your username : ");
                userName = Console.ReadLine();

                if (userName != string.Empty)
                {
                    retry:
                    Console.Write("Enter Password : ");
                    passWord = Console.ReadLine();
                    Console.Write("Confirm Password : ");
                    confirmPassword = Console.ReadLine();
                    
                    if (passWord == confirmPassword)
                    {
                        using (sqlConnection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                sqlConnection.Open();
                                string query = "Insert into UserData(first_name,last_name,gender,UserId,UserName,Password) Values('" + firstName + "','" + lastName + "','" + gender + "','"  + email + "','" + userName + "','" + passWord + "')";
                                SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
                                da.SelectCommand.ExecuteNonQuery();
                              
                                //sqlConnection.Close();
                                Console.WriteLine("Registered Successfully");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine( e.Message);
                            }
                            finally
                            {
                                sqlConnection.Close();
                               // Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Passwords are not matching. Please try again!");
                        goto retry;
                    }
                }
            }
            static void Forget() 
            {
                Console.Write("Enter your UserId : ");
                string userId = Console.ReadLine();

                Console.Write("Enter your username : ");
                string userName = Console.ReadLine();

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand("select count(*) from UserData where UserId=@userId and UserName=@username", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@userId", userId);
                        sqlCommand.Parameters.AddWithValue("@username", userName);
                        int result = (int)sqlCommand.ExecuteScalar();
                        if (result > 0)
                        {
                            
                            Console.Write("Enter your new password : ");
                            string password = Console.ReadLine();
                            SqlCommand UpdateCommand = new SqlCommand("UPDATE UserData SET Password = @Password WHERE UserId = @UserId", sqlConnection);
                            UpdateCommand.Parameters.Add(new SqlParameter("Password", password));
                            UpdateCommand.Parameters.Add(new SqlParameter("UserId", userId));
                            UpdateCommand.ExecuteNonQuery();
                            Console.WriteLine("New Password set successfully. ");
                        }
                        else
                        {
                            Console.WriteLine("Given UserId does not exist.");
                        }
                       // sqlConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                       // Console.ReadKey();
                    }
                }
            }
            
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
