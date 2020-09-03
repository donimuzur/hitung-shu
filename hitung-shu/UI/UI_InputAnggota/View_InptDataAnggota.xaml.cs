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

namespace hitung_shu.UI.UI_InputAnggota
{
    /// <summary>
    /// Interaction logic for View_InptDataAnggota.xaml
    /// </summary>
    public partial class View_InptDataAnggota : UserControl
    {
        private IIptAnggotaServices _iptAnggotaServices;

        private Insert_InptDataAnggota _insertIptDataAnggota;
        private Edit_InptDataAnggota _editIptDataAnggota;

        private ICollectionView _data;
        Dictionary<string, Predicate<IptAnggotaDto>> filters = new Dictionary<string, Predicate<IptAnggotaDto>>();
        public View_InptDataAnggota()
        {
            InitializeComponent();

            _iptAnggotaServices = new IptAnggotaServices();
            _insertIptDataAnggota = new Insert_InptDataAnggota();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            Tahun.Text = DateTime.Today.Year.ToString();
            Init();
            BtnFilter_Click(sender, e);
        }
        private void Init()
        {
            #region --- DGV Main ---
            Dgv_Home.CanUserAddRows = false;
            Dgv_Home.CanUserDeleteRows = false;
            Dgv_Home.IsReadOnly = true;
            Dgv_Home.AutoGenerateColumns = false;

            DataGridTextColumn Id = new DataGridTextColumn();
            DataGridTextColumn NamaAnggota= new DataGridTextColumn();
            DataGridTextColumn Tanggal = new DataGridTextColumn();
            DataGridTextColumn Pokok = new DataGridTextColumn();
            DataGridTextColumn Wajib = new DataGridTextColumn();
            DataGridTextColumn Sukarela = new DataGridTextColumn();
            DataGridTextColumn Belanja = new DataGridTextColumn();
            DataGridTextColumn BungaPinjaman = new DataGridTextColumn();
            DataGridTextColumn DUMMY = new DataGridTextColumn();

            Binding TANGGALBinding = new Binding("Tanggal");
            TANGGALBinding.StringFormat = "dd MMM yyyy";

            Binding POKOKBinding = new Binding("Pokok");
            POKOKBinding.StringFormat = "{0:N0}";
            Binding WAJIBBinding = new Binding("Wajib");
            WAJIBBinding.StringFormat = "{0:N0}";
            Binding SUKARELABinding = new Binding("Sukarela");
            SUKARELABinding.StringFormat = "{0:N0}";
            Binding BELANJABinding = new Binding("Belanja");
            BELANJABinding.StringFormat = "{0:N0}";
            Binding BUNGAPINJAMANBinding = new Binding("BungaPinjaman");
            BUNGAPINJAMANBinding.StringFormat = "{0:N0}";

            Id.Binding = new Binding("Id");
            NamaAnggota.Binding = new Binding("NamaAnggota");
            Tanggal.Binding = TANGGALBinding;
            Pokok.Binding = POKOKBinding;
            Wajib.Binding = WAJIBBinding;
            Sukarela.Binding = SUKARELABinding;
            Belanja.Binding = BELANJABinding;
            BungaPinjaman.Binding = BUNGAPINJAMANBinding;

            Id.Header = "ID";
            NamaAnggota.Header = "Nama Anggota";
            Tanggal.Header = "Tanggal";
            Pokok.Header = "Pokok";
            Wajib.Header = "Wajib";
            Sukarela.Header = "Sukarela";
            Belanja.Header = "Belanja";
            BungaPinjaman.Header = "Bunga Pinjaman";

            Dgv_Home.Columns.Add(NamaAnggota);
            Dgv_Home.Columns.Add(Tanggal);
            Dgv_Home.Columns.Add(Pokok);
            Dgv_Home.Columns.Add(Wajib);
            Dgv_Home.Columns.Add(Sukarela);
            Dgv_Home.Columns.Add(Belanja);
            Dgv_Home.Columns.Add(BungaPinjaman);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(Id);
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;
            Id.Visibility = Visibility.Hidden;

            #endregion

            PopulateData();
        }
        private void PopulateData()
        {
            try
            {
                var Data = _iptAnggotaServices.GetAllByTahun(Convert.ToInt32(Tahun.Text)).OrderByDescending(x => x.Tanggal).ToList();
                if(!string.IsNullOrEmpty(Filter_IdAnggota.Text))
                {
                    Data = Data.Where(x => x.IdAnggota.ToString() == Filter_IdAnggota.Text).ToList();
                }
                
                _data = CollectionViewSource.GetDefaultView(Data);
                //_data.Filter = new Predicate<object>(FilterCandidates);

                Dgv_Home.ItemsSource = _data;
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _insertIptDataAnggota = new Insert_InptDataAnggota();
                var result = _insertIptDataAnggota.ShowDialog();
                if (result == true)
                {
                    PopulateData();
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var idx = (IptAnggotaDto)Dgv_Home.SelectedItem;
                if (idx != null)
                {
                    _editIptDataAnggota = new Edit_InptDataAnggota(idx.Id);
                    var result = _editIptDataAnggota.ShowDialog();
                    if (result == true)
                    {
                        PopulateData();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var idx = (IptAnggotaDto)Dgv_Home.SelectedItem;
                if (idx != null)
                {
                    var Result = System.Windows.MessageBox.Show("Apakah and ingin menghapus data ini ?", "Info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (Result == MessageBoxResult.Yes)
                    {
                        _iptAnggotaServices.Delete(idx.Id);

                        System.Windows.MessageBox.Show("Data berhasil dihapus", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        PopulateData();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dgv_Home_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var idx = (IptAnggotaDto)Dgv_Home.SelectedItem;
                if (idx != null)
                {
                    _editIptDataAnggota = new Edit_InptDataAnggota(idx.Id);
                    var result = _editIptDataAnggota.ShowDialog();
                    if (result == true)
                    {
                        PopulateData();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            PopulateData();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Tahun.Text = DateTime.Today.Year.ToString();
            Filter_IdAnggota.Text = "";
            PopulateData();
        }
    }
}
