using polowijo.gosari.dto;
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

namespace hitung_shu.UI.UI_BagiSHU
{
    /// <summary>
    /// Interaction logic for Window_BagiShuAnggota.xaml
    /// </summary>
    public partial class Window_BagiShuAnggota : Window
    {
        private BagiShuDto Data = new BagiShuDto();
        public Window_BagiShuAnggota(BagiShuDto Dto)
        {
            InitializeComponent();
            Data = Dto;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            id_anggota.Text = Data.IdAnggota;
            nama_anggota.Text = Data.NamaAnggota;

            total_simpanan.Text = string.Format("{0:N0}",Data.TotalSimpanan);
            total_belanja.Text = string.Format("{0:N0}", Data.TotalBelanja);
            bunga_pinjaman.Text = string.Format("{0:N0}", Data.TotalBungaPinjaman);

            Jua.Text = string.Format("{0:N0}", Data.Jua);
            Jma.Text = string.Format("{0:N0}", Data.Jma);
            Jpa.Text = string.Format("{0:N0}", Data.Jpa);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
