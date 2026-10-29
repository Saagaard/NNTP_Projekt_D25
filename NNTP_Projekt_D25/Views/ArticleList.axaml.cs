using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NNTP_Projekt_D25.Services;

namespace NNTP_Projekt_D25.Views;

public partial class ArticleList : UserControl
{
    public ArticleList(string newsgroup)
    {
        InitializeComponent();
        
        foreach (string group in NntpService.GetArticles(newsgroup))
        {
            var label = new Label { Content = group };
            label.PointerPressed += (_, _) => Article_Button_Click(group);
            article_list.Items.Add(label);
            
        }
    }
    
    private void Article_Button_Click(string group)
    {
        Console.WriteLine(group);
        //Content = new ArticleList(group);
    }
}