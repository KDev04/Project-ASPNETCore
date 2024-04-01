using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreApi.Database
{

    public class Bill
    {
        [Key]
        public int Id { get; set; }

        public int? IdTicketForeign { get; set; }

        public int? IdOrderForeign { get; set; }

       



    }




}
