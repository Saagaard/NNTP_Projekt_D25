using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace NNTP_Projekt_D25.Views;

public partial class Article : UserControl
{
    private string newsgroup;
    public Article(string article, string previousNewsgroup)
    {
        InitializeComponent();
        newsgroup = previousNewsgroup;
        var label = new Label { Content = article };
        article_stack.Children.Add(label);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Content = new ArticleList(newsgroup);
    }
}