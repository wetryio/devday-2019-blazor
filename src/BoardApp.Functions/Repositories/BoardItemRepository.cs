using BoardApp.Functions.Infrastructure;
using BoardApp.Shared.Models;
using Microsoft.Build.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoardApp.Functions.Repositories
{
    public interface IBoardItemRepository
    {
        Task<BoardItem[]> ReadAll();
        Task<BoardItem> Read(Guid id);
        Task<bool> Insert(BoardItem item);
        Task<bool> Update(BoardItem item);
        Task<bool> Delete(BoardItem item);
    }

    public class BoardItemRepository : IBoardItemRepository
    {
        private readonly BoardContext boardItemContext;

        public BoardItemRepository(BoardContext boardItemContext)
        {
            this.boardItemContext = boardItemContext;
        }

        public async Task<bool> Insert(BoardItem item)
        {
            try
            {
                await boardItemContext.AddAsync(item);
                return await boardItemContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<BoardItem> Read(Guid id)
        {
            return await boardItemContext.BoardItem.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<BoardItem[]> ReadAll()
        {
            return await boardItemContext.BoardItem.ToArrayAsync();
        }

        public async Task<bool> Update(BoardItem item)
        {
            try
            {
                boardItemContext.Entry(item).State = EntityState.Modified;
                return await boardItemContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(BoardItem item)
        {
            boardItemContext.Entry(item).State = EntityState.Modified;
            return await boardItemContext.SaveChangesAsync() > 0;
        }
    }
}
