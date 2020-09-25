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

namespace hitung_shu.UI.UI_BagiSHU
{
    /// <summary>
    /// Interaction logic for View_BagiShu.xaml
    /// </summary>
    public partial class View_BagiShu : UserControl
    {
        private IDataKoperasiServices _dataKoperasiServices;
        private IBagiShuServices _bagiShuServices;

        private ICollectionView _data;

        private Loading Dialog_Loading;

        private int _intTahun;

        public View_BagiShu()
        {
            InitializeComponent();
            _dataKoperasiServices = new DataKoperasiServices();
            _bagiShuServices = new BagiShuServices();
            init();
        }
        public void init()
        {
            
            #region --- DGV Main ---
            Dgv_Home.CanUserAddRows = false;
            Dgv_Home.CanUserDeleteRows = false;
            Dgv_Home.IsReadOnly = true;
            Dgv_Home.AutoGenerateColumns = false;

            DataGridTextColumn Id = new DataGridTextColumn();
            DataGridTextColumn IdAnggota = new DataGridTextColumn();
            DataGridTextColumn NamaAnggota = new DataGridTextColumn();
            DataGridTextColumn TotalSimpanan = new DataGridTextColumn();
            DataGridTextColumn TotalBelanja = new DataGridTextColumn();
            DataGridTextColumn TotalBungaPinjaman = new DataGridTextColumn();
            DataGridTextColumn TotalSHU = new DataGridTextColumn();
            DataGridTextColumn JMA = new DataGridTextColumn();
            DataGridTextColumn JUA = new DataGridTextColumn();
            DataGridTextColumn JPA = new DataGridTextColumn();
            DataGridTextColumn Periode = new DataGridTextColumn();
            DataGridTextColumn DUMMY = new DataGridTextColumn();
          
            Binding TOTALSIMPANANBinding = new Binding("TotalSimpanan");
            TOTALSIMPANANBinding.StringFormat = "{0:N0}";
            Binding TOTALBELANJABinding = new Binding("TotalBelanja");
            TOTALBELANJABinding.StringFormat = "{0:N0}";
            Binding TOTALBUNGAPINJAMANBinding = new Binding("TotalBungaPinjaman");
            TOTALBUNGAPINJAMANBinding.StringFormat = "{0:N0}";
            Binding TOTALSHUBinding = new Binding("TotalShu");
            TOTALSHUBinding.StringFormat = "{0:N0}";
            Binding JMABinding = new Binding("Jma");
            JMABinding.StringFormat = "{0:N0}";
            Binding JUABinding = new Binding("Jua");
            JUABinding.StringFormat = "{0:N0}";
            Binding JPABinding = new Binding("Jpa");
            JPABinding.StringFormat = "{0:N0}";
            

            Id.Binding = new Binding("Id");
            IdAnggota.Binding = new Binding("IdAnggota");
            NamaAnggota.Binding = new Binding("NamaAnggota");
            TotalSimpanan.Binding = TOTALSIMPANANBinding;
            TotalBelanja.Binding = TOTALBELANJABinding;
            TotalBungaPinjaman.Binding = TOTALBUNGAPINJAMANBinding;
            TotalSHU.Binding = TOTALSHUBinding;
            JMA.Binding = JMABinding;
            JUA.Binding = JUABinding;
            JPA.Binding = JPABinding;
            Periode.Binding = new Binding("Periode");

            Id.Header = "ID";
            IdAnggota.Header = "ID Anggota";
            NamaAnggota.Header = "Nama Anggota";
            TotalSimpanan.Header = "Total Simpanan";
            TotalBelanja.Header = "Total Belanja";
            TotalBungaPinjaman.Header = "Total Bunga pinjaman";
            TotalSHU.Header = "Total SHU";
            JMA.Header = "JMA";
            JUA.Header = "JUA";
            JPA.Header = "JPA";
            Periode.Header = "Periode";
            
            Dgv_Home.Columns.Add(IdAnggota);
            Dgv_Home.Columns.Add(NamaAnggota);
            Dgv_Home.Columns.Add(Periode);
            Dgv_Home.Columns.Add(TotalSimpanan);
            Dgv_Home.Columns.Add(TotalBelanja);
            Dgv_Home.Columns.Add(TotalBungaPinjaman);
            Dgv_Home.Columns.Add(TotalSHU);
            Dgv_Home.Columns.Add(JMA);
            Dgv_Home.Columns.Add(JUA);
            Dgv_Home.Columns.Add(JPA);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(Id);
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;
            Id.Visibility = Visibility.Hidden;

            #endregion           

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
        private async void HitungShu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnSimpan.IsEnabled = false;
                Export.IsEnabled = false;

                if (!string.IsNullOrEmpty(Tahun.Text))
                {
                    int intTahun = Convert.ToInt32(Tahun.Text);
                    
                    _intTahun = intTahun;

                    Dialog_Loading = new Loading();
                    Dialog_Loading.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    Dialog_Loading.Show();

                    var Data = await Task.Run(() => _bagiShuServices.HitungAllShu(_intTahun));
                    _data = CollectionViewSource.GetDefaultView(Data);

                    Dialog_Loading.Close();
                    Dgv_Home.ItemsSource = _data;

                    BtnSimpan.IsEnabled = true;
                    Export.IsEnabled = true;
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
                if (!string.IsNullOrEmpty(Tahun.Text))
                {
                    var intTahun = Convert.ToInt32(Tahun.Text);
                    var GetDataShu = _dataKoperasiServices.GetByTahun(intTahun);
                    
                    if(GetDataShu == null)
                    {
                        return;
                    }
                    total_shu.Text = String.Format("{0:N}", GetDataShu.TotalShu);
                    total_simpanan.Text = String.Format("{0:N}", GetDataShu.TotalSimpanan);
                    total_pinjaman.Text = String.Format("{0:N}", GetDataShu.TotalPinjaman); 
                    total_belanja.Text = String.Format("{0:N}", GetDataShu.TotalPenjualan); 

                    double doubleDanaCadangan = 0;
                    doubleDanaCadangan = (double) GetDataShu.TotalShu * 0.3;
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

                    double doubleDanaPendidikan  = 0;
                    doubleDanaPendidikan = (double)GetDataShu.TotalShu * 0.1;
                    Pendidikan.Text = String.Format("{0:N}", doubleDanaPendidikan);

                    double doubleDanaSosial = 0;
                    doubleDanaSosial = (double)GetDataShu.TotalShu * 0.1;
                    DanaSosial.Text = String.Format("{0:N}", doubleDanaSosial);

                    var Data = _bagiShuServices.GetAll();

                    _data = CollectionViewSource.GetDefaultView(Data);
                    Dgv_Home.ItemsSource = _data;

                    HitungShu.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Data = Dgv_Home.ItemsSource.Cast<BagiShuDto>().ToList();
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
                Dialog_Loading.Dispatcher.BeginInvoke(new Action(() => { Dialog_Loading.Close(); }));
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
        }
        #region --- Export to XLS ---
        public void CreateXls(List<BagiShuDto> Data, string FilePath)
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

                total_shu.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(3, 1, "Total SHU = Rp. " + total_shu.Text); }));
                DanaCadangan.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(4, 1, "Dana Cadangan (30%) = Rp. " + DanaCadangan.Text); }));
                JasaModal.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(4, 4, "Jasa Modal (20%) = Rp. " + JasaModal.Text); }));
                JasaAnggota.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(5, 1, "Jasa Anggota (10%) = Rp. " + JasaAnggota.Text); }));
                PengurusPengawas.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(5, 4, "Dana Pengawas (10%) = Rp. " + PengurusPengawas.Text); }));
                KesejahteraanPegawai.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(6, 1, "Kesejahteraan Pegawai (10%) = Rp. " + KesejahteraanPegawai.Text); }));
                Pendidikan.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(6, 4, "Pendidikan (10%) = Rp. " + Pendidikan.Text); }));
                DanaSosial.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(7, 1, "Dana Sosial (10%) = Rp. " + DanaSosial.Text); }));
                total_simpanan.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(8, 1, "Total Simpanan = Rp. " + total_simpanan.Text); }));
                total_pinjaman.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(8, 4, "Total Piutang = Rp. " + total_pinjaman.Text); }));
                total_belanja.Dispatcher.BeginInvoke(new Action(() => { slDocument.SetCellValue(9, 1, "Total Penjualan = Rp. " + total_belanja.Text); }));

                //create style
                SLStyle valueStyle = slDocument.CreateStyle();
                valueStyle.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
                valueStyle.Font.Bold = true;
                valueStyle.Font.FontSize = 16;
                slDocument.SetCellStyle(1,1, 2,1, valueStyle);

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
            catch (Exception )
            {
                Dialog_Loading.Close();
                throw;
            }
        }
        private SLDocument CreateHeaderExcel(SLDocument slDocument)
        {
            int iRow = 10;

            slDocument.SetCellValue(iRow, 1, "ID Anggota");
            slDocument.SetColumnWidth(1, 10);
            //slDocument.MergeWorksheetCells(iRow, 1, iRow + 1, 1);
            slDocument.SetCellValue(iRow, 2, "Nama Anggota");
            slDocument.SetColumnWidth(2, 20);
            //slDocument.MergeWorksheetCells(iRow, 2, iRow + 1, 2);
            slDocument.SetCellValue(iRow, 3, "Total Simpanan");
            slDocument.SetColumnWidth(3, 15);
            //slDocument.MergeWorksheetCells(iRow, 3, iRow + 1, 3);
            slDocument.SetCellValue(iRow, 4, "Total Belanja");
            slDocument.SetColumnWidth(4, 15);
            //slDocument.MergeWorksheetCells(iRow, 4, iRow + 1, 4);
            slDocument.SetCellValue(iRow, 5, "Total Pinjaman");
            slDocument.SetColumnWidth(5, 15);
            //slDocument.MergeWorksheetCells(iRow, 5, iRow + 1, 5);
            slDocument.SetCellValue(iRow, 6, "JMA");
            slDocument.SetColumnWidth(6, 15);
            //slDocument.MergeWorksheetCells(iRow, 6, iRow + 1, 6);
            slDocument.SetCellValue(iRow, 7, "JUA");
            slDocument.SetColumnWidth(7, 15);
            slDocument.SetCellValue(iRow, 8, "JPA");
            slDocument.SetColumnWidth(8, 15);
            slDocument.SetCellValue(iRow, 9, "Total");
            slDocument.SetColumnWidth(9, 15);

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

        private SLDocument CreateDataExcel(SLDocument slDocument, List<BagiShuDto> listData)
        {
            int iRow = 11; //starting row data
            int row = 1;
            try
            {
                foreach (var data in listData)
                {
                    slDocument.SetCellValue(iRow, 1, data.IdAnggota);
                    slDocument.SetCellValue(iRow, 2, data.NamaAnggota);
                    slDocument.SetCellValue(iRow, 3, data.TotalSimpanan);
                    slDocument.SetCellValue(iRow, 4, data.TotalBelanja);
                    slDocument.SetCellValue(iRow, 5, data.TotalBungaPinjaman);
                    slDocument.SetCellValue(iRow, 6, data.Jma);
                    slDocument.SetCellValue(iRow, 7, data.Jua);
                    slDocument.SetCellValue(iRow, 8, data.Jpa);
                    slDocument.SetCellValue(iRow, 9, data.TotalShu);

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
