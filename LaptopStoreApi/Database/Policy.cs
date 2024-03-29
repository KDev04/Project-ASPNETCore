namespace LaptopStoreApi.Database
{
    public class Policy
    {
        public bool IsReadProduct { get; set; } = false;
        public bool IsWriteProduct { get; set;} = false;
        public bool IsUpdateProduct { get; set; } = false;
        public bool IsDeleteProduct { get; set;} = false;
        
        //Category
        public bool IsReadCategory { get; set; } = false;
        public bool IsWriteCategory { get; set;} = false;
        public bool IsUpdateCategory { get; set;} = false;
        public bool IsDeleteCategory { get; set;} = false;

        // Order 
        public bool IsReadOrder { get; set; } = false;
        public bool IsWriteOrder { get; set;} = false;
        public bool IsDeleteOrder { get; set; } = false;
        public bool IsUpdateOrder { get; set;} = false;


        // User 
        public bool IsReadUser { get; set; } = false;
        public bool IsWriteUser { get; set;} = false;
        public bool IsDeleteUser { get; set; } = false;
        public bool IsUpdateUser { get; set;} = false;

        
    }
}
