﻿using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;

namespace COURSEPROJECT.Services
{
    public interface IRatingService: IService<Rating>
    {
        Task<bool> EditAsync(int id,string UserId ,Rating rating, CancellationToken cancellationToken = default);

    }
}
