using polowijo.gosari.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.dal
{
   public interface IBagiShuServices
    {
        List<BagiShuDto> GetAll();
        void Insert(BagiShuDto Dto);
        List<BagiShuDto> HitungAllShu(int Tahun);
    }
}
