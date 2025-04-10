using HulluKyla.Services;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;

namespace HulluKyla.Pages;

public partial class TestiSivu : ContentPage
{
	public TestiSivu()
	{
		InitializeComponent();
		PaivitaLista();
	}

	void PaivitaLista() 
	{
		AsiakasLista.ItemsSource = null;
		AsiakasLista.ItemsSource = AsiakasService.HaeKaikki();
	}
}