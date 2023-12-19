using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Data
{
    public class DonHangChiTiet
    {
        [Key]
        public int DhctId { get; set; }
        public int MaDh { get; set; }
        [ForeignKey("MaDh")]
        public int MaLaptop { get; set; }

        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public byte GiamGia { get; set; }
        // relationship
        public Laptop? Laptop { get; set; }
        public DonHang? DonHang { get; set; }

    }
}
