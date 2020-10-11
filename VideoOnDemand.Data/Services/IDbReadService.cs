﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoOnDemand.Data.Services
{
    public interface IDbReadService
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(int id, bool includeRelatedRecords = false) where TEntity : class;
        TEntity Get<TEntity>(string userId, int id) where TEntity : class;
        IEnumerable<TEntity> GetWithIncludes<TEntity>() where TEntity : class;
        SelectList GetSelectList<TEntity>(string valueField, string textField) where TEntity : class;
    }
}
