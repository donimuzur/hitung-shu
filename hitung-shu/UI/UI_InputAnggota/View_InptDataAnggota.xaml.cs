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
        private Import_InptDataAnggota _importIptDataAnggota;
        private Loading Dialog_Loading;

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

        private async void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Data = Dgv_Home.ItemsSource.Cast<IptAnggotaDto>().ToList();
                SaveFileDialog _saveFileDialog = new SaveFileDialog();
                _saveFileDialog.Filter = "Excel file (*.xls)|*.xlsx";
                if (_saveFileDialog.ShowDialog() == true)
                {
                    Dialog_Loading = new Loading();
                    Dialog_Loading.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    Dialog_Loading.Show();

                    await Task.Run(() => CreateXls(Data, _saveFileDialog.FileName));

                    Dialog_Loading.Close();
                    Dgv_Home.ItemsSource = _data;
                }
            }
            catch (Exception ex)
            {
                await Dialog_Loading.Dispatcher.BeginInvoke(new Action(() => { Dialog_Loading.Close(); }));
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
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
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _importIptDataAnggota = new Import_InptDataAnggota();
                var result = _importIptDataAnggota.ShowDialog();
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
        #region --- Export to XLS ---
        public void CreateXls(List<IptAnggotaDto> Data, string FilePath)
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
                slDocument.SetCellValue(1, 1, "Detail SHU Koperasi Polowijo Karya Abadi");
                slDocument.MergeWorksheetCells(1, 1, 1, 9);
                slDocument.SetCellValue(2, 1, "Periode 2020");
                slDocument.MergeWorksheetCells(2, 1, 2, 9);

                //create style
                SLStyle valueStyle = slDocument.CreateStyle();
                valueStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                valueStyle.Font.Bold = true;
                valueStyle.Font.FontSize = 16;
                slDocument.SetCellStyle(1, 1, 2, 1, valueStyle);

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
                Dialog_Loading.Close();
                throw;
            }
        }
        private SLDocument CreateHeaderExcel(SLDocument slDocument)
        {
            int iRow = 10;
            int col = 1;
            
            slDocument.SetCellValue(iRow, col++, "Id");
            slDocument.SetColumnWidth(1, 10);
            //slDocument.MergeWorksheetCells(iRow, 1, iRow + 1, 1);
            slDocument.SetCellValue(iRow, col++, "Id Anggota");
            slDocument.SetColumnWidth(2, 20);            
            //slDocument.MergeWorksheetCells(iRow, 2, iRow + 1, 2);
            slDocument.SetCellValue(iRow, col, "Nama Anggota");
            slDocument.SetColumnWidth(3, 15);
            col++;
            //slDocument.MergeWorksheetCells(iRow, 3, iRow + 1, 3);
            slDocument.SetCellValue(iRow, col, "Tanggal Gabung");
            slDocument.SetColumnWidth(4, 15);
            col++;
            //slDocument.MergeWorksheetCells(iRow, 4, iRow + 1, 4);
            slDocument.SetCellValue(iRow, col, "Pokok");
            slDocument.SetColumnWidth(5, 15);
            col++;
            //slDocument.MergeWorksheetCells(iRow, 5, iRow + 1, 5);
            slDocument.SetCellValue(iRow, col, "Wajib");
            slDocument.SetColumnWidth(6, 15);
            col++;
            //slDocument.MergeWorksheetCells(iRow, 6, iRow + 1, 6);
            slDocument.SetCellValue(iRow, col, "Sukarela");
            slDocument.SetColumnWidth(7, 15);
            col++;
            //slDocument.MergeWorksheetCells(iRow, 6, iRow + 1, 6);
            slDocument.SetCellValue(iRow, col, "Belanja");
            slDocument.SetColumnWidth(8, 15);
            col++;
            //slDocument.MergeWorksheetCells(iRow, 6, iRow + 1, 6);
            slDocument.SetCellValue(iRow, col, "Bunga Pinjaman");
            slDocument.SetColumnWidth(9, 15);
            col++;

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
            slDocument.SetCellStyle(10, 1, iRow, 9, headerStyle);

            return slDocument;
        }

        private SLDocument CreateDataExcel(SLDocument slDocument, List<IptAnggotaDto> listData)
        {
            int iRow = 11; //starting row data
            int row = 1;
            try
            {
                foreach (var data in listData)
                {
                    int col = 1;
                    slDocument.SetCellValue(iRow, col, data.Id);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.IdAnggota);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.NamaAnggota);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.Tanggal);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.Pokok);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.Wajib);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.Sukarela);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.Belanja);
                    col++;
                    slDocument.SetCellValue(iRow, col, data.BungaPinjaman);

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

            slDocument.SetCellStyle(11, 3, iRow, 9, decimalFormat);

            valueStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            valueStyle.SetVerticalAlignment(VerticalAlignmentValues.Center);
            valueStyle.Font.FontSize = 10;
            slDocument.SetCellStyle(10, 1, iRow, 9, valueStyle);

            return slDocument;
        }
        #endregion

      
    }
}
