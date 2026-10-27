using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace NNTP_Projekt_D25.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Send_UserPass(object? sender, RoutedEventArgs e)
    {
        
        Create_Connection(box_user.Text, box_pass.Text);
    }

    private void Create_Connection(string username, string password)
    {
        const string host = "news.eternal-september.org";
        const int port = 119;

        try
        {
            using var client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            client.Connect(host, port);

            using NetworkStream ns = client.GetStream();
            using var reader = new StreamReader(ns, Encoding.ASCII);
            using var writer = new StreamWriter(ns, Encoding.ASCII)
            {
                NewLine = "\r\n",
                AutoFlush = true
            };
            
            string? response = reader.ReadLine();
            Console.WriteLine("Server greeting: " + response);

            if (response.StartsWith("200"))
            {
                Console.WriteLine("Connection success");
            }
            else
            {
                Console.WriteLine("Connection failed");
                return;
            }

            // Send username
            writer.WriteLine($"AUTHINFO USER {username}");
            response = reader.ReadLine();
            Console.WriteLine("Sender username... " + username);
            Console.WriteLine("Server: " + response);

            if (response?.StartsWith("381") == true)
            {
                // Send password når serveren returnerer 381
                writer.WriteLine($"AUTHINFO PASS {password}");
                response = reader.ReadLine();
                Console.WriteLine("Sender password... " + password);
                Console.WriteLine("Server: " + response);
                
                if (response?.StartsWith("281") == true)
                {
                    Console.WriteLine("Login successful!");
                }
                else
                {
                    Console.WriteLine("Authentication failed or was not accepted.");
                }
            }



        }
        catch (Exception ex)
        {
            Console.WriteLine("Fejl: " + ex.Message);
        }
    }
}