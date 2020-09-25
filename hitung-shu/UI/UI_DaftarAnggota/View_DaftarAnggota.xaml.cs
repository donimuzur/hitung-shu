using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using polowijo.gosari.dal;
using polowijo.gosari.dto;
using polowijo.gosari.helpers;
using SpreadsheetLight;
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

        private Loading Dialog_Loading;
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

        private async void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Data = _dataAnggotaServices.GetAll();
                SaveFileDialog _saveFileDialog = new SaveFileDialog();
                _saveFileDialog.Filter = "Excel file (*.xls)|*.xlsx";
                if (_saveFileDialog.ShowDialog() == true)
                {
                    Dialog_Loading = new Loading();
                    Dialog_Loading.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    Dialog_Loading.Show();

                    await Task.Run(() => CreateXls(Data, _saveFileDialog.FileName));
                    System.Windows.MessageBox.Show("Data berhasil di export", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Dialog_Loading.Dispatcher.BeginInvoke(new Action(() => { Dialog_Loading.Close(); }));
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void CreateXls(string FilePath)
        {
            try
            {
                _dataAnggotaServices.ExportXls(FilePath);
            }
            catch (Exception)
            {

                throw;
            }

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

        #region --- Export to XLS ---
        public void CreateXls(List<DataAnggotaDto> Data, string FilePath)
        {
            try
            {
                var slDocument = new SLDocument();
                SLPageSettings ps = new SLPageSettings();
                ps.Orientation = OrientationValues.Landscape;
                ps.ScalePage(50);
                ps.PaperSize = SLPaperSizeValues.FolioPaper;
                ps.LeftMargin = 0;
                ps.RightMargin = 0;
                slDocument.SetPageSettings(ps);

                //title
                slDocument.SetCellValue(1, 1, "Data Anggota Koperasi");
                slDocument.MergeWorksheetCells(1, 1, 1, 5);

                //create style
                SLStyle valueStyle = slDocument.CreateStyle();
                valueStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                valueStyle.Font.Bold = true;
                valueStyle.Font.FontSize = 16;
                slDocument.SetCellStyle(1, 1, 1, 5, valueStyle);

                //create header
                slDocument = CreateHeaderExcel(slDocument);

                //create data
                slDocument = CreateDataExcel(slDocument, Data);

                var FullPath = FilePath;
                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }

                slDocument.SaveAs(FullPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private SLDocument CreateHeaderExcel(SLDocument slDocument)
        {
            int iRow = 3;

            slDocument.SetCellValue(iRow, 1, "ID");
            slDocument.SetColumnWidth(1, 10);
            //slDocument.MergeWorksheetCells(iRow, 1, iRow + 1, 1);
            slDocument.SetCellValue(iRow, 2, "ID Anggota");
            slDocument.SetColumnWidth(2, 15);
            //slDocument.MergeWorksheetCells(iRow, 2, iRow + 1, 2);
            slDocument.SetCellValue(iRow, 3, "Nama Anggota");
            slDocument.SetColumnWidth(3, 25);
            //slDocument.MergeWorksheetCells(iRow, 3, iRow + 1, 3);
            slDocument.SetCellValue(iRow, 4, "Tanggal Gabung");
            slDocument.SetColumnWidth(4, 15);
            //slDocument.MergeWorksheetCells(iRow, 4, iRow + 1, 4);
            slDocument.SetCellValue(iRow, 5, "Status");
            slDocument.SetColumnWidth(5, 15);
            
            SLStyle headerStyle = slDocument.CreateStyle();
            headerStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            headerStyle.Font.Bold = true;
            headerStyle.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            headerStyle.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            headerStyle.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            headerStyle.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            //headerStyle.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightGreen, System.Drawing.Color.LightGreen);
            headerStyle.SetWrapText(true);
            headerStyle.SetVerticalAlignment(VerticalAlignmentValues.Center);
            headerStyle.Font.FontSize = 10;
            slDocument.SetCellStyle(3, 1, iRow, 5, headerStyle);

            return slDocument;
        }

        private SLDocument CreateDataExcel(SLDocument slDocument, List<DataAnggotaDto> listData)
        {
            int iRow = 4; //starting row data
            int row = 1;
            try
            {
                foreach (var data in listData)
                {
                    slDocument.SetCellValue(iRow, 1, data.Id);
                    slDocument.SetCellValue(iRow, 2, data.IdAnggota);
                    slDocument.SetCellValue(iRow, 3, data.NamaAnggota);
                    slDocument.SetCellValue(iRow, 4, data.TanggalGabung);
                    slDocument.SetCellValue(iRow, 5, data.Status?"Aktif":"Tidak Aktif");
                    iRow++;
                    row++;
                }
            }
            catch (Exception)
            {
                throw;
            }

            //create style
            SLStyle valueStyle = slDocument.CreateStyle();
            valueStyle.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            valueStyle.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            valueStyle.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            valueStyle.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;

            SLStyle dateStyle = slDocument.CreateStyle();
            dateStyle.FormatCode = "dd-MMM-yyyy";

            SLStyle hourStyle = slDocument.CreateStyle();
            hourStyle.FormatCode = "HH:mm";

            SLStyle decimalFormat = slDocument.CreateStyle();
            decimalFormat.FormatCode = "#,##0.00";

            SLStyle decimalFormat2 = slDocument.CreateStyle();
            decimalFormat2.FormatCode = "Rp #,##0.00";

            slDocument.SetCellStyle(4, 4, iRow, 4, dateStyle);

            valueStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            valueStyle.SetVerticalAlignment(VerticalAlignmentValues.Center);
            valueStyle.Font.FontSize = 10;
            slDocument.SetCellStyle(4, 1, iRow, 5, valueStyle);

            return slDocument;
        }
        #endregion
    }
}
