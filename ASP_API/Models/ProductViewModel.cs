namespace ASP_API.Models
{
    public class ProductUploadImageViewModel
    {
        public IFormFile Image { get; set; }
    }


    public class ProductImageItemViewModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }

    public class ProductCreateViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double? DiscountPrice { get; set; }
        public List<int> ImagesID { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Priority { get; set; }
        public bool Status { get; set; }
        
    }
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double? DiscountPrice { get; set; }
        public List<int> ImagesID { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Priority { get; set; }
        public bool Status { get; set; }

    }
    public class ProductItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double? DiscountPrice { get; set; }
        public bool IsDelete { get; set; }
        public List<ProductImageItemViewModel> Images { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }
    }
}
