﻿using MyApp.MVVM.ViewModels;

namespace MyApp.MVVM.Views;

public partial class SearchView : ContentPage
{
	public SearchView()
	{
		InitializeComponent();
		BindingContext = new SearchViewModel();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        
    }
}