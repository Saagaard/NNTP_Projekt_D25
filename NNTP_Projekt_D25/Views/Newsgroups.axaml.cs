using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NNTP_Projekt_D25.Services;

namespace NNTP_Projekt_D25.Views;

public partial class Newsgroups : UserControl
{
    public Newsgroups()
    {
        InitializeComponent();
        
        foreach (string group in NntpService.GetNewsgroups())
        {
            var label = new Label { Content = group };
            label.PointerPressed += (_, _) => Newsgroup_Button_Click(group);
            newsgroup_list.Items.Add(label);
            
        }
    }

    private void Newsgroup_Button_Click(string group)
    {
        //Console.WriteLine(group);
        Content = new ArticleList(group);
    }
}