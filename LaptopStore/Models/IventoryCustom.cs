using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models
{
    public class IventoryCustom
    {
        public List<Laptop>? Laptops { get; set; }
        public List<InventoryTicket> InventoryTickets { get; set; }

        public int Page { get; set; } // Trang hiện tại
        public int PageSize { get; set; } // Kích thước trang (số sản phẩm trên mỗi trang)
        public int TotalPages { get; set; } // Tổng số trang

        public int TotalTicketPages { get; set; }
        public int PageTicket {get; set; }
        public int PageSizeTicket {get; set; }
        public List<Laptop>? Laptop2s { get; set; }


    }
}
