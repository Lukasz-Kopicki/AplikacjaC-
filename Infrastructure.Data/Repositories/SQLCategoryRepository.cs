using DataAccessLogic.Entities;
using DataAccessLogic.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Infrastructure.Data.Repositories
{
    public class SQLCategoryRepository : ICategoriesRepository
    {
        private readonly AppDbContext context;

        public SQLCategoryRepository(AppDbContext context)
        {
            this.context = context;
        }
        
        public Category AddCategory(Category category)
        {
            context.Add(category);
            context.SaveChanges();
            return category;
        }

        public Category DeleteCategory(int Id)
        {
            Category category = context.Categories.Find(Id);
            if(category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            return category;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return context.Categories.Include(c => c.Subcategories);
        }

        public Category GetCategory(int Id)
        {
            return context.Categories.Include(e => e.Subcategories).Single(e => e.Id == Id);
        }

        public Category UpdateCategory(Category categoryChanges)
        {
            var category = context.Categories.Attach(categoryChanges);
            category.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return categoryChanges;
        }
    }
}
