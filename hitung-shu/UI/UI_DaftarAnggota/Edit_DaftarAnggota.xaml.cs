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
    /// Interaction logic for Edit_DaftarAnggota.xaml
    /// </summary>
    public partial class Edit_DaftarAnggota : Window
    {
        private IDataAnggotaServices _dataAnggotaServices;
        private string _idAngota;
        public Edit_DaftarAnggota(string IdAngota)
        {
            InitializeComponent();
            _dataAnggotaServices = new DataAnggotaServices();

            _idAngota = IdAngota;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var Dto = _dataAnggotaServices.GetByIdAnggota(_idAngota);
                Id.Text = Dto.Id.ToString();
                CreatedBy.Text = Dto.CreatedBy;
                CreatedDate.Text = Dto.CreatedDate.ToString();
                ModifiedBy.Text = Dto.ModifiedBy;
                ModifiedDate.Text = Dto.ModifiedDate.ToString();
                IdAnggota.Text = Dto.IdAnggota;
                TanggalGabung.Text = Dto.TanggalGabung.ToString();
                NamaAnggota.Text = Dto.NamaAnggota;
                Status.IsChecked = Dto.Status;               
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(IdAnggota.Text) || IdAnggota.Text == "0")
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
                Dto.Id = Convert.ToInt64(Id.Text);
                var ExistID = _dataAnggotaServices.GetByIdAnggota(IdAnggota.Text);
                if(ExistID != null)
                {
                    System.Windows.MessageBox.Show("Id Anggota sudah ada", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Dto.CreatedBy = CreatedBy.Text;
                Dto.CreatedDate = Convert.ToDateTime(CreatedDate.Text);
                Dto.ModifiedBy = ModifiedBy.Text;
                Dto.ModifiedDate = Convert.ToDateTime(ModifiedDate.Text);

                Dto.IdAnggota = Convert.ToString(IdAnggota.Text);
                Dto.NamaAnggota = NamaAnggota.Text;
                Dto.TanggalGabung = Convert.ToDateTime(TanggalGabung.Text);
                Dto.Status = Status.IsChecked.HasValue ? Status.IsChecked.Value : false;

                Dto = _dataAnggotaServices.InserOrUpdate(Dto);
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
        private void CloseWin()
        {
            DialogResult = true;
            this.Close();
        }
    }
}
