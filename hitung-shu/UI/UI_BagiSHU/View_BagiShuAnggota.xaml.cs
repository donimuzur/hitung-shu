using polowijo.gosari.dal;
using polowijo.gosari.dto;
using polowijo.gosari.helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Xceed.Wpf.Toolkit;

namespace hitung_shu.UI.UI_BagiSHU
{
    /// <summary>
    /// Interaction logic for View_BagiShuAnggota.xaml
    /// </summary>
    public partial class View_BagiShuAnggota : UserControl
    {
        private IDataKoperasiServices _dataKoperasiServices;
        private IBagiShuServices _bagiShuServices;

        private ICollectionView _data;
        private Window_BagiShuAnggota _windowBagiShuAnggota;

        public View_BagiShuAnggota()
        {
            InitializeComponent();
            _dataKoperasiServices = new DataKoperasiServices();
            _bagiShuServices = new BagiShuServices();

            _windowBagiShuAnggota = new Window_BagiShuAnggota(new BagiShuDto());
        }
        private void ButtonSpinner_Spin(object sender, SpinEventArgs e)
        {
            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtBox = (TextBox)spinner.Content;

            int value = String.IsNullOrEmpty(txtBox.Text) ? 0 : Convert.ToInt32(txtBox.Text);
            if (e.Direction == SpinDirection.Increase)
                value++;
            else
                if (value > 0)
                value--;
            txtBox.Text = value.ToString();
        }
        private void HitungShu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Tahun.Text) && !string.IsNullOrEmpty(IdAnggota.Text))
                {
                    int intTahun = Convert.ToInt32(Tahun.Text);
                    var Data = _bagiShuServices.HitungShuAnggota(intTahun, IdAnggota.Text);
                    
                    _windowBagiShuAnggota = new Window_BagiShuAnggota(Data);
                    _windowBagiShuAnggota.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    _windowBagiShuAnggota.Show();
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Tahun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Cek_Click(sender, e);
            }
        }

        private void Cek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HitungShu.IsEnabled = false;
                IdAnggota.IsEnabled = false;
                if (!string.IsNullOrEmpty(Tahun.Text))
                {
                    var intTahun = Convert.ToInt32(Tahun.Text);
                    var GetDataShu = _dataKoperasiServices.GetByTahun(intTahun);

                    if (GetDataShu == null)
                    {
                        return;
                    }
                    total_shu.Text = String.Format("{0:N}", GetDataShu.TotalShu);
                    total_simpanan.Text = String.Format("{0:N}", GetDataShu.TotalSimpanan);
                    total_pinjaman.Text = String.Format("{0:N}", GetDataShu.TotalPinjaman);
                    total_belanja.Text = String.Format("{0:N}", GetDataShu.TotalPenjualan);

                    double doubleDanaCadangan = 0;
                    doubleDanaCadangan = (double)GetDataShu.TotalShu * 0.3;
                    DanaCadangan.Text = String.Format("{0:N}", doubleDanaCadangan);

                    double doubleJasaModal = 0;
                    doubleJasaModal = (double)GetDataShu.TotalShu * 0.2;
                    JasaModal.Text = String.Format("{0:N}", doubleJasaModal);

                    double doubleJasaAnggota = 0;
                    doubleJasaAnggota = (double)GetDataShu.TotalShu * 0.1;
                    JasaAnggota.Text = String.Format("{0:N}", doubleJasaAnggota);

                    double doubleDanaPengurusPgw = 0;
                    doubleDanaPengurusPgw = (double)GetDataShu.TotalShu * 0.1;
                    PengurusPengawas.Text = String.Format("{0:N}", doubleDanaPengurusPgw);

                    double doubleKesejahteraanPGW = 0;
                    doubleKesejahteraanPGW = (double)GetDataShu.TotalShu * 0.1;
                    KesejahteraanPegawai.Text = String.Format("{0:N}", doubleKesejahteraanPGW);

                    double doubleDanaPendidikan = 0;
                    doubleDanaPendidikan = (double)GetDataShu.TotalShu * 0.1;
                    Pendidikan.Text = String.Format("{0:N}", doubleDanaPendidikan);

                    double doubleDanaSosial = 0;
                    doubleDanaSosial = (double)GetDataShu.TotalShu * 0.1;
                    DanaSosial.Text = String.Format("{0:N}", doubleDanaSosial);

                    var Data = _bagiShuServices.GetAll();

                    _data = CollectionViewSource.GetDefaultView(Data);

                    HitungShu.IsEnabled = true;
                    IdAnggota.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

