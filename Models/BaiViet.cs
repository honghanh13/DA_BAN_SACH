//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DA_BAN_SACH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BaiViet
    {
        public int MaBV { get; set; }
        public string TenBV { get; set; }
        public string HinhDD { get; set; }
        public string NoiDung { get; set; }
        public Nullable<System.DateTime> NgayDang { get; set; }
        public string LoaiTin { get; set; }
        public string NDTomTat { get; set; }
        public Nullable<bool> DaDuyet { get; set; }
        public Nullable<int> MaKH { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
    }
}
