using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA_BAN_SACH.Models;
namespace DA_BAN_SACH.Models
{
    public class SachBaiViet
    {
        public IEnumerable<Sach> Saches { get; set; }
        public IEnumerable<BaiViet> BaiViets { get; set; }
    }
}