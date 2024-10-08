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
            if (ChangePhoto != null)
            {
                return ChangePhoto;
            }
            if (!string.IsNullOrEmpty(this.Mainimagepath))
            {
                return ImageHelper.Load(new Uri($"avares://demo3/Assets/{this.Mainimagepath}"));
            }
            return null; 
        }
    }

    public Bitmap? ChangePhoto { get; set; } = null;
    
    public string ProductTitle {get=>this.Title;}
    public float ProductCost {get=>this.Cost; }
    public string IsActive { get => this.Isactive == 1 ? "Активен" : "Неактивен"; }
    public IBrush? ProductColor {  get => IsActive=="Активен" ? null : Brushes.LightGray; }
    public List<string>? ProductManufacturer { get; set; }
}
public partial class MainWindow : Window
{
    private List<ProductPresenter> _currentPageProducts { get; set; }
    private int[] _itemsPerPage = new[] { 10, 50, 100,200};
    private string ItemsPerPage;
    private int pages;
    private int currentPage = 1;
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
        ProductsPerPageComboBox.ItemsSource = _itemsPerPage;
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
        if (!string.IsNullOrEmpty(ItemsPerPage))
        {
            int IntItemsPerPage =  int.Parse(ItemsPerPage);
            pages = displayProductList.Count / IntItemsPerPage;
            displayProductList = displayProductList.Skip((currentPage-1)*IntItemsPerPage).Take(IntItemsPerPage).ToList();
            
        }

        if (_displayProducts.Count!=0 && _displayProducts !=null)
        {
            _displayProducts.Clear();
        }
        foreach(var product in displayProductList){_displayProducts.Add(product);}
        StaticticTextBlock.Text = string.Format("Показано {0} из {1}", _displayProducts.Count, _products.Count);
        PagesTextBlock.Text = string.Format("Страница: {0}",currentPage );
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

    private async void GoToAddWindow(object? sender, RoutedEventArgs e)
    {
            var addWindow = new AddOrEditWindow();
            Windows.Add(addWindow);
            if (Windows.Count >= 2)
            {
                addWindow.Close();
            }
            else
            {
                var result = await addWindow.ShowDialog<ProductPresenter>(this);
                if (result != null)
                {
                    _products.Add(result);
                    DisplayProducts();
                }

                Windows.Remove(addWindow);
            }

    }

    private async void EditProduct_Click(object? sender, RoutedEventArgs e)
    {
        var editWindow = new AddOrEditWindow(_selectedProduct, true);
        Windows.Add(editWindow);
        if (Windows.Count >= 2)
        {
            editWindow.Close();
        }
        else
        {
            var result = await editWindow.ShowDialog<ProductPresenter>(this);
            if (result != null)
            {
                _selectedProduct.Title = result.Title;
                _selectedProduct.Cost = result.Cost;
                _selectedProduct.Description = result.Description;
                _selectedProduct.Isactive = result.Isactive;
                DisplayProducts();
            }

            Windows.Remove(editWindow);
        }
    }

    private void DeleteProduct_Click(object? sender, RoutedEventArgs e)
    {
        if (_selectedProduct != null)
        {
            _products.Remove(_selectedProduct);
            DisplayProducts();
        }
    }

    private void ProductsListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedProduct = (ProductPresenter?)ProductsListBox.SelectedItem;
        EditButton.IsVisible = true;
        DeleteButton.IsVisible = true;
    }

    private void SelectionProductsPerPage(object? sender, SelectionChangedEventArgs e)
    {
        ItemsPerPage = (sender as ComboBox).SelectedItem.ToString();
        DisplayProducts();
    }

    private void PreviousPage(object? sender, RoutedEventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            DisplayProducts();
        }
        
    }

    private void NextPage(object? sender, RoutedEventArgs e)
    {
        if (currentPage < pages)
        {
            currentPage++;
            DisplayProducts();
        }
    }
}


