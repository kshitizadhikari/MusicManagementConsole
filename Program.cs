﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MusicManagementConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Server=DESKTOP-72AIJKL;Initial Catalog=MusicStreamService;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                bool isRunning = true;
                while (isRunning)
                {
                    mainMenu(connection);
                }

                void mainMenu(SqlConnection conn)
                {
                    Console.Clear();
                    Console.WriteLine("\n-----MENU-----");
                    Console.WriteLine("Create User = 1");
                    Console.WriteLine("Create Artist = 2");
                    Console.WriteLine("Login as User = 3");
                    Console.WriteLine("Login as Artist = 4");
                    Console.WriteLine("Exit = 5");
                    Console.Write("\nEnter your choice: ")
                    char mainChoice = Console.ReadKey().KeyChar;

                    int userId = 0;
                    int artistId = 0;

                    switch (mainChoice)
                    {
                        case '1':
                            //create user
                            createUser(conn);
                            break;

                        case '2':
                            //create artist
                            createArtist(conn);
                            break;

                        case '3':
                            //login as user
                            userId = userAuthentication(conn);
                            //if (userId != 0)
                            //{
                            //    Console.Clear();
                            //    userMenu(conn, userId);
                            //}
                            break;


                        case '4':
                            //login as artist
                            artistId = artistAuthentication(conn);
                            bool isArtistMenuRunning = true;
                            if (artistId != 0)
                            {
                                Console.Clear();
                                while(isArtistMenuRunning)
                                {
                                isArtistMenuRunning = artistMenu(conn, artistId);

                                }
                            }
                            break;


                        case '5':
                            isRunning = exit();
                            break;

                        default:
                            Console.WriteLine("Invalid Choice");
                            break;
                    }
                }

                void createUser(SqlConnection conn)
                {
                    Console.Clear();
                    Console.WriteLine("Create User Menu\n");
                    Console.Write("Enter User Name: ");
                    String userName = Console.ReadLine();
                    Console.Write("Enter email: ");
                    String email = Console.ReadLine();
                    Console.Write("Enter password: ");
                    String password = Console.ReadLine();
                    Console.Write("Enter bio: ");
                    String bio = Console.ReadLine();
                    String insertQuery = "INSERT INTO MSS.Users(userName, email, password, bio) VALUES(@userName, @email, @password, @bio)";
                    using (SqlCommand command = new SqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@userName", userName);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@bio", bio);
                        command.ExecuteNonQuery();
                    }

                }

                int userAuthentication(SqlConnection conn)
                {
                    Console.Write("\nEnter email: ");
                    String email = Console.ReadLine();
                    Console.Write("Enter password: ");
                    String password = Console.ReadLine();
                    String selectQuery = "SELECT * FROM MSS.Users where email=@email and password=@password";
                    using (SqlCommand cmd1 = new SqlCommand(selectQuery, conn))
                    {
                        cmd1.Parameters.AddWithValue("@email", email);
                        cmd1.Parameters.AddWithValue("@password", password);
                        SqlDataReader reader = cmd1.ExecuteReader();
                        if (reader.Read())
                        {
                            int userId = (int)reader["userId"];
                            return userId;
                        }
                        else
                        {
                            Console.WriteLine("No such user exists.");
                            return 0;
                        }
                        reader.Close();
                    }

                }



                void createArtist(SqlConnection conn)
                {
                    Console.Clear();
                    Console.WriteLine("Create Artist Menu\n");
                    Console.Write("Enter Artist Name: ");
                    String artistName = Console.ReadLine();
                    Console.Write("Enter Emai: ");
                    String email = Console.ReadLine();
                    Console.Write("Enter password: ");
                    String password = Console.ReadLine();
                    Console.Write("Enter genre: ");
                    String genre = Console.ReadLine();
                    Console.Write("Enter bio: ");
                    String bio = Console.ReadLine();
                    String insertQuery = "INSERT INTO MSS.Artists(artist_name, aEmail, aPassword, genre, bio) VALUES(@artistName, @email, @password, @genre, @bio)";
                    using (SqlCommand command = new SqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@artistName", artistName);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@genre", genre);
                        command.Parameters.AddWithValue("@bio", bio);
                        command.ExecuteNonQuery();

                    }

                }

                int artistAuthentication(SqlConnection conn)
                {
                    Console.Write("\nEnter email: ");
                    String email = Console.ReadLine();
                    Console.Write("Enter password: ");
                    String password = Console.ReadLine();
                    String selectQuery = "SELECT * FROM MSS.Artists where aEmail=@email and aPassword=@password";
                    SqlCommand cmd1 = new SqlCommand(selectQuery, conn);
                    cmd1.Parameters.AddWithValue("@email", email);
                    cmd1.Parameters.AddWithValue("@password", password);
                    SqlDataReader reader = cmd1.ExecuteReader();
                    if (reader.Read())
                    {
                        int artistId = (int)reader["artist_id"];
                        reader.Close();
                        return artistId;
                    }
                    else
                    {
                        Console.WriteLine("No such Artist exists.");
                        reader.Close();
                        return 0;
                    }
                }

                bool artistMenu(SqlConnection conn, int artistId)
                {
                artistMenu:
                    bool artistMenuExitVal = false;
                    Console.WriteLine("\n-----ArtistMenu-----");
                    Console.WriteLine("Create Album = 1");
                    Console.WriteLine("Add a track to a Album = 2");
                    Console.WriteLine("View your Music = 3");
                    Console.WriteLine("Goto MainMenu = 4");
                    Console.Write("\nEnter your choice: ");
                    char artistChoice = Console.ReadKey().KeyChar;
                    Console.Clear();
                    switch (artistChoice)
                    {
                        case '1':
                            createAlbum(conn, artistId);
                            break;

                        case '2':
                            break;

                        case '3':
                            break;

                        case '4':
                            artistMenuExitVal = exit();
                            break;

                        default:
                            Console.WriteLine("Invalid Input");
                            goto artistMenu;
                            break;
                    }
                    return artistMenuExitVal;
                }

                void createAlbum(SqlConnection conn, int artistId)
                {
                    Console.Clear();
                    Console.WriteLine("Artist ID: " + artistId);
                    Console.Write("Enter AlbumId: ");
                    char albumId = Console.ReadKey().KeyChar;
                    Console.Write("\nEnter Album Name: ");
                    String albumName = Console.ReadLine();
                    Console.Write("Enter Release Date: ");
                    String releaseDate = Console.ReadLine();
                    Console.Write("Enter CoverArt: ");
                    String coverArt = Console.ReadLine();
                    String insertQuery = "INSERT INTO MSS.Albums(artist_id, album_id, album_name, release_date, cover_art) VALUES (@artistId, @albumId, @albumName, @releaseDate, @coverArt)";
                    using (SqlCommand command = new SqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@artistId", artistId);
                        command.Parameters.AddWithValue("@albumId", albumId);
                        command.Parameters.AddWithValue("@albumName", albumName);
                        command.Parameters.AddWithValue("@releaseDate", releaseDate);
                        command.Parameters.AddWithValue("@coverArt", coverArt);
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("New album created successfully.");
                    Console.ReadLine();
                    artistMenu(conn, artistId);
                }

                bool exit()
                {
                exitTryAgain:
                    Console.Write("\nAre you sure? (y / n): ");
                    char val = Console.ReadKey().KeyChar;
                    if (val == 'y') return false;
                    else if (val == 'n') return true;
                    else goto exitTryAgain;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
