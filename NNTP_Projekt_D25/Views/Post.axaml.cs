using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace NNTP_Projekt_D25.Views;

public partial class Post : UserControl
{
    private string newsgroup;
    public Post(string previousNewsgroup)
    {
        InitializeComponent();
        newsgroup = previousNewsgroup;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Go_back(object? sender, RoutedEventArgs e)
    {
        Content = new ArticleList(newsgroup);
    }
}