using HDKmall.DAL.Interfaces;
using HDKmall.Models;

namespace HDKmall.DAL.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContext _context;

        public BannerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Banner> GetAll()
        {
            return _context.Banners.OrderBy(b => b.Id).ToList();
        }

        public Banner GetById(int id)
        {
            return _context.Banners.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Banner banner)
        {
            _context.Banners.Add(banner);
        }

        public void Update(Banner banner)
        {
            _context.Banners.Update(banner);
        }

        public void Delete(int id)
        {
            var banner = GetById(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
