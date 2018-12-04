using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class Product
  {
    [Key]
    public int ProductId {get;set;}

    [Required]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated {get;set;}

    [Required]
    [StringLength(255)]
    [RegularExpression(@"[A-Za-z0-9`~ ]*", ErrorMessage = "Please refrain from using the following characters !@#$%^&*().")]
    public string Description { get; set; }

    [Required]
    [StringLength(55, ErrorMessage="Please shorten the product title to 55 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required]
    [Range(0.001, 10000, ErrorMessage = "Please list a product for no more than $10,000"), DataType(DataType.Currency) ]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public double Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public string UserId {get; set;}

    [Required]
    public string City {get; set;}

    [Required]
    public string ImagePath {get; set;}

    [Required]
    public ApplicationUser User { get; set; }

    [Required]
    [Display(Name="Product Category")]
    public int ProductTypeId { get; set; }

    public ProductType ProductType { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; }

  }
}
