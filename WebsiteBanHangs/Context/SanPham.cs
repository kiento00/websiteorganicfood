namespace WebsiteBanHangs.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        [Key]
        public int MaSP { get; set; }

        [StringLength(50)]
        public string TenSP { get; set; }

        public string MoTa1 { get; set; }

        public string MoTa2 { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public decimal? GiaBan { get; set; }

        [StringLength(50)]
        public string Hinh { get; set; }

        public int? SoLuongTon { get; set; }

        public int? MaLoaiSP { get; set; }

        public int? MaNCC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual LoaiSanPham LoaiSanPham { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
