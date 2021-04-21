using System;
using Xceed.Wpf.Toolkit;
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
using polowijo.gosari.dal;
using polowijo.gosari.dto;
using polowijo.gosari.helpers;

namespace hitung_shu.UI.UI_DataShu
{
    /// <summary>
    /// Interaction logic for View_Main.xaml
    /// </summary>
    public partial class View_Main : UserControl
    {
        private IDataKoperasiServices _dataKoperasiServices;
        public View_Main()
        {
            InitializeComponent();

            _dataKoperasiServices = new DataKoperasiServices();
            Init();
        }
        public void Init()
        {
            Tahun.Text = DateTime.Today.Year.ToString();

            var data = _dataKoperasiServices.GetByTahun(Convert.ToInt32(Tahun.Text));

            Id.Text = "0";
            total_shu.Text = "";
            total_pokok.Text = "";
            total_simpanan.Text = "";
            total_belanja.Text = "";
            total_pinjaman.Text = "";
            total_sukarela.Text = "";
            total_wajib.Text = "";
            CreatedBy.Text = "Admin";
            CreatedDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (data != null)
            {
                PopulateData(data);
            }
        }
        public void PopulateData(DataKoperasiDto data)
        {
            Id.Text = data.Id.ToString();
            total_shu.Text = data.TotalShu.ToString();
            total_simpanan.Text = data.TotalSimpanan.ToString();
            total_belanja.Text = data.TotalPenjualan.ToString();
            total_pinjaman.Text = data.TotalPinjaman.ToString();
            CreatedBy.Text = data.CreatedBy;
            CreatedDate.Text = data.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            ModifiedBy.Text = data.ModifiedBy;
            ModifiedDate.Text = data.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private void ButtonSpinner_Spin(object sender, SpinEventArgs e)
        {
            ButtonSpinner spinner = (ButtonSpinner)sender;
            TextBox txtBox = (TextBox)spinner.Content;

            int value = String.IsNullOrEmpty(txtBox.Text) ? 0 : Convert.ToInt32(txtBox.Text);
            if (e.Direction == SpinDirection.Increase )
                value++;
            else
                if (value > 0)
                    value--;
            txtBox.Text = value.ToString();
            Cek_Click(sender, e);
        }
        private void Cek_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Tahun.Text))
            {
                System.Windows.MessageBox.Show("Tahun tidak boleh kosong","Info",MessageBoxButton.OK,MessageBoxImage.Information);
                return ;
            }
            var data = _dataKoperasiServices.GetByTahun(Convert.ToInt32(Tahun.Text));

            Id.Text = "0";
            total_shu.Text ="0";
            total_wajib.Text = "0";
            total_sukarela.Text = "0";
            total_pokok.Text = "0";
            total_simpanan.Text = "0";
            total_belanja.Text = "0";
            total_pinjaman.Text = "0";
            CreatedBy.Text = "0";
            CreatedDate.Text = "0";
            ModifiedBy.Text = "0";
            ModifiedDate.Text = "0";

            if (data != null)
            {
                Id.Text = data.Id.ToString();
                total_shu.Text = data.TotalShu.ToString();
                total_simpanan.Text = data.TotalSimpanan.ToString();
                total_belanja.Text = data.TotalPenjualan.ToString();
                total_pinjaman.Text = data.TotalPinjaman.ToString();
                CreatedBy.Text = data.CreatedBy;
                CreatedDate.Text = data.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                ModifiedBy.Text = data.ModifiedBy;
                ModifiedDate.Text = data.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
                return;
            }

            System.Windows.MessageBox.Show("Tidak ada data pada tahun tersebut", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Dto = new DataKoperasiDto();

                if(string.IsNullOrEmpty(total_shu.Text))
                {
                    System.Windows.MessageBox.Show("Total SHU tidak boleh kosong", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrEmpty(total_belanja.Text))
                {
                    total_belanja.Text = "0";
                }
                
                if (string.IsNullOrEmpty(total_simpanan.Text))
                {
                    total_simpanan.Text = "0";
                }

                if (string.IsNullOrEmpty(total_pokok.Text))
                {
                    total_pokok.Text = "0";
                }

                if (string.IsNullOrEmpty(total_wajib.Text))
                {
                    total_wajib.Text = "0";
                }

                if (string.IsNullOrEmpty(total_sukarela.Text))
                {
                    total_sukarela.Text = "0";
                }

                if (string.IsNullOrEmpty(total_pinjaman.Text))
                {
                    total_pinjaman.Text = "0";
                }

               

                if(!string.IsNullOrEmpty(Id.Text))
                    Dto.Id = Convert.ToInt32(Id.Text);

                Dto.Tahun = Convert.ToInt32(Tahun.Text);
                Dto.TotalShu = Convert.ToDouble(total_shu.Text);
                Dto.TotalPenjualan = Convert.ToDouble(total_belanja.Text);
                Dto.TotalPinjaman = Convert.ToDouble(total_pinjaman.Text);
                Dto.TotalSimpanan = Convert.ToDouble(total_simpanan.Text);

                _dataKoperasiServices.InserOrUpdate(Dto);
                PopulateData(Dto);
                System.Windows.MessageBox.Show("Data sudah tersimpan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Tahun_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key ==Key.Enter)
            {
                Cek_Click(sender, e);
            }
        }

        private void total_shu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void total_pokok_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void total_sukarela_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void total_simpanan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void total_belanja_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void total_pinjaman_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void total_wajib_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelpers.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void total_wajib_TextChanged(object sender, TextChangedEventArgs e)
        {
            TotalSimpanan(total_pokok.Text, total_wajib.Text, total_sukarela.Text);
        }

        private void total_sukarela_TextChanged(object sender, TextChangedEventArgs e)
        {
            TotalSimpanan(total_pokok.Text, total_wajib.Text, total_sukarela.Text);
        }

        private void total_pokok_TextChanged(object sender, TextChangedEventArgs e)
        {
            TotalSimpanan(total_pokok.Text, total_wajib.Text, total_sukarela.Text);
        }
        private void TotalSimpanan(string TotalPokok, string TotalWajib, string TotalSukarela)
        {
            int IntTotalPokok = 0;
            int IntTotalWajib = 0;
            int IntTotalSukarela = 0;
            int IntTotalSimpanan = 0;
            if (!string.IsNullOrEmpty(TotalPokok))
            {
                IntTotalPokok = Convert.ToInt32(total_pokok.Text);
            }
            if (!string.IsNullOrEmpty(TotalWajib))
            {
                IntTotalWajib = Convert.ToInt32(total_wajib.Text);
            }
            if (!string.IsNullOrEmpty(TotalSukarela))
            {
                IntTotalSukarela = Convert.ToInt32(total_sukarela.Text);
            }
            IntTotalSimpanan = IntTotalPokok + IntTotalSukarela + IntTotalWajib;
            total_simpanan.Text = Convert.ToString(IntTotalSimpanan);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var parent = (DockPanel)this.Parent;
            parent.Children.RemoveAt(0);
        }
    }
}
