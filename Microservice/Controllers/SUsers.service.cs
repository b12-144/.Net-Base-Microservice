#region Imports
using Shared.Codes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks; 
#endregion

namespace Microservice.Services {
    public class UsersService {

        #region GetAll
        public List<EUser> GetAll(int listCount = -1, int pageNumber = 0, string orderBy="id asc") {
            using var context = new SMySQLContext();
            if (listCount == -1) return context.Users.OrderBy(orderBy).ToList();
            return context.Users.OrderBy(orderBy).Skip(pageNumber * listCount).Take(listCount).ToList();            
        }
        #endregion

        #region GetAllWithLinq
        public List<EUser> GetAllUsingLinq(int listCount = -1, int pageNumber = 0, string orderBy = "id asc") {
            using var context = new SMySQLContext();
            if (listCount == -1) return context.Users.OrderBy(orderBy).ToList();
            var list = (from eUser in context.Users
                    select eUser).OrderBy(orderBy).Skip(pageNumber * listCount).Take(listCount).ToList();
            return list;
        } 
        #endregion

        #region GetByID
        public EUser GetByID(Int64 id) {
            using var context = new SMySQLContext();
            var e = context.Users.SingleOrDefault(x => x.id == id);
            return e;
        }
        #endregion

        #region GetByIDUsingLinq
        public EUser GetByIDUsingLinq(Int64 id) {
            using var context = new SMySQLContext();
            EUser user = (from EUser u in context.Users
                          where u.id == id
                          select u).Single();
            return user;
        }
        #endregion

        #region SaveAsync
        public async Task<Int64> SaveAsync(EUser eUser) {
            //eUser.ModificationDateUTC = DateTime.UtcNow;
            await using var context = new SMySQLContext();
            if (eUser.id < 1) {
                //eUser.CreationDateUTC = eUser.ModificationDateUTC = DateTime.UtcNow;
                var e = await context.Users.AddAsync(eUser);
                await context.SaveChangesAsync();
                return e.Entity.id;
            } else {
                var e = context.Users.Update(eUser);
                await context.SaveChangesAsync();
                return e.Entity.id;
            }
        }
        #endregion

        #region InsertAsync
        public async Task<Int64> InsertAsync(EUser eUser) {
            await using var context = new SMySQLContext();
            var e = await context.Users.AddAsync(eUser);
            await context.SaveChangesAsync();
            return e.Entity.id;
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(EUser eUser) {
            await using var context = new SMySQLContext();
            if (context.Users.SingleOrDefault(x => x.id == eUser.id) == null) return false;
            var e = context.Users.Update(eUser);
            await context.SaveChangesAsync();
            return true;
        } 
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(Int64 id) {
            await using var context = new SMySQLContext();
            var e = context.Users.SingleOrDefault(x => x.id == id);
            if (e == null) return false;
            context.Remove(e);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
