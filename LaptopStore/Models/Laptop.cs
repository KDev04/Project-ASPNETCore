namespace LaptopStore.Models
{
    public class Laptop
    {
        public int? MaLaptop { get; set; }
        public string? TenLaptop { get; set; }
        public decimal? Gia { get; set; }
        public int? GiamGia { get; set; }
        public int? LoaiManHinh { get; set; }
        public string? Mau { get; set; }
        public int? NamSanXuat { get; set; }
        public string? Mota { get; set; }
        public string? ImgPath { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<DonHangChiTiet>? DonHangChiTiets { get; set; }
        public IFormFile? Image { get; set; }

    }

    public class Category
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public List<string>? laptops { get; set; }
    }

    public class DonHangChiTiet
    {
        public string? MaLaptop { get; set; }
        public string? MaDh { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public int? GiamGia { get; set; }
        public string? Laptop { get; set; }
        public DonHang? DonHang { get; set; }
    }

    public class DonHang
    {
        public string?  MaDh { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime NgayGiao { get; set; }
        public int TinhTrangDonHang { get; set; }
        public string? NguoiNhan { get; set; }
        public string? DiaChiGiao { get; set; }
        public string? SodienThoai { get; set; }
        public List<string>? DonHangChiTiets { get; set; }
    }
}
