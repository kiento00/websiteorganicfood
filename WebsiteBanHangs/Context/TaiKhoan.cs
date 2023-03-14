namespace WebsiteBanHangs.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            DonHangs = new HashSet<DonHang>();
        }

        [Key]
        public int MaTK { get; set; }

        [StringLength(200)]
        public string HoTen { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(200)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string SDT { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        public int? MaLoaiTK { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }

        public virtual LoaiTaiKhoan LoaiTaiKhoan { get; set; }
    }
}
