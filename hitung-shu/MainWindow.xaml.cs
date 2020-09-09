﻿using hitung_shu.UI.UI_BagiSHU;
using hitung_shu.UI.UI_DaftarAnggota;
using hitung_shu.UI.UI_DataShu;
using hitung_shu.UI.UI_InputAnggota;
using polowijo.gosari.dal;
using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hitung_shu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBagiShuServices _bagiShuServices;
        private IDataKoperasiServices _dataKoperasiServices;

        private View_Main _dataShuView;
        private View_InptDataAnggota _iptAnggotaView;
        private View_BagiShu _bagiShuView;
        private View_BagiShuAnggota _bagiShuAnggotaView;
        private View_DaftarAnggota _daftarAnggotaView;
        public MainWindow()
        { 
            InitializeComponent();
            _bagiShuServices = new BagiShuServices();
            _dataKoperasiServices = new DataKoperasiServices();

            _dataShuView = new View_Main();
            _iptAnggotaView = new View_InptDataAnggota();
            _bagiShuView = new View_BagiShu();
            _bagiShuAnggotaView = new View_BagiShuAnggota();
            _daftarAnggotaView = new View_DaftarAnggota();
        }

        private void DataShu_Click(object sender, RoutedEventArgs e)
        {
            _dataShuView = new View_Main();
            MainView.Children.Clear();
            MainView.Children.Add(_dataShuView);
        }

        private void DataAnggota_Click(object sender, RoutedEventArgs e)
        {
            _iptAnggotaView = new View_InptDataAnggota();
            MainView.Children.Clear();
            MainView.Children.Add(_iptAnggotaView);
        }
        private void BagiShu_Click(object sender, RoutedEventArgs e)
        {
            _bagiShuView = new View_BagiShu();
            MainView.Children.Clear();
            MainView.Children.Add(_bagiShuView);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DaftarAnggota_Click(object sender, RoutedEventArgs e)
        {
            _daftarAnggotaView = new View_DaftarAnggota();
            MainView.Children.Clear();
            MainView.Children.Add(_daftarAnggotaView);
        }

        private void DataAnggota_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void BagiShuAnggota_Click(object sender, RoutedEventArgs e)
        {
            _bagiShuAnggotaView = new View_BagiShuAnggota();
            MainView.Children.Clear();
            MainView.Children.Add(_bagiShuAnggotaView);
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
