namespace LaptopStore.Models
{
    public class Laptop
    {
        public int LaptopId { get; set; }
        public string Name { get; set; } = string.Empty;
         public string? Description { get; set; } = string.Empty;
            public string? Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } // so luong ton kho
        public string Description { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public IFormFile? Image { get; set; }

    }


   /* public class DonHangChiTiet
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
    }*/
}
