using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CustomerService
{
    private readonly DataContext _contex;
    private readonly IMapper _mapper;

    public CustomerService(DataContext contex, IMapper mapper)
    {
        _contex = contex;
        _mapper = mapper;
    }


    public async Task<Response<List<CustomerDto>>> GetCustomers()
    {
        try
        {
            var result = await _contex.Customers.ToListAsync();
            var mapped = _mapper.Map<List<CustomerDto>>(result);
            return new Response<List<CustomerDto>>(mapped);
        }
        catch (Exception ex)
        {
            return  new Response<List<CustomerDto>>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }
    
    public async Task<Response<CustomerDto>> AddCustomer(CustomerDto customer)
    {
        
        try
        {
            var existingStudent = _contex.Customers.Where(x => x.CustomerId == customer.CustomerId).FirstOrDefault();
            if (existingStudent != null)
            {
                return new Response<CustomerDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "Customer with this Address already exists" });
            }
            var mapped = _mapper.Map<Customer>(customer);
            await _contex.Customers.AddAsync(mapped);
            await _contex.SaveChangesAsync();
            return new Response<CustomerDto>(customer);
        }
        catch (Exception ex)
        {
            return new Response<CustomerDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }


    
    public async  Task<Response<CustomerDto>> UpdateCustomer(CustomerDto customer)
    {
        try
        {
            var existingStudent = _contex.Customers.Where(x => x.CustomerId == customer.CustomerId).AsNoTracking().FirstOrDefault();
            if (existingStudent == null)
                return new Response<CustomerDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "We ca not updte it some thig is going wrong in " });            
            var mapped = _mapper.Map<Customer>(customer);
            _contex.Customers.Update(mapped);
            await _contex.SaveChangesAsync();
            return new Response<CustomerDto>(customer);
            
            var updated = _contex.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            
            return new Response<CustomerDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});

        }
    }
    
    
    public async  Task<Response<CustomerDto>> UpdateAddress(CustomerDto customerDto)
    {
        try
        {
            var existingStudent = _contex.Customers.Where(x => x.CustomerId == customerDto.CustomerId).AsNoTracking().FirstOrDefault();
            if (existingStudent != null)
            {
                return new Response<CustomerDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "We ca not updte it some thig is going wrong in " });
            }

            var mapped = _mapper.Map<Customer>(customerDto);
            _contex.Customers.Update(mapped);
            await _contex.SaveChangesAsync();
            return new Response<CustomerDto>(customerDto);
            
            var updated = _contex.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            
            return new Response<CustomerDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});

        }
    }

    public async Task DeleteCustomer(int id)
    {
        var delete = await _contex.Customers.FirstAsync(x => x.CustomerId == id);
        _contex.Customers.Remove(delete);
        await _contex.SaveChangesAsync();

    }

}