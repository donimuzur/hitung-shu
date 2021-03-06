﻿using polowijo.gosari.dal;
using polowijo.gosari.dto;
using polowijo.gosari.helpers;
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
using System.Windows.Shapes;

namespace hitung_shu.UI.UI_InputAnggota
{
    /// <summary>
    /// Interaction logic for Insert_InptDataAnggota.xaml
    /// </summary>
    public partial class Insert_InptDataAnggota : Window
    {
        private IIptAnggotaServices _iptAnggotaServices;
        private IDataAnggotaServices _dataAnggotaServices;
        public Insert_InptDataAnggota()
        {
            InitializeComponent();
            _iptAnggotaServices = new IptAnggotaServices();
            _dataAnggotaServices = new DataAnggotaServices();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();
            IdAnggota.Focus();
        }
        public void init()
        {
            Id.Text = "0";
            Tanggal.Text = DateTime.Today.ToString();
            CreatedBy.Text = "Admin";
            CreatedDate.Text = DateTime.Now.ToString();
        }
        private void CloseWin()
        {
            DialogResult = true;
            this.Close();
        }
        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Dto = new IptAnggotaDto();

                if (string.IsNullOrEmpty(IdAnggota.Text))
                {
                    System.Windows.MessageBox.Show("Id Anggota tidak boleh kosong", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if(string.IsNullOrEmpty(Tanggal.Text))
                {
                    System.Windows.MessageBox.Show("Tanggal tidak boleh kosong", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if(string.IsNullOrEmpty(Pokok.Text))
                {
                    Pokok.Text = "0";
                }

                if(string.IsNullOrEmpty(Wajib.Text))
                {
                    Wajib.Text = "0";
                }

                if(string.IsNullOrEmpty(Sukarela.Text))
                {
                    Sukarela.Text = "0";
                }

                if(string.IsNullOrEmpty(Belanja.Text))
                {
                    Belanja.Text = "0";
                }

                if(string.IsNullOrEmpty(BungaPinjaman.Text))
                {
                    BungaPinjaman.Text = "0";
                }

                Dto.Id = 0;                

                Dto.IdAnggota =IdAnggota.Text;
                var CheckAnggota = CheckAnggotaExist(Dto.IdAnggota);
                if (CheckAnggota == null)
                {
                    System.Windows.MessageBox.Show("ID tersebut tidak terdaftar", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Dto.NamaAnggota = CheckAnggota.NamaAnggota;
                Dto.Tanggal = Convert.ToDateTime(Tanggal.Text);
                Dto.Pokok = Convert.ToDouble(Pokok.Text);
                Dto.Wajib = Convert.ToDouble(Wajib.Text);
                Dto.Sukarela = Convert.ToDouble(Sukarela.Text);
                Dto.Belanja = Convert.ToDouble(Belanja.Text);
                Dto.BungaPinjaman = Convert.ToDouble(BungaPinjaman.Text);
                Dto = _iptAnggotaServices.InserOrUpdate(Dto);
                System.Windows.MessageBox.Show("Data sudah tersimpan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWin();
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnTutup_Click(object sender, RoutedEventArgs e)
        {
            CloseWin();
        }
        private void IdAnggota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                if (String.IsNullOrEmpty(IdAnggota.Text))
                {
                    return;
                }

                var Id = IdAnggota.Text;
                var CheckAnggota = CheckAnggotaExist(Id);
                if (CheckAnggota == null)
                {
                    System.Windows.MessageBox.Show("ID tersebut tidak terdaftar", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                NamaAnggota.Text = CheckAnggota.NamaAnggota;
            }
        }
        private DataAnggotaDto CheckAnggotaExist(string IdAnggota)
        {
            try
            {
                DataAnggotaDto result = null;
                var CheckAnggota = _dataAnggotaServices.GetByIdAnggota(IdAnggota);
                if (CheckAnggota != null)
                {
                    return CheckAnggota;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Wajib_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void Pokok_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void Sukarela_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void Belanja_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void BungaPinjaman_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
    }
}
