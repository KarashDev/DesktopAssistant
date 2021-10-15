using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using EverythingNet;
//using EverythingNet.Core;
//using EverythingNet.Interfaces;

namespace DesktopAssistant
{
	/// <summary>
	/// Interaction logic for SearchEverythingWindow.xaml
	/// </summary>
	public partial class SearchEverythingWindow : Window
	{
		public SearchEverythingWindow()
		{
			InitializeComponent();
		}

		private void button_Search_Click(object sender, RoutedEventArgs e)
		{
			//// Попытки подключить библиотеку поисковика Everything
			//IEverything everything = new Everything();

			//INameQueryable results = everything.Search().Name.Contains(textBox_Search.Text);

			//List<string> list = new List<string>();
			//list.Add(Convert.ToString(results));
			////foreach (var result in results)
			////{
			////	list.Add(Convert.ToString(result));
			////}

		}
	}
}
