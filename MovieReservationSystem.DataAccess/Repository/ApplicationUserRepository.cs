using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository
{
    internal class ApplicationUserRepository : IApplicationUserRepository
    {
        public Task AddAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Expression<Func<ApplicationUser, bool>> Predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetAllAsync(string? Selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetItemAsync(Expression<Func<ApplicationUser, bool>> expression, string? Selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> ReadAllAsync(string? Selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Expression<Func<ApplicationUser, bool>> Predicate, ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
