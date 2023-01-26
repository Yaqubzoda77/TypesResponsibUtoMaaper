using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProductService
{
    private readonly DataContext _contex;
    private readonly IMapper _mapper;

    public ProductService(DataContext contex, IMapper mapper)
    {
        _contex = contex;
        _mapper = mapper;
    }

    public async Task<Response<List<ProductDto>>> GetProducts()
    {
        try
        {
            var result = await _contex.Products.ToListAsync();
            var mapped = _mapper.Map<List<ProductDto>>(result);
            return new Response<List<ProductDto>>(mapped);
        }
        catch (Exception ex)
        {
            return  new Response<List<ProductDto>>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }
    public async Task<Response<ProductDto>> AddProduct(ProductDto product)
    {
        try
        {
            var existingStudent = _contex.Products.Where(x => x.ProductId == product.ProductId).FirstOrDefault();
          
            var mapped = _mapper.Map<Product>(product);
            await _contex.Products.AddAsync(mapped);
            await _contex.SaveChangesAsync();
            return new Response<ProductDto>(product);
        }
        catch (Exception ex)
        {
            return new Response<ProductDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }

    public async Task<Response<ProductDto>> UpdateProduct(ProductDto product)
    {     try
        {
            var existingProduct = _contex.Products.Where(x => x.ProductId == product.ProductId).AsNoTracking().FirstOrDefault();
            if (existingProduct == null)
            {
                return new Response<ProductDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "We ca not updte it some thig is going wrong in " });
            }

            var mapped = _mapper.Map<Product>(product);
            _contex.Products.Update(mapped);
            await _contex.SaveChangesAsync();
            return new Response<ProductDto>(product);
            
            
        }
        catch (Exception ex)
        {
            
            return new Response<ProductDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});

        }
    }

    public async Task DeleteProduct(int id)
    {
        var delete = await _contex.Products.FirstAsync(x => x.ProductId == id);
        _contex.Products.Remove(delete);
        await _contex.SaveChangesAsync();

    }

}
