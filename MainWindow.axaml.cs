using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using demo3.EntityModels;
using demo3.Helpers;

namespace demo3;
public class ProductPresenter : Product
{
    public Bitmap? MainPhoto 
    {
        get 
        {
            if (!string.IsNullOrEmpty(this.Mainimagepath))
            {
                return ImageHelper.Load(new Uri($"avares://demo3/Assets/{this.Mainimagepath}"));
            }
            return null; 
        }
    }

    
    public string ProductTitle {get=>this.Title;}
    public float ProductCost {get=>this.Cost; }
    public string IsActive { get => this.Isactive == 1 ? "Активен" : "Неактивен"; }
    public IBrush? ProductColor {  get => IsActive=="Активен" ? null : Brushes.LightGray; }
    public List<string>? ProductManufacturer { get; set; }
}
public partial class MainWindow : Window
{
    private List<Window> Windows = new List<Window>();
    private string _searchWord;
    private string filtredItem;
    private int sortIndex;
    private ProductPresenter? _selectedProduct;

    private string[] sortByPrice = 
    {
        "Все", "По возраст.","По убыв."
    };
    public List<ProductPresenter> _products {get;set;}
    public ObservableCollection<ProductPresenter>_displayProducts { get; set; } 
    
    public List<string> Manufacturer { get; set; }


    public MainWindow()
    {
        
        InitializeComponent();
    
        using (var dbContext = new ShaluhinContext())
        {
            _products = dbContext.Products.Select(product => new ProductPresenter
            {
                Title = product.Title,
                Mainimagepath = product.Mainimagepath,
                Cost = product.Cost,
                Isactive = product.Isactive,
                ProductManufacturer = new List<string> { product.Manufacturer.Name }
            }).ToList();
        }
        
        Manufacturer = new List<string> { "Все" }
            .Concat(_products.SelectMany(p => p.ProductManufacturer).Distinct())
            .ToList();
        
        _displayProducts = new ObservableCollection<ProductPresenter>(_products);
        
        FilterManufactureComboBox.ItemsSource = Manufacturer;
        SortByPriceCombobox.ItemsSource = sortByPrice;   
        ProductsListBox.ItemsSource = _displayProducts;
    }

    private void DisplayProducts()
    {
        var displayProductList = _products;
        if (!string.IsNullOrEmpty(_searchWord))
        {
            displayProductList = displayProductList.Where(product => product.Title.ToLower().Contains(_searchWord)||
                                                                     (product.Description != null && product.Description.ToLower().Contains(_searchWord))).ToList();
        }
        if (!string.IsNullOrEmpty(filtredItem) && filtredItem != "Все")
        {
            displayProductList = displayProductList
                .Where(product => product.ProductManufacturer.Contains(filtredItem))
                .ToList();
        }
        switch (sortIndex)
        {
            case 0:
                break;
            case 1: 
                displayProductList = displayProductList.OrderBy(product => product.ProductCost).ToList();
                break;
            case 2: 
                displayProductList = displayProductList.OrderByDescending(product => product.ProductCost).ToList();
                break;
        }

        if (_displayProducts.Count!=0 && _displayProducts !=null)
        {
            _displayProducts.Clear();
        }
        foreach(var product in displayProductList){_displayProducts.Add(product);}
        StaticticTextBlock.Text = string.Format("Показано {0} из {1}", _displayProducts.Count, _products.Count);
    }

    private void SearchProducts(object? sender, TextChangedEventArgs e)
    {
        _searchWord = (sender as TextBox).Text.ToLower();
        DisplayProducts();
    }

    private void SelectedFilterCombobox(object? sender, SelectionChangedEventArgs e)
    {
        filtredItem = (sender as ComboBox).SelectedItem.ToString();
        DisplayProducts();
    }

    private void SelectedSortedComboBox(object? sender, SelectionChangedEventArgs e)
    {
        sortIndex = SortByPriceCombobox.SelectedIndex;
        DisplayProducts();
    }

    private void GoToAddWindow(object? sender, RoutedEventArgs e)
    {
        var addWindow = new AddOrEditWindow();
        
        Windows.Add(addWindow);
        if (Windows.Count >= 2)
        {
            addWindow.Close();
        }
        else
        {
            addWindow.Closed += (s, args) =>
            {
                Windows.Remove(addWindow);
                _products.Add(addWindow.Product);
                _displayProducts.Add(addWindow.Product);
                DisplayProducts();
            };
            addWindow.Show();
        }
        
    }

    private void EditProduct_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedProduct != null)
        {
            var editWindow = new AddOrEditWindow(_selectedProduct, true);
            Windows.Add(editWindow);
            if (Windows.Count >= 2)
            {
                editWindow.Close();
            }
            else
            {
                editWindow.Closed += (s, args) =>
                {
                    Windows.Remove(editWindow);
                    ProductsListBox.ItemsSource = _products;
                };
                editWindow.Show();
            }
            
        }
    }

    private void DeleteProduct_Click(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ProductsListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedProduct = (ProductPresenter?)ProductsListBox.SelectedItem;
        EditButton.IsVisible = true;
        DeleteButton.IsVisible = true;
    }
}


