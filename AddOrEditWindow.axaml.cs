using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace demo3;

public partial class AddOrEditWindow : Window
{
    public ProductPresenter Product = new ProductPresenter();
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
        PhotoProduct.Source = product.MainPhoto;
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
        Product.Mainimagepath = Product.Mainimagepath;
        this.Close(Product);
    }

    private async void SelectImageButton_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Choose Product Image",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Image Files", Extensions = { "png", "jpg", "jpeg" } }
            }
        };
        string[] result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            Product.ChangePhoto = new Bitmap(result[0]);
        }
    }

    private void ActivitiCheckkBox(object? sender, RoutedEventArgs e)
    {
        Product.Isactive = (IsActiveCheckBox.IsChecked == true) ? 1 : 2;
    }

    private void GoToMainWindow(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}