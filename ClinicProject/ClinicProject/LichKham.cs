//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class LichKham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LichKham()
        {
            this.PhieuKhams = new HashSet<PhieuKham>();
        }
    
        public int ID_LichKham { get; set; }
        public Nullable<int> ID_BenhNhan { get; set; }
        public Nullable<int> ID_BacSi { get; set; }
        public string TrangThai { get; set; }
        public string ChuDe { get; set; }
        public string MoTa { get; set; }
        public Nullable<System.DateTime> ThoiGian { get; set; }
    
        public virtual BenhNhan BenhNhan { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuKham> PhieuKhams { get; set; }
    }
}
