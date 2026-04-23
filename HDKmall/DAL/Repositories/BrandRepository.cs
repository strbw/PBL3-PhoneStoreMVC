using HDKmall.DAL.Interfaces;
using HDKmall.Models;
using System.Linq;
using System.Collections.Generic;

namespace HDKmall.DAL.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Brand> GetAll()
        {
            return _context.Brands.ToList();
        }

        public Brand GetById(int id)
        {
            return _context.Brands.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Brand brand)
        {
            _context.Brands.Add(brand);
        }

        public void Update(Brand brand)
        {
            _context.Brands.Update(brand);
        }

        public void Delete(int id)
        {
            var brand = GetById(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
