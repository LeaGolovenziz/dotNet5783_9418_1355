﻿using BO;
using PL.OrderWindows;
using PL.ProductWindows;
using System;
using System.IO;
using System.Windows;

namespace PL
{

    /// <summary>
    /// Interaction logic for BuyerWindow.xaml
    /// </summary>

    public partial class BuyerWindow : Window
    {
        User user;
        public BuyerWindow(User user)
        {
            InitializeComponent();

            this.user = user;   

            // try to upload the openning video if exists
            try
            {
                Uri resourceUri = new Uri(Directory.GetCurrentDirectory().Replace("bin", "PL\\images\\openningVideo.mp4"), UriKind.Absolute); ;
                video.Source = resourceUri;
            }
            // incase there is no image
            catch (Exception ex) { }
        }

        private void MenuItem_OpenCatalog(object sender, RoutedEventArgs e) => new CatalogWindow(user).Show();

        private void MenuItem_TrackOrder(object sender, RoutedEventArgs e) => new TrackingWindow(user).Show();

    }
}
