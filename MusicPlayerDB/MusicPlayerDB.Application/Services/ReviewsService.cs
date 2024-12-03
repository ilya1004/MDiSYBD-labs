using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class ReviewsService
{
    private readonly ReviewsRepository _reviewsRepository;

    public ReviewsService(ReviewsRepository reviewsRepository)
    {
        _reviewsRepository = reviewsRepository;
    }

    /// <summary>
    /// Создание нового отзыва.
    /// </summary>
    public async Task<ResponseData<int>> Create(Review review)
    {
        if (review == null)
            return ResponseData<int>.Error("Review cannot be null.");

        if (string.IsNullOrWhiteSpace(review.Title))
            return ResponseData<int>.Error("Review title cannot be empty.");

        if (string.IsNullOrWhiteSpace(review.Text))
            return ResponseData<int>.Error("Review text cannot be empty.");

        if (review.Rating < 1 || review.Rating > 5)
            return ResponseData<int>.Error("Rating must be between 1 and 5.");

        return await _reviewsRepository.Create(review);
    }

    /// <summary>
    /// Получить все отзывы.
    /// </summary>
    public async Task<ResponseData<List<Review>>> GetAll()
    {
        return await _reviewsRepository.GetAll();
    }

    /// <summary>
    /// Получить отзыв по ID.
    /// </summary>
    public async Task<ResponseData<Review>> GetById(int id)
    {
        if (id <= 0)
            return ResponseData<Review>.Error("Invalid review ID.");

        return await _reviewsRepository.GetById(id);
    }

    /// <summary>
    /// Обновление информации об отзыве.
    /// </summary>
    public async Task<ResponseData<int>> Update(int id, Review review)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid review ID.");

        if (review == null)
            return ResponseData<int>.Error("Review cannot be null.");

        if (string.IsNullOrWhiteSpace(review.Title))
            return ResponseData<int>.Error("Review title cannot be empty.");

        if (string.IsNullOrWhiteSpace(review.Text))
            return ResponseData<int>.Error("Review text cannot be empty.");

        if (review.Rating < 1 || review.Rating > 5)
            return ResponseData<int>.Error("Rating must be between 1 and 5.");

        return await _reviewsRepository.Update(id, review);
    }

    /// <summary>
    /// Удаление отзыва по ID.
    /// </summary>
    public async Task<ResponseData<int>> Delete(int id)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid review ID.");

        return await _reviewsRepository.Delete(id);
    }

    /// <summary>
    /// Получить отзывы по ID песни.
    /// </summary>
    public async Task<ResponseData<List<Review>>> GetBySongId(int songId, int limit = 10, int offset = 0)
    {
        if (songId <= 0)
            return ResponseData<List<Review>>.Error("Invalid song ID.");

        if (limit <= 0)
            return ResponseData<List<Review>>.Error("Limit must be greater than 0.");

        if (offset < 0)
            return ResponseData<List<Review>>.Error("Offset cannot be negative.");

        return await _reviewsRepository.GetBySongId(songId, limit, offset);
    }
}

