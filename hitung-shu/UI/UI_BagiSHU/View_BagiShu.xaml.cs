using polowijo.gosari.dal;
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
        public View_BagiShu()
        {
            InitializeComponent();
            _dataKoperasiServices = new DataKoperasiServices();
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

        }
        private void Tahun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Cek_Click(sender, e);
            }
        }

        private void Cek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(Tahun.Text))
                {
                    var intTahun = Convert.ToInt32(Tahun.Text);
                    var GetDataShu = _dataKoperasiServices.GetByTahun(intTahun);
                    
                    total_shu.Text = GetDataShu.TotalShu.ToString("{0:N0}");


                    HitungShu.IsEnabled = true;
                }
                HitungShu.IsEnabled = false;
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                throw;
            }
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
