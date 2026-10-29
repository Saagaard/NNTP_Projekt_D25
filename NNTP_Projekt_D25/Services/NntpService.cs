﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using Avalonia.Controls;
using NNTP_Projekt_D25.Views;

namespace NNTP_Projekt_D25.Services;

public static class NntpService
{
    public static string? response;
    public static string? greetingResponse;
    public static string? loginResponse;
    private static NetworkStream? ns;
    private static StreamReader? reader;
    private static StreamWriter? writer;
    private static TcpClient? client;
    private static IList<string>? newsgroups;
    
    public static void Create_Connection(string username, string password)
    {
        const string host = "news.eternal-september.org";
        const int port = 119;

        try
        {
            client = new TcpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            client.Connect(host, port);

            ns = client.GetStream();
            reader = new StreamReader(ns, Encoding.ASCII);
            writer = new StreamWriter(ns, Encoding.ASCII)
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
            response = reader.ReadLine();
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
                    GetNewsgroups();
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

        }
        catch (Exception ex)
        {
            Console.WriteLine("Fejl: " + ex.Message);
        }
    }

    public static IList<string> GetNewsgroups()
    {
        if (newsgroups != null) {
            return newsgroups; 
        }
        if (NntpResponseChecker.IsLoginSuccessful(loginResponse))
        {

            // Request list of newsgroups
            writer.WriteLine("LIST");
            response = reader.ReadLine();
            Console.WriteLine("Server: " + response);

            // Check if the server accepted the LIST command
            if (response != null && response.StartsWith("215"))
            {
                IList<string> lines = new List<string>();
                newsgroups = new List<string>();

                string? line;

                while ((line = reader.ReadLine()) != null && line != ".")
                {
                    lines.Add(line);
                }

                foreach (string responseLine in lines)
                {
                    newsgroups.Add(responseLine);
                }

                return newsgroups;
                
            }
            else
            {
                Console.WriteLine("LIST command failed.");
                
            }
        }
        else
        {
            Console.WriteLine("There is no connection to the server");
        }

        return null;
    }
    
    public static IList<string> GetArticles(string newsgroup)
    {
        Console.WriteLine("Getting articles from " + newsgroup);
        if (NntpResponseChecker.IsLoginSuccessful(loginResponse))
        {

            // Request list of articles
            Console.WriteLine("group " + newsgroup);
            writer.WriteLine("group " + newsgroup);
            response = reader.ReadLine();
            Console.WriteLine("Server: " + response);

            // Check if the server accepted the GROUP command
            if (response != null && response.StartsWith("211"))
            {
                // XOVER
                Console.WriteLine("xover");
                writer.WriteLine("xover");
                response = reader.ReadLine();
                Console.WriteLine("Server: " + response);

                if (response != null && response.StartsWith("224"))
                {

                    IList<string> lines = new List<string>();
                    IList<string> articles = new List<string>();

                    string? line;

                    while ((line = reader.ReadLine()) != null && line != ".")
                    {
                        lines.Add(line);
                    }

                    foreach (string responseLine in lines)
                    {
                        articles.Add(responseLine);
                    }

                    return articles;
                }
            }
            else
            {
                Console.WriteLine("GROUP command failed.");
                
            }
        }
        else
        {
            Console.WriteLine("There is no connection to the server");
        }

        return null;
    }
}