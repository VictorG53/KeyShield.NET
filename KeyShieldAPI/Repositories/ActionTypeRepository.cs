using KeyShieldDB.Context;
using KeyShieldDB.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Repositories;

public class ActionTypeRepository(KeyShieldDbContext dbContext)
{
    public async Task<List<ActionType>> GetListActionType()
    {
        List<ActionType> actionTypes = await dbContext.ActionTypes.ToListAsync();

        return actionTypes;
    }
}