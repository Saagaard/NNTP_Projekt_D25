using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using NNTP_Projekt_D25.Services;


namespace NNTP_Projekt_D25.Views;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }

    private void Send_UserPass(object? sender, RoutedEventArgs e)
    {
        
        NntpService.Create_Connection(box_user.Text, box_pass.Text);
        if (NntpResponseChecker.IsGreetingSuccessful(NntpService.greetingResponse))
        {
            Content = new Newsgroups();
        }
        

    }

    
}