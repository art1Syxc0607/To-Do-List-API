using BusinessLogic.DTO.Category;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponseDto> CreateAsync(int userId, CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _categoryRepository.CreateAsync(category);

        return new CategoryResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            Color = created.Color,
            CreatedAt = created.CreatedAt
        };
    }

    public async Task<List<CategoryResponseDto>> GetUserCategoriesAsync(int userId)
    {
        var categories = await _categoryRepository.GetUserCategoriesAsync(userId);

        return categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Color = c.Color,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
    }


    public async Task<CategoryResponseDto?> GetByIdAsync(int categoryId, int userId)
    {
        // 1. Получаем категорию
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        // 2. Проверяем, существует ли
        if (category == null)
            throw new UnauthorizedAccessException("Such Категория insn't here");

        // 3. Проверяем, принадлежит ли пользователю (БИЗНЕС-ПРАВИЛО!)
        if (category.UserId != userId)
            throw new UnauthorizedAccessException("Категория не принадлежит пользователю");

        // 4. Преобразуем в DTO
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task UpdateAsync(int userId, int id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if(category == null || category.UserId != userId)
            throw new UnauthorizedAccessException("Не выша котегория или нет такой");

        if(dto.Name != null)
            category.Name = dto.Name;

        if(dto.Description != null)
            category.Description = dto.Description;

        await _categoryRepository.UpdateAsync(category);

    }


    public async Task DeleteAsync(int userId, int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new InvalidOperationException("Нет такой котегории");

        if (category.UserId != userId)
            throw new UnauthorizedAccessException("Категория не принадлежит пользователю");

        await _categoryRepository.DeleteAsync(category);
    }

}



