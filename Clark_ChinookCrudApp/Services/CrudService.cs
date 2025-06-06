using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clark_ChinookCrudApp.Services;
using Clark_ChinookCrudApp.Data;
using Clark_ChinookCrudApp.Interfaces;
using Clark_ChinookCrudApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clark_ChinookCrudApp.Services;

public class CrudService : ICrudSerivce
{
    private readonly ApplicationDbContext _context;

    public CrudService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> AddCustomerAsync(string firstName, string lastName, string email, int supportRepId)
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            SupportRepId = supportRepId
        };
        _context.Customer.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> UpdateTrackPriceAsync(int trackId, decimal newPrice)
    {
        bool result = false;
        var track = await _context.Track.FindAsync(trackId);
        if (track != null)
        {
            track.UnitPrice = newPrice;
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<bool> DeletePlaylistAsync(int playlistId)
    {
        bool result = false;
        var playlist = await _context.Playlist.FindAsync(playlistId);
        if (playlist != null)
        {
            _context.Playlist.Remove(playlist);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<Album> CreateAlbumForArtistAsync(int artistId, string title)
    {
        var artist = await _context.Artist.FindAsync(artistId);
        var album = new Album
        {
            Title = title,
            ArtistId = artistId
        };
        _context.Album.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }

    ////////////////////////////////////////////////////////////////////////////////

    public async Task<int> UpdateTracksByComposerAsync(string composer, decimal newPrice)
    {
        var tracks = await _context.Track
            .Where(track => track.Composer == composer)
            .ToListAsync();
        foreach (var track in tracks)
        {
            track.UnitPrice = newPrice;
        }
        await _context.SaveChangesAsync();
        return tracks.Count();
    }

    public async Task<int> DeleteCustomersByCountryAsync(string country)
    {
        var customers = await _context.Customer
            .Where(customer => customer.Country == country)
            .ToListAsync();
        foreach (var customer in customers)
        {
            _context.Customer.Remove(customer);
        }
        await _context.SaveChangesAsync();
        return customers.Count();
    }
    public async Task<int> AdjustTrackPricesByGenreAsync(int genreId, decimal percentIncrease)
    {
        var tracks = await _context.Track
            .Where(track => track.GenreId == genreId)
            .ToListAsync();
        foreach (var track in tracks)
        {
            track.UnitPrice *= (1 + percentIncrease / 100);
        }
        await _context.SaveChangesAsync();
        return tracks.Count;
    }

    public async Task<int> DeleteEmptyPlaylistsAsync()
    {
        var playlists = await _context.Playlist
            .Where(playlist => playlist.Tracks.Count() == 0)
            .ToListAsync();
        foreach (var playlist in playlists)
        {
            _context.Playlist.Remove(playlist);
        }
        await _context.SaveChangesAsync();
        return playlists.Count();
    }

    public async Task<int> RenameComposerAsync(string oldName, string newName)
    {
        var tracks = await _context.Track
            .Where(track => track.Composer == oldName)
            .ToListAsync();
        foreach (var track in tracks)
        {
            track.Composer = newName;
        }
        await _context.SaveChangesAsync();
        return tracks.Count;
    }

    public async Task<int> DeleteCustomersWithNoInvoicesAsync()
    {
        var customers = await _context.Customer
            .Where(customer => !_context.Invoice.Any(invoice => invoice.CustomerId == customer.CustomerId))
            .ToListAsync();
        foreach (var customer in customers)
        {
            _context.Customer.Remove(customer);
        }
        await _context.SaveChangesAsync();
        return customers.Count();
    }

    public async Task<int> RenameAlbumsContainingKeywordAsync(string keyword, string appendText)
    {
        var albums = await _context.Album
            .Where(album => album.Title.Contains(keyword))
            .ToListAsync();
        foreach (var album in albums)
        {
            album.Title += appendText;
        }
        await _context.SaveChangesAsync();
        return albums.Count();
    }

    public async Task<int> DeleteTracksNotPurchasedAsync()
    {
        var tracks = await _context.Track
        .Where(track => !_context.InvoiceLine.Any(invoiceLine => invoiceLine.TrackId == track.TrackId))
        .ToListAsync();
        foreach (var track in tracks)
        {
            _context.Track.Remove(track);
        }
        await _context.SaveChangesAsync();
        return tracks.Count();
    }
}