using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NNTP_Projekt_D25.Services;

public static class NntpService
{
    public static string? greetingResponse;
    public static string? loginResponse;
    
    public static void Create_Connection(string username, string password)
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
            
            greetingResponse = reader.ReadLine();
            Console.WriteLine("Server greeting: " + greetingResponse);

            // Tjekker om der er hul igennem til serveren - 200
            if (NntpResponseChecker.IsGreetingSuccessful(greetingResponse))
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
            string? response = reader.ReadLine();
            Console.WriteLine("Sender username... " + username);
            Console.WriteLine("Server: " + response);

            // Send password når serveren returnerer 381
            if (NntpResponseChecker.ShouldSendPassword(response))
            {
                writer.WriteLine($"AUTHINFO PASS {password}");
                loginResponse = reader.ReadLine();
                Console.WriteLine("Sender password... " + password);
                Console.WriteLine("Server: " + loginResponse);

                // Tjekker om login er succesfuld - 281
                if (NntpResponseChecker.IsLoginSuccessful(loginResponse))
                {
                    Console.WriteLine("Login successful!");
                }
                else
                {
                    Console.WriteLine("Authentication failed or was not accepted.");
                    return;
                }


            }
            else
            {
                return;
            }

            // Request list of newsgroups
            writer.WriteLine("LIST");
            response = reader.ReadLine();
            Console.WriteLine("Server: " + response);

            // Check if the server accepted the LIST command
            if (response != null && response.StartsWith("215"))
            {
                // Console.WriteLine(reader.ReadLine());
                // Console.WriteLine(reader.ReadLine());
                // Console.WriteLine(reader.ReadLine());
                List<string> newsgroups = new List<string>();
                
                string? line;

                for (int i = 0; i < 5; i++)
                {
                    line = reader.ReadLine();
                    if (line != null && line != ".")
                    {
                        newsgroups.Add(line);
                    }
                }

                Console.WriteLine("\nNewsgroups:");
                
                foreach (string group in newsgroups)
                {
                    Console.WriteLine(group);
                }
            }
            else
            {
                Console.WriteLine("LIST command failed.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Fejl: " + ex.Message);
        }
    }
}