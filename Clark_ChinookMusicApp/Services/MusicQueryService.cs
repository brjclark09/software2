using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clark_ChinookMusicApp.Data;
using Clark_ChinookMusicApp.Models;
using Clark_ChinookMusicApp.Models.Dtos;
using Clark_ChinookMusicApp.Models.Entities;
using System.Security;

namespace Clark_ChinookMusicApp.Services;

public class MusicQueryService
{
    private readonly ApplicationDbContext _context;
    public MusicQueryService(ApplicationDbContext context)
    {
        _context = context;
    }

    // #1
    public async Task<List<string>> GetAllArtistsWithAlbums()
    {
        // SELECT Artist.Albums
        // FROM Artist
        return await _context.Artists
            .Include(artist => artist.Albums)
                .ThenInclude(album => album.Title)
            .Select(artist => artist.Name)
            .ToListAsync();
    }

    // #2
    public async Task<List<string>> GetAllArtistsWithMoreThanOneAlbum()
    {
        return await _context.Artists
            .Where(artist => artist.Albums.Count > 1)
            .Select(artist => artist.Name)
            .ToListAsync();
    }

    // #3
    public async Task<List<string>> GetArtistByNameWithAlbums(string artistName)
    {
        return await _context.Artists
            .Where(artist => artist.Name == artistName)
            .Include(artist => artist.Albums)
                .ThenInclude(album => album.Title)
            .Select(artist => artist.Name)
            .ToListAsync();
    }

    // #4
    public async Task<List<string>> GetTracksByAlbumId(int albumId)
    {
        return await _context.Tracks
            .Where(track => track.Album.AlbumId == albumId)
            .Select(track => track.Name)
            .ToListAsync();
    }

    // #5
    public async Task<List<string>> GetAllGenresWithTracks()
    {
        return await _context.Genres
            .Include(genre => genre.Tracks)
                .ThenInclude(track => track.Name)
            .Select(genre => genre.Name)
            .ToListAsync();
    }

    // #6
    public async Task<List<string>> GetTracksByGenreId(int genreId)
    {
        return await _context.Tracks
            .Where(track => track.GenreId == genreId)
            .Select(track => track.Name)
            .ToListAsync();
    }

    // #7
    public async Task<List<Statistic>> GetTotalTracksByAlbum()
    {
        return await _context.Albums
            .Select(album => new Statistic
            {
                Label = album.Title,
                Value = album.Tracks.Count()
            })
            .ToListAsync();
    }

    // #8
    public async Task<List<string>> GetAlbumsByArtistId(int artistId)
    {
        return await _context.Albums
            .Where(album => album.ArtistId == artistId)
            .Select(album => album.Title)
            .ToListAsync();
    }

    // #9
    public async Task<List<string>> GetAllPlaylistsWithTracks()
    {
        return await _context.Playlists
            .Include(playlist => playlist.Tracks)
                .ThenInclude(track => track.Name)
            .Select(playlist => playlist.Name)
            .ToListAsync();
    }

    // #10
    // public async Task<List<Statistic>> GetAverageDurationByGenre()
    // {
    //     return await _context.Genres
    //         .Select(genre => new Statistic
    //         {
    //             Label = genre.Name,
    //             Value = genre.Tracks.Average(track => track.Milliseconds) / 1000,
    //             ValueMetric = "Seconds"
    //         })
    //         .ToListAsync();
    // }

    // #11
    public async Task<List<string>> GetArtistsWithoutAlbums()
    {
        return await _context.Artists
            .Where(artist => artist.Albums.Count == 0)
            .Select(artist => artist.Name)
            .ToListAsync();
    }

    // #12
    public async Task<List<string>> GetTracksWithGenreAndAlbums()
    {
        return await _context.Tracks
            .Include(track => track.Genre)
            .Include(track => track.Album)
            .Select(track => track.Name)
            .ToListAsync();
    }

    // #13
    public async Task<List<TrackDetails>> GetTrackDetails()
    {
        return await _context.Tracks
            .Select(track => new TrackDetails
            {
                Track = track.Name,
                Album = track.Album.Title,
                Artist = track.Composer
            })
            .ToListAsync();
    }

    // #14
    public async Task<List<Statistic>> GetAlbumsWithTrackDuration()
    {
        return await _context.Albums
            .Select(album => new Statistic
            {
                Label = album.Title,
                Value = album.Tracks.Sum(track => track.Milliseconds) / 1000,
                ValueMetric = "Seconds"
            })
            .ToListAsync();
    }

    // #15
    public async Task<List<Statistic>> GetGenreTrackCounts()
    {
        return await _context.Genres
            .Select(genre => new Statistic
            {
                Label = genre.Name,
                Value = genre.Tracks.Count(),
                ValueMetric = "Count"
            })
            .ToListAsync();
    }

    // #16
    public async Task<List<Statistic>> GetPlaylistsWithTrackCount()
    {
        return await _context.Playlists
            .Select(playlist => new Statistic
            {
                Label = playlist.Name,
                Value = playlist.Tracks.Count(),
                ValueMetric = "Count"
            })
            .ToListAsync();
    }

    // #17
    // public async Task<List<Track>> GetTracksByPlaylistId(int playlistId)
    // {
    //     return await _context.Playlists
    //         .Where(playlist => playlist.PlaylistId == playlistId)
    //         .Select(playlist => playlist.Tracks)
    //         .ToListAsync();
    // }

    // #18
    public async Task<Playlist?> GetPlaylistWithMostTracks()
    {
        return await _context.Playlists
            .OrderByDescending(playlist => playlist.Tracks.Count)
            .FirstOrDefaultAsync();
    }

    // #19
    public async Task<Playlist?> GetPlaylistWithLeastTracks()
    {
        return await _context.Playlists
            .OrderBy(playlist => playlist.Tracks.Count)
            .FirstOrDefaultAsync();
    }

    // #20
    public async Task<List<Statistic>> GetTopFivePlaylistsWithMostTracks()
    {
        return await _context.Playlists
            .OrderByDescending(playlist => playlist.Tracks.Count)
            .Take(5)
            .Select(playlist => new Statistic
            {
                Label = playlist.Name,
                Value = playlist.Tracks.Count
            })
            .ToListAsync();
    }

    // #21
    public async Task<List<Statistic>> GetBottomFivePlaylistsWithLeastTracks()
    {
        return await _context.Playlists
            .OrderBy(playlist => playlist.Tracks.Count)
            .Take(5)
            .Select(playlist => new Statistic
            {
                Label = playlist.Name,
                Value = playlist.Tracks.Count
            })
            .ToListAsync();
    }
}