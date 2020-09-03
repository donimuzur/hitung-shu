using polowijo.gosari.dal;
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

namespace hitung_shu.UI.UI_DaftarAnggota
{
    /// <summary>
    /// Interaction logic for Insert_DaftarAnggota.xaml
    /// </summary>
    public partial class Insert_DaftarAnggota : Window
    {
        private IDataAnggotaServices _dataAnggotaServices;
        public Insert_DaftarAnggota()
        {
            InitializeComponent();

            _dataAnggotaServices = new DataAnggotaServices();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TanggalGabung.Text = DateTime.Today.ToString();
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
                if(string.IsNullOrEmpty(IdAnggota.Text) || IdAnggota.Text == "0")
                {
                    System.Windows.MessageBox.Show("Id Anggota tidak boleh kosong", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (string.IsNullOrEmpty(NamaAnggota.Text))
                {
                    System.Windows.MessageBox.Show("Nama Anggota tidak boleh kosong", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (string.IsNullOrEmpty(TanggalGabung.Text))
                {
                    System.Windows.MessageBox.Show("Tanggal Gabung tidak boleh kosong", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                DataAnggotaDto Dto = new DataAnggotaDto();
                Dto.Id = 0;
                Dto.IdAnggota = Convert.ToString(IdAnggota.Text);
                Dto.NamaAnggota = NamaAnggota.Text;
                Dto.TanggalGabung = Convert.ToDateTime(TanggalGabung.Text);
                Dto.Status = Status.IsChecked.HasValue? Status.IsChecked.Value:false;

                var CheckAnggota = CheckAnggotaExist(Dto.IdAnggota);
                if (CheckAnggota != null)
                {
                    System.Windows.MessageBox.Show("ID tersebut sudah terdaftar", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Dto = _dataAnggotaServices.Insert(Dto);
                System.Windows.MessageBox.Show("Data sudah tersimpan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWin();

            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnTutup_Click(object sender, RoutedEventArgs e)
        {
            CloseWin();
        }
    }
}
