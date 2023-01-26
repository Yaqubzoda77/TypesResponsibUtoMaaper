using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class OrderService
{
    private readonly DataContext _contex;
    private readonly IMapper _mapper;

    public OrderService(DataContext contex, IMapper mapper)
    {
        _contex = contex;
        _mapper = mapper;
    }

    public async Task<Response<List<OrderDto>>> GetOrders()
    {
        try
        {
            var result = await _contex.Orders.ToListAsync();
            var mapped = _mapper.Map<List<OrderDto>>(result);
            return new Response<List<OrderDto>>(mapped);
        }
        catch (Exception ex)
        {
            return  new Response<List<OrderDto>>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }

    public async Task<Response<OrderDto>> AddOrder(OrderDto order)
    {
        try
        {
         
            var mapped = _mapper.Map<Order>(order);
            await _contex.Orders.AddAsync(mapped);
            await _contex.SaveChangesAsync();
            return new Response<OrderDto>(order);
        }
        catch (Exception ex)
        {
            return new Response<OrderDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});
        }
    }
    

 
    
    
    public async  Task<Response<OrderDto>> UpdateOrder(OrderDto orderDto)
    {
        try
        {
            var existingCustomer = _contex.Orders.Where(x => x.OrderId == orderDto.OrderId).AsNoTracking().FirstOrDefault();
            if (existingCustomer == null)
            {
                return new Response<OrderDto>(HttpStatusCode.BadRequest,
                    new List<string>() { "We ca not updte it some thig is going wrong in " });
            }

            var mapped = _mapper.Map<Order>(orderDto);
            _contex.Orders.Update(mapped);
            await _contex.SaveChangesAsync();
            return new Response<OrderDto>(orderDto);
            
            
        }
        catch (Exception ex)
        {
            
            return new Response<OrderDto>(HttpStatusCode.InternalServerError,new List<string>(){ex.Message});

        }
    }

    public async Task DeleteOrder(int id)
    {
        var delete = await _contex.Orders.FirstAsync(x => x.OrderId == id);
        _contex.Orders.Remove(delete);
        await _contex.SaveChangesAsync();

    }

}
