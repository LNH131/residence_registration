﻿using Resident.ViewModels; // or wherever your VM is
using System.Windows;

namespace Resident.View
{
    public partial class HouseholdSeparationDetailsWindow : Window
    {
        public HouseholdSeparationDetailsWindow(HouseholdSeparationDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
