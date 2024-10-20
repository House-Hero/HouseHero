﻿using Azure.Core;
using BLL.Interface;
using DAL.Data.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Customer GetCustomerByApplicationUserId(int applicationUserId)
        {
            
                var customer= _context.Customers
                    .FirstOrDefault(c => c.ApplicationUserId == applicationUserId);
            return customer;
        }

        public IEnumerable<SavedProvider> GetSaved(int CustomerId)
        {
            var customer = _context.SavedProviders.Where(c => c.CustomerId == CustomerId);
            return customer.ToList();
        }

        public void SaveProviders(int CustomerId, int ProviderId)
        {
            var save =new SavedProvider() { CustomerId = CustomerId, ProviderId = ProviderId };
            _context.SavedProviders.Add(save);
            _context.SaveChanges();
        }

        public void SaveRequest(Requests requests)
        {
            _context.Requests.Add(requests);
            _context.SaveChanges();
        }

        public void UnSaveProviders(int CustomerId, int ProviderId)
        {
            var save = new SavedProvider() { CustomerId = CustomerId, ProviderId = ProviderId };
            _context.SavedProviders.Remove(save);
            _context.SaveChanges();
        }
        public Customer GetCustomerById(int CustomerId)
        {
            if (CustomerId <= 0)
            {
                throw new ArgumentException("ProviderId must be greater than zero.", nameof(CustomerId));
            }
            var customer = _context.Customers
                .Include(AU => AU.ApplicationUser)
                .FirstOrDefault(c => c.Id == CustomerId);

            return customer;
        }
        public Customer GetAllCustomerDetiles(int id)
        {
            return _context.Customers
                .Include(c => c.ApplicationUser)
                .ThenInclude(AU => AU.City)
                .FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCustomerApplactionUser(ApplicationUser user)
        {
            if(user is not null)
            {
            var result = _context.Users.Where(c => c.Id == user.Id).FirstOrDefault();
                if (result != null)
                {
                    result.Address = user.Address;
                    result.City = user.City;
                    result.ProfilePicture_ID = user.ProfilePicture_ID;
                    result.Age = user.Age;
                    result.Name = user.Name;
                    result.PhoneNumber = user.PhoneNumber;
                    _context.Users.Update(result);
                    _context.SaveChanges();
                }
            }
        }
    }
}
