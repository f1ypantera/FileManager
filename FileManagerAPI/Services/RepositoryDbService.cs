﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManagerAPI.Context;
using FileManagerAPI.Interfaces;
using FileManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagerAPI.Services
{
    public class RepositoryDbService<T> : IRepositoryDbService<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly FileManagerDBcontext context;

        public RepositoryDbService(FileManagerDBcontext context)
        {
            dbSet = context.Set<T>();
            this.context = context;
        }
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }
    }
}
