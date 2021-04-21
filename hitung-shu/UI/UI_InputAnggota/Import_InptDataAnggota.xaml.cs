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
using System.Windows.Navigation;
using System.Diagnostics;
using System.IO;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Net;
using polowijo.gosari.helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using polowijo.gosari.dto;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using polowijo.gosari.dal;
using hitung_shu.Helper;

namespace hitung_shu.UI.UI_InputAnggota
{
    /// <summary>
    /// Interaction logic for Import_InptDataAnggota.xaml
    /// </summary>
    public partial class Import_InptDataAnggota : Window
    {
        private IDataAnggotaServices _dataAnggotaServices;
        private IIptAnggotaServices _iptAnggotaServices;
        private List<IptAnggotaDto> _listDto; 
        public Import_InptDataAnggota()
        {
            InitializeComponent();
            Init();
            _dataAnggotaServices = new DataAnggotaServices();
            _iptAnggotaServices = new IptAnggotaServices();
            _listDto = new List<IptAnggotaDto>();
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
            DataGridTextColumn Tanggal = new DataGridTextColumn();
            DataGridTextColumn Pokok = new DataGridTextColumn();
            DataGridTextColumn Wajib = new DataGridTextColumn();
            DataGridTextColumn Sukarela = new DataGridTextColumn();
            DataGridTextColumn Belanja = new DataGridTextColumn();
            DataGridTextColumn BungaPinjaman = new DataGridTextColumn();
            DataGridTextColumn Message = new DataGridTextColumn();
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
            IdAnggota.Binding = new Binding("IdAnggota");
            NamaAnggota.Binding = new Binding("NamaAnggota");
            Tanggal.Binding = TANGGALBinding;
            Pokok.Binding = POKOKBinding;
            Wajib.Binding = WAJIBBinding;
            Sukarela.Binding = SUKARELABinding;
            Belanja.Binding = BELANJABinding;
            BungaPinjaman.Binding = BUNGAPINJAMANBinding;
            Message.Binding = new Binding("Message");

            Id.Header = "ID";
            IdAnggota.Header = "ID Anggota";
            NamaAnggota.Header = "Nama Anggota";
            Tanggal.Header = "Tanggal";
            Pokok.Header = "Pokok";
            Wajib.Header = "Wajib";
            Sukarela.Header = "Sukarela";
            Belanja.Header = "Belanja";
            BungaPinjaman.Header = "Bunga Pinjaman";
            Message.Header = "Keterangan";

            Dgv_Home.Columns.Add(IdAnggota);
            Dgv_Home.Columns.Add(NamaAnggota);
            Dgv_Home.Columns.Add(Tanggal);
            Dgv_Home.Columns.Add(Pokok);
            Dgv_Home.Columns.Add(Wajib);
            Dgv_Home.Columns.Add(Sukarela);
            Dgv_Home.Columns.Add(Belanja);
            Dgv_Home.Columns.Add(BungaPinjaman);
            Dgv_Home.Columns.Add(Message);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(Id);
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;
            Id.Visibility = Visibility.Hidden;

            #endregion

        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var path = System.IO.Path.GetFullPath(e.Uri.ToString());
            var FilePathCopy = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Template Upload_" + DateTime.Now.ToString("yyyyMMddHHmmss")+".xls");
            File.Copy(path, FilePathCopy, true);
            Process.Start(FilePathCopy);

        }

        private void BtnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "Excel file| *.xls; *.xlsx";
            _openFileDialog.DefaultExt = ".xls";
            
            if (_openFileDialog.ShowDialog() == true)
            {
                string filename = _openFileDialog.FileName;
                txtUpload.Text = filename;
                BtnImport.IsEnabled = true;
            }
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<IptAnggotaDto> ListData = new List<IptAnggotaDto>();
                if (File.Exists(txtUpload.Text))
                {
                    if(System.IO.Path.GetExtension(txtUpload.Text) == ".xls" || System.IO.Path.GetExtension(txtUpload.Text) == ".xlsx")
                    {

                        IWorkbook workbook = null;
                        string fileName = txtUpload.Text;
                        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        if (fileName.IndexOf(".xlsx") > 0)
                            workbook = new XSSFWorkbook(fs);
                        else if (fileName.IndexOf(".xls") > 0)
                            workbook = new HSSFWorkbook(fs);
                        //First sheet
                        ISheet sheet = workbook.GetSheetAt(0);
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum; // This may not be valid row count.

                            string Message = "";
                            for (int i = 2; i <= rowCount; i++)
                            {
                                IRow curRow = sheet.GetRow(i);
                                // Works for consecutive data. Use continue otherwise 
                                if (curRow == null)
                                {
                                    // Valid row count
                                    rowCount = i - 1;
                                    continue;
                                }
                                // Get data from the 4th column (4th cell of each row)
                                var Data = new IptAnggotaDto();
                                try
                                {
                                    if(!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(0)))
                                    {
                                        var cellValue = curRow.GetCell(0).ToString();
                                        Data.IdAnggota = cellValue;
                                        var CheckAnggota = CheckAnggotaExist(Data.IdAnggota);
                                        if (CheckAnggota == null)
                                        {
                                            Data.Message = "ID Anggota tidak terdaftar";
                                        }
                                    }
                                }
                                catch (Exception ex){ Data.Message = ex.Message; }

                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(1)))
                                    {
                                        var cellValue = curRow.GetCell(1).ToString();
                                        Data.NamaAnggota = cellValue;
                                    }   
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }

                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(2)))
                                    {
                                        var cellValue = curRow.GetCell(2).DateCellValue;
                                        Data.Tanggal = cellValue;
                                    }
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }

                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(3)))
                                    {
                                        var cellValue = curRow.GetCell(3).NumericCellValue;
                                        Data.Pokok = Convert.ToDouble(cellValue);
                                    }
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }

                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(4)))
                                    {
                                        var cellValue = curRow.GetCell(4).NumericCellValue;
                                        Data.Wajib = Convert.ToDouble(cellValue);
                                    }
                                        
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }

                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(5)))
                                    {
                                        var cellValue = curRow.GetCell(5).NumericCellValue;
                                        Data.Sukarela = Convert.ToDouble(cellValue);
                                    }
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }

                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(6)))
                                    {
                                        var cellValue = curRow.GetCell(6).NumericCellValue;
                                        Data.Belanja = Convert.ToDouble(cellValue);
                                    }
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }


                                try
                                {
                                    if (!NpoiExtensions.IsNullOrEmpty(curRow.GetCell(7)))
                                    {
                                        var cellValue = curRow.GetCell(7).NumericCellValue;
                                        Data.BungaPinjaman = Convert.ToDouble(cellValue);
                                    }
                                }
                                catch (Exception ex) { Data.Message = ex.Message; }

                                Data.CreatedBy = "Admin";
                                Data.CreatedDate = DateTime.Now;
                                Data.ModifiedBy = "Admin";
                                Data.ModifiedDate = DateTime.Now;
                                Message = Message + Data.Message;
                                ListData.Add(Data);
                            }

                            ICollectionView _data = CollectionViewSource.GetDefaultView(ListData);
                            Dgv_Home.ItemsSource = _data;
                            if(string.IsNullOrEmpty(Message))
                            {
                                BtnSave.Visibility = Visibility.Visible;
                                _listDto = ListData;
                                return;
                            }
                            System.Windows.MessageBox.Show("Terdapat error pada file yang di upload \n check di kolom keterangan", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("File Tersebut tidak mendukung \n Silahkan download file template", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("File Tersebut Tidak ada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
        private void txtUpload_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                BtnImport.IsEnabled = false;
                if (!string.IsNullOrEmpty(txtUpload.Text))
                {
                    BtnImport.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result =  _iptAnggotaServices.InsertBulk(_listDto);
                System.Windows.MessageBox.Show("Data sudah tersimpan", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWin();
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                System.Windows.MessageBox.Show("Error!! \n telah terjadi kesalahan, Hubungi administrator", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CloseWin()
        {
            DialogResult = true;
            this.Close();
        }
    }
}
