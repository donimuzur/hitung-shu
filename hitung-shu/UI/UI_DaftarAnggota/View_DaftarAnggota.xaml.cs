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

namespace hitung_shu.UI.UI_DaftarAnggota
{
    /// <summary>
    /// Interaction logic for View_DaftarAnggota.xaml
    /// </summary>
    public partial class View_DaftarAnggota : UserControl
    {
        private IDataAnggotaServices _dataAnggotaServices;

        private Insert_DaftarAnggota _insertDaftarAnggota;
        private Edit_DaftarAnggota _editDaftarAnggota;

        private ICollectionView _data;
        Dictionary<string, Predicate<DataAnggotaDto>> filters = new Dictionary<string, Predicate<DataAnggotaDto>>();
        public View_DaftarAnggota()
        {
            InitializeComponent();
            _dataAnggotaServices = new DataAnggotaServices();

            _insertDaftarAnggota = new Insert_DaftarAnggota();
            Init();
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            PopulateData();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Filter_NamaAnggota.Text = "";
            Filter_IdAnggota.Text = "";
            PopulateData();
        }

        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _insertDaftarAnggota = new Insert_DaftarAnggota();

                var result = _insertDaftarAnggota.ShowDialog();
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
                var idx = (DataAnggotaDto)Dgv_Home.SelectedItem;
                if (idx != null)
                {
                    _editDaftarAnggota = new Edit_DaftarAnggota(idx.IdAnggota);
                    var result = _editDaftarAnggota.ShowDialog();
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
                var idx = (DataAnggotaDto)Dgv_Home.SelectedItem;
                if (idx != null)
                {
                    var Result = MessageBox.Show("Apakah and ingin menghapus data ini ?", "Info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (Result == MessageBoxResult.Yes)
                    {
                        _dataAnggotaServices.Delete(idx.Id);

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

        private void Init()
        {
            #region --- DGV Main ---
            Dgv_Home.CanUserAddRows = false;
            Dgv_Home.CanUserDeleteRows = false;
            Dgv_Home.IsReadOnly = true;
            Dgv_Home.AutoGenerateColumns = false;

            DataGridTextColumn Id = new DataGridTextColumn();
            DataGridTextColumn IdAnggota = new DataGridTextColumn();
            DataGridTextColumn NamaAnggota = new DataGridTextColumn();
            DataGridTextColumn TanggalGabung = new DataGridTextColumn();
            DataGridTextColumn Status = new DataGridTextColumn();
            DataGridTextColumn StatusDesc = new DataGridTextColumn();
            DataGridTextColumn DUMMY = new DataGridTextColumn();

            Binding TANGGALGABUNGBinding = new Binding("TanggalGabung");
            TANGGALGABUNGBinding.StringFormat = "dd MMM yyyy";

            Id.Binding = new Binding("Id");
            NamaAnggota.Binding = new Binding("NamaAnggota");
            IdAnggota.Binding = new Binding("IdAnggota");
            TanggalGabung.Binding = TANGGALGABUNGBinding;
            Status.Binding = new Binding("Status");
            StatusDesc.Binding = new Binding("StatusDesc");

            Id.Header = "ID";
            IdAnggota.Header = "ID Anggota";
            NamaAnggota.Header = "Nama Anggota";
            TanggalGabung.Header = "Tanggal Gabung";
            Status.Header = "Status";
            StatusDesc.Header = "Status";

            Dgv_Home.Columns.Add(IdAnggota);
            Dgv_Home.Columns.Add(NamaAnggota);
            Dgv_Home.Columns.Add(TanggalGabung);
            Dgv_Home.Columns.Add(StatusDesc);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(Id);
            Dgv_Home.Columns.Add(Status);
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Id.Visibility = Visibility.Hidden;

            #endregion

            PopulateData();
        }

        private void PopulateData()
        {
            try
            {
                var Data = _dataAnggotaServices.GetAll().OrderByDescending(x => x.TanggalGabung).ToList();
                if (!string.IsNullOrEmpty(Filter_IdAnggota.Text))
                {
                    Data = Data.Where(x => x.IdAnggota.ToUpper().Contains(Filter_IdAnggota.Text.ToUpper())).ToList();
                }
                if (!string.IsNullOrEmpty(Filter_NamaAnggota.Text))
                {
                    Data = Data.Where(x => x.NamaAnggota.ToUpper().Contains(Filter_NamaAnggota.Text.ToUpper())).ToList();
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

        private void Dgv_Home_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var idx = (DataAnggotaDto)Dgv_Home.SelectedItem;
                if (idx != null)
                {
                    _editDaftarAnggota = new Edit_DaftarAnggota(idx.IdAnggota);
                    var result = _editDaftarAnggota.ShowDialog();
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
    }
}
