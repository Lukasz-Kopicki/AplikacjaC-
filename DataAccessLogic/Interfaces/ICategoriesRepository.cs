using DataAccessLogic.Entities;
using System.Collections.Generic;

namespace DataAccessLogic.Interfaces
{
    public interface ICategoriesRepository
    {
        Category GetCategory(int Id);
        IEnumerable<Category> GetAllCategories();
        Category AddCategory(Category category);
        Category UpdateCategory(Category categoryChanges);
        Category DeleteCategory(int Id);
    }
}
