using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace demo3;

public partial class AddOrEditWindow : Window
{
    public ProductPresenter Product { get; set; }
    public bool IsEditing { get; set; }
    
    
    public AddOrEditWindow()
    {
        InitializeComponent();
    }
    
    public AddOrEditWindow( ProductPresenter? product, bool isEditing)
    {
        InitializeComponent();
        Product = product;
        IsEditing = isEditing;
        NameTextBox.Text = product.Title;
        CostTextBox.Text = product.Cost.ToString();
        DescriptionTextBox.Text = product.Description;
        IsActiveCheckBox.IsChecked = (product.Isactive == 1) ? true : false;
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        Product.Title = NameTextBox.Text;
        if (float.TryParse(CostTextBox.Text, out float cost))
        {
            Product.Cost = cost;
        }
        Product.Description = DescriptionTextBox.Text;
        Product.Isactive = IsActiveCheckBox.IsChecked == true ? 1 : 2;
        this.Close();
    }

    private void SelectImageButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ActivitiCheckkBox(object? sender, RoutedEventArgs e)
    {
        Product.Isactive = (IsActiveCheckBox.IsChecked == true) ? 1 : 2;
    }

    private void GoToMainWindow(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}