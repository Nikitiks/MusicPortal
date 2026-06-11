using MusicPortal.BLL.DTOs;
using MusicPortal.Common.Entities;

namespace MusicPortal.BLL.Mappers
{
    /// <summary>
    /// Розширення для маппінгу між Entity та DTO
    /// </summary>
    public static class MappingExtensions
    {
        // User mappings
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                SongsCount = user.Songs?.Count ?? 0
            };
        }

        public static IEnumerable<UserDto> ToDto(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDto());
        }

        // Song mappings
        public static SongDto ToDto(this Song song)
        {
            return new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                Artist = song.Artist,
                FileName = song.FileName,
                UploadDate = song.UploadDate,
                GenreId = song.GenreId,
                GenreName = song.Genre?.Name,
                UserId = song.UserId,
                Username = song.User?.Username
            };
        }

        public static IEnumerable<SongDto> ToDto(this IEnumerable<Song> songs)
        {
            return songs.Select(s => s.ToDto());
        }

        public static Song ToEntity(this CreateSongDto dto)
        {
            return new Song
            {
                Title = dto.Title,
                Artist = dto.Artist,
                FileName = dto.FileName,
                GenreId = dto.GenreId,
                UserId = dto.UserId,
                UploadDate = DateTime.Now
            };
        }

        public static void UpdateEntity(this UpdateSongDto dto, Song song)
        {
            song.Title = dto.Title;
            song.Artist = dto.Artist;
            song.GenreId = dto.GenreId;
        }

        // Genre mappings
        public static GenreDto ToDto(this Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                SongsCount = genre.Songs?.Count ?? 0
            };
        }

        public static IEnumerable<GenreDto> ToDto(this IEnumerable<Genre> genres)
        {
            return genres.Select(g => g.ToDto());
        }

        public static Genre ToEntity(this GenreDto dto)
        {
            return new Genre
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
