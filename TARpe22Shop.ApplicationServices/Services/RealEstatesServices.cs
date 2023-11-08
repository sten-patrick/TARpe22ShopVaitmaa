﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TARpe22ShopVaitmaa.Core.Domain;
using TARpe22ShopVaitmaa.Core.Dto;
using TARpe22ShopVaitmaa.Core.ServiceInterface;
using TARpe22ShopVaitmaa.Data;

namespace TARpe22ShopVaitmaa.ApplicationServices.Services
{
    public class RealEstatesServices : IRealEstatesServices
    {
        private readonly TARpe22ShopVaitmaaContext _context;
        private readonly IFilesServices _filesServices;
        public RealEstatesServices
            (
            TARpe22ShopVaitmaaContext context,
            IFilesServices filesServices
            )
        {
            _context = context;
            _filesServices = filesServices;
        }
        public async Task<RealEstate> GetAsync(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }
        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            var realEstateProps = typeof(RealEstate).GetProperties();
            var realEstateDtoProps = typeof(RealEstateDto).GetProperties();
            for (int i = 0; i < realEstateProps.Length; i++)
            {
                var realEstateProp = realEstateProps[i];
                for (int j = 0; j < realEstateDtoProps.Length; j++)
                {
                    var realEstateDtoProp = realEstateDtoProps[j];
                    if (realEstateProp.Name == realEstateDtoProp.Name)
                    {
                        realEstateProp.SetValue(realEstate, realEstateDtoProp.GetValue(dto));
                    }
                }
            }
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;
            _filesServices.FilesToApi(dto, realEstate);

            await _context.RealEstates.AddAsync(realEstate);
            await _context.SaveChangesAsync();
            return realEstate;

            //realEstate.Id = Guid.NewGuid();
            //realEstate.Type = dto.Type;
            //realEstate.ListingDescription = dto.ListingDescription;
            //realEstate.Address = dto.Address;
            //realEstate.City = dto.City;
            //realEstate.PostalCode = dto.PostalCode;
            //realEstate.ContactPhone = dto.ContactPhone;
            //realEstate.ContactFax = dto.ContactFax;
            //realEstate.SquareMeters = dto.SquareMeters;
            //realEstate.Floor = dto.Floor;
            //realEstate.FloorCount = dto.FloorCount;
            //realEstate.Price= dto.Price;
            //realEstate.RoomCount = dto.RoomCount;
            //realEstate.BedroomCount = dto.BedroomCount;
            //realEstate.BathroomCount = dto.BathroomCount;
            //realEstate.WhenEstateListedAt = dto.WhenEstateListedAt;
            //realEstate.IsPropertySold = dto.IsPropertySold;
            //realEstate.DoesHaveSwimmingPool = dto.DoesHaveSwimmingPool;
            //realEstate.BuiltAt = dto.BuiltAt
        }
        public async Task<RealEstate> Delete(Guid id)
        {
            var realEstateId = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);
            _context.RealEstates.Remove(realEstateId);
            await _context.SaveChangesAsync();
            return realEstateId;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            var domain = new RealEstate()
            {
                Id = Guid.NewGuid(),
                Type = dto.Type,
                ListingDescription = dto.ListingDescription,
                Address = dto.Address,
                City = dto.City,
                PostalCode = dto.PostalCode,
                ContactPhone = dto.ContactPhone,
                ContactFax = dto.ContactFax,
                SquareMeters = dto.SquareMeters,
                Floor = dto.Floor,
                FloorCount = dto.FloorCount,
                Price = dto.Price,
                RoomCount = dto.RoomCount,
                BedroomCount = dto.BedroomCount,
                BathroomCount = dto.BathroomCount,
                WhenEstateListedAt = dto.WhenEstateListedAt,
                IsPropertySold = dto.IsPropertySold,
                DoesHaveSwimmingPool = dto.DoesHaveSwimmingPool,
                BuiltAt = dto.BuiltAt
            };
            _context.RealEstates.Update(domain);
            await _context.SaveChangesAsync();
            return domain;
        }
    }
}