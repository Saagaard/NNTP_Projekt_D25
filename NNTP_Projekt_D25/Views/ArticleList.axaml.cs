using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NNTP_Projekt_D25.Services;

namespace NNTP_Projekt_D25.Views;

public partial class ArticleList : UserControl
{
    string? currentNewsgroup;
    public ArticleList(string newsgroup)
    {
        InitializeComponent();
        currentNewsgroup = newsgroup;
        
        foreach (string group in NntpService.GetArticles(newsgroup))
        {
            var label = new Label { Content = group };
            label.PointerPressed += (_, _) => Article_Button_Click(group);
            article_list.Items.Add(label);
            
        }
    }
    
    private void Article_Button_Click(string article)
    {
        //Console.WriteLine(article);
        string article_id = article.Split(null)[0];
        Console.WriteLine(article_id);
        Content = new Article(NntpService.GetArticle(article_id),  currentNewsgroup);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Content = new Newsgroups();
    }

    private void Create_Post(object? sender, RoutedEventArgs e)
    {
        Content = new Post(currentNewsgroup);
    }
}