namespace LaptopStore.Models
{
    public class Laptop
    {
        public int LaptopId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? BigPrice { get; set; }
        public string? Color { get; set; } = string.Empty;
        public int Quantity { get; set; } // so luong ton kho
        public string ImgPath { get; set; } = string.Empty;

        public ICollection<LaptopCategory>? LaptopCategories { get; set; }
        public ICollection<LikeProduct>? LikeProducts { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public IFormFile? Image { get; set; }

        public string? Brand { get; set; } // Thuong hieu 
        public string? SeriesLaptop { get; set; }
        public string?Cpu { get; set; }
        public string? Chip { get; set; }
        public string?RAM { get; set; }
        public string?Memory { get; set; } // bo nho
        public string?BlueTooth { get; set; }
        public string?Keyboard { get; set; }
        public string?OperatingSystem { get; set; }
        public string?Pin { get; set; }
        public string?weight { get; set; }
        public string?Accessory { get; set; } // Phụ kiện
        public string?Screen { get; set; }
        public int?CategoryId { get; set; } //dùng để đẩy qua api 

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
