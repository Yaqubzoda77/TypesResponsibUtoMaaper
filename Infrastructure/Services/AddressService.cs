using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AddressService
{
    private readonly DataContext _contex;
    private readonly IMapper _mapper;

    public AddressService(DataContext contex, IMapper mapper)
    {
        _contex = contex;
        _mapper = mapper;
    }

    public async Task<Response<List<AddressDto>>> GetAdresses()
    {
        try
        {
            var result = await _contex.Addresses.ToListAsync();
            var mapped = _mapper.Map<List<AddressDto>>(result);
            return new Response<List<AddressDto>>(mapped);
        }
        catch (Exception ex)
        {
            return  new Response<List<AddressDto>>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }

    public async Task<Response<AddressDto>> AddAddress(AddressDto address)
    {
        try
        {
            var existingStudent = _contex.Addresses.Where(x => x.AddressId == address.AddressId).FirstOrDefault();
            if (existingStudent != null)
            {
                return new Response<AddressDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "Customer with this Address already exists" });
            }
            var mapped = _mapper.Map<Address>(address);
            await _contex.Addresses.AddAsync(mapped);
            await _contex.SaveChangesAsync();
            return new Response<AddressDto>(address);
        }
        catch (Exception ex)
        {
            return new Response<AddressDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }

    public async  Task<Response<AddressDto>> UpdateAddress(AddressDto address)
    {
        try
        {
            var existingStudent = _contex.Addresses.Where(x => x.AddressId == address.AddressId).FirstOrDefault();
            if (existingStudent == null)
            {
                return new Response<AddressDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "We ca not updte it some thig is going wrong in " });
            }

            var mapped = _mapper.Map<Address>(address);
             _contex.Addresses.Update(mapped);
            await _contex.SaveChangesAsync();
            return new Response<AddressDto>(address);
            
            var updated = _contex.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            
            return new Response<AddressDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});

        }
    }

    public async Task DeleteAddress(int id)
    {
        try
        {
            
            var delete = await _contex.Addresses.FirstAsync(x => x.AddressId == id);
            _contex.Addresses.Remove(delete);
            await _contex.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       

    }


}
