using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhomMMT
{
    public class DanhSachLienLac
    {
        List<LienLac> dsll = new List<LienLac>();

        public void ThemLL(string ten)
        {
            dsll.Add(new LienLac(ten));
        }

    }




}
