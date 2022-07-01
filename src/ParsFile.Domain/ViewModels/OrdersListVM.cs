namespace ParsFile.Domain.ViewModels
{

    public class OrdersListVM
    {
        public IList<File> Files { get; set; } = new List<File>();

        public double TotalPrice { get; set; }

        public class File
        {
            public String? Id { get; set; }

            public String? Name { get; set; }

            public String? Image { get; set; }

            public double Price { get; set; }

            public int SaleCount { get; set; }

            public DateTime? CreateTime { get; set; }
        }

    }
}
