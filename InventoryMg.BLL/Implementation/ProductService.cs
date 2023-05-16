using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InventoryMg.BLL.DTOs.Request;
using InventoryMg.BLL.DTOs.Response;
using InventoryMg.BLL.Exceptions;
using InventoryMg.BLL.Interfaces;
using InventoryMg.DAL.Entities;
using InventoryMg.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using KeyNotFoundException = InventoryMg.BLL.Exceptions.KeyNotFoundException;
using NotImplementedException = InventoryMg.BLL.Exceptions.NotImplementedException;
using UnauthorizedAccessException = InventoryMg.BLL.Exceptions.UnauthorizedAccessException;

namespace InventoryMg.BLL.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product> _productRepo;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cloudName = "";
        private readonly string _apiKey = "";
        private readonly string _secretKey = "";
        

        public ProductService(IUnitOfWork unitOfWork, UserManager<UserProfile> userManager,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _productRepo = _unitOfWork.GetRepository<Product>();
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductResult> AddProductAsync(ProductViewRequest product)
        {

            Cloudinary _cloudinary = new Cloudinary(new Account(_cloudName, _apiKey, _secretKey));
            var result = new ImageUploadResult();
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExist = await _userManager.FindByIdAsync(userId);
            product.userId = userExist.Id;
            product.userId = userId;
            var file = product.File;

            if (userExist == null)
                throw new KeyNotFoundException($"User Id: {userId} does not match with the product");
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = $"{file.FileName}"
                };
                result = await _cloudinary.UploadAsync(uploadParams);

                //  var newProd = _mapper.Map<Product>(product);
                var newProd = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Category = product.Category,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    BrandName = product.BrandName,
                    ProductImagePath = result.SecureUri.ToString(),
                    UserId =  new Guid(userId)
                    
                };
                var createdProduct = await _productRepo.AddAsync(newProd);

                if (createdProduct != null)
                {

                    var toReturn = _mapper.Map<ProductView>(createdProduct);
                    return (new ProductResult()
                    {
                        Products = new List<ProductView>()
                        {
                          toReturn
                        },
                        Result = true,
                        Message = new List<string>() {
                        "Product was Added Successfully"
                        }
                    });
                }

                throw new NotImplementedException("Something went wrong,Unable to Add Product");
            }
            throw new NotImplementedException("Invalid File Size");
        }

        public async Task<ProductResult> DeleteProductAsync(Guid prodId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new NotFoundException("User not logged in");

            Product productToDelete = await _productRepo.GetSingleByAsync(p => p.Id == prodId);
            if (productToDelete == null)
                throw new NotFoundException($"Invalid product id: {prodId}");
            if (productToDelete.UserId.ToString() != userId)
                throw new UnauthorizedAccessException("You are not authorized to delete this product");
            await _productRepo.DeleteAsync(productToDelete);
            return (new ProductResult()
            {
                Result = true,
                Message = new List<string>()
                    {
                        "Product has Deleted successful"
                    },

            });

        }

        public async Task<ProductResult> EditProductAsync(string prodId, UpdateProduct product)
        {
            Cloudinary _cloudinary = new Cloudinary(new Account(_cloudName, _apiKey, _secretKey));
            var result = new ImageUploadResult();
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)

                throw new NotFoundException("User not logged in");
            UserProfile user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with id: {userId} not found");
            var file = product.File;

           /* if (file == null || file.Length == 0)
            {
                throw new NotImplementedException("No file uploaded.");
            }
            string path = "";*/
            if (file.Length > 0)
            {
                /* path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                 if (!Directory.Exists(path))
                 {
                     Directory.CreateDirectory(path);
                 }
                 using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                 {
                     await file.CopyToAsync(fileStream);
                 }*/
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = $"{file.FileName}"
                };
                result = await _cloudinary.UploadAsync(uploadParams);

                Product userProduct = await _productRepo.GetSingleByAsync(p => p.Id.ToString() == prodId);

                if (userProduct != null)
                {

                    userProduct.Name = product.Name;
                    userProduct.Description = product.Description;
                    userProduct.Price = product.Price;
                    userProduct.Quantity = product.Quantity;
                    userProduct.BrandName = product.BrandName;
                    userProduct.Category = product.Category;
                    userProduct.ProductImagePath = result.SecureUri.ToString();
                    Product updatedProduct = await _productRepo.UpdateAsync(userProduct);
                    if (updatedProduct != null)
                    {
                        return new ProductResult()
                        {
                            Products = new List<ProductView>
                                     {
                                         new ProductView()
                                         {
                                             Id = updatedProduct.Id,
                                             Name = updatedProduct.Name,
                                             Description = updatedProduct.Description,
                                             Price = updatedProduct.Price,
                                             Quantity = updatedProduct.Quantity,
                                             Category = updatedProduct.Category,
                                             BrandName =  updatedProduct.BrandName,
                                             ProductImagePath = updatedProduct.ProductImagePath
                                         }
                                     },
                            Result = true,
                            Message = new List<string>()
                                     {
                                         "Here are your Products"
                                     }

                        };
                    }
                }
                throw new NotImplementedException("Unbale to update product");
            }
            throw new NotFoundException($"Product with id: {prodId} not found");



        }

        public async Task<IEnumerable<ProductView>> GetAllUserProducts()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserProfile user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");
            var products = _productRepo.GetQueryable(p => p.UserId.ToString() == id);
            if (products == null)
                throw new NotFoundException("Products not found");
            var toReturn = _mapper.Map<IEnumerable<ProductView>>(products);
            return toReturn;

        }

        public async Task<ProductView> GetProductById(Guid prodId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new NotFoundException("User not logged in");
            Product product = await _productRepo.GetByIdAsync(prodId);
            if (product == null) throw new NotFoundException("Invalid Id");
            var toReturn = _mapper.Map<ProductView>(product);
            return toReturn;
        }

        public async Task<string> UploadProductImage(string prodId, IFormFile file)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new NotFoundException("User not logged in");

            var product = await _productRepo.GetSingleByAsync(p => p.Id.ToString() == prodId);
            if (product == null)
                throw new NotFoundException($"Product with id: {prodId} not found");

            if (product.UserId.ToString() != userId)
                throw new UnauthorizedAccessException("You are not authorized to perform this action");
            if (file == null || file.Length == 0)
            {
                throw new NotImplementedException("No file uploaded.");
            }
            string path = "";
            if (file.Length > 0)
            {
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                product.ProductImagePath = path + $"/{file.FileName}";

                var updatedProd = await _productRepo.UpdateAsync(product);
                if (updatedProd == null)
                {
                    throw new NotImplementedException("Unable to upload image");
                }
                return $"File '{file.FileName}' was uploaded. Path: '{product.ProductImagePath}'";
            }

            throw new NotImplementedException("Invalid file size");
        }
    }
}
