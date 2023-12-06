namespace LaptopStore.Models
{
    public class Laptop
    {
        public string? maLaptop { get; set; }
        public string? tenLaptop { get; set; }
        public decimal? gia { get; set; }
        public int? giamGia { get; set; }
        public int? loaiManHinh { get; set; }
        public string? mau { get; set; }
        public int? namSanXuat { get; set; }
        public string? mota { get; set; }
        public string? imgPath { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public int? categoryId { get; set; }
        public Category? category { get; set; }
        public List<DonHangChiTiet>? donHangChiTiets { get; set; }
    }

    public class Category
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public List<string>? laptops { get; set; }
    }

    public class DonHangChiTiet
    {
        public string? maLaptop { get; set; }
        public string? maDh { get; set; }
        public int? soLuong { get; set; }
        public decimal? donGia { get; set; }
        public int? giamGia { get; set; }
        public string? laptop { get; set; }
        public DonHang? donHang { get; set; }
    }

    public class DonHang
    {
        public string?  maDh { get; set; }
        public DateTime ngayDat { get; set; }
        public DateTime ngayGiao { get; set; }
        public int tinhTrangDonHang { get; set; }
        public string? nguoiNhan { get; set; }
        public string? diaChiGiao { get; set; }
        public string? sodienThoai { get; set; }
        public List<string>? donHangChiTiets { get; set; }
    }
}
