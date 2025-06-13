using Clark_ChinookEnhancedQueries.Interfaces;
using Clark_ChinookEnhancedQueries.Models.Entities;
using Clark_ChinookEnhancedQueries.Models.Dtos;
using Clark_ChinookEnhancedQueries.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Clark_ChinookEnhancedQueries.Services;

public class EnhancedQueryService : IEnhancedQuery
{
    private readonly ApplicationDbContext _context;

    public EnhancedQueryService(ApplicationDbContext context)
    {
        _context = context;
    }

// 1
    /// Groups customers by their support representative's email.
    public async Task<Dictionary<string, List<Customer>>> CustomersGroupedBySupportRepAsync()
    {
        var result = await _context.Customer
            .GroupBy(customer => customer.SupportRep.Email)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.ToList()
            );
        return result;
    }

// 2
    /// Groups customers by country.
    public async Task<Dictionary<string, List<Customer>>> GetCustomersByCountry()
    {
        var result = await _context.Customer
            .GroupBy(customer => customer.Country)
            .ToDictionaryAsync(
                customer => customer.Key,
                customer => customer.ToList()
            );
        return result;
    }

// 3
    /// Retrieves a dictionary mapping album title to the number of tracks in each album.
    public async Task<Dictionary<string, int>> TrackCountByAlbumAsync()
    {
        var result = await _context.Track
            .GroupBy(track => track.Album.Title)
            .ToDictionaryAsync(
                track => track.Key,
                track => track.Count()
            );
        return result;
    }

// 4
    /// Retrieves the top three albums with the most tracks.
    public async Task<List<AlbumStatDto>> TopThreeAlbumsMostTracks()
    {
        var result = await _context.Album
            .Select(album => new AlbumStatDto
            {
                Title = album.Title,
                TrackCount = album.Tracks.Count()
            })
            .OrderByDescending(dto => dto.TrackCount)
            .Take(3)
            .ToListAsync();
        return result;
    }

// 5
    /// Groups tracks by composer.
    public async Task<Dictionary<string, List<Track>>> TracksByComposer()
    {
        var result = await _context.Track
            .GroupBy(track => track.Composer ?? "Unknown Composer")
            .ToDictionaryAsync(
                group => group.Key,
                group => group.ToList()
            );
        return result;
    }

// 6
    /// Lists each composer and the total number of tracks they have composed.
    public async Task<List<ComposerStatDto>> ComposersAndTracks()
    {
        var result = await _context.Track
            .GroupBy(track => track.Composer)
            .Select(track => new ComposerStatDto
            {
                Name = track.Key,
                TrackCount = track.Count()
            })
            .ToListAsync();
        return result;
    }

// 7
    /// Retrieves all tracks that belong to a specified genre.
    public async Task<List<Track>> GetTracksByGenreAsync(string genreName)
    {
        var result = await _context.Track
            .Where(track => track.Genre.Name == genreName)
            .ToListAsync();
        return result;
    }

// 8                                                                                                                 asdf
    /// Retrieves tracks longer than a given duration in seconds.
    public async Task<List<Track>> GetTracksLongerThanAsync(int seconds) 
    {
        var result = await _context.Track
            .Where(track => (track.Milliseconds / 1000) > seconds)
            .ToListAsync();
        return result;
    }
 
// 9
    /// Retrieves the top five most expensive tracks.
    public async Task<List<TrackStatDto>> FiveMostExpensiveTracks()
    {
        var result = await _context.Track
            .OrderByDescending(track => (double)track.UnitPrice)
            .Take(5)
            .Select(track => new TrackStatDto
            {
                Name = track.Name,
                Price = track.UnitPrice,
                AlbumTitle = track.Album.Title
            })
            .ToListAsync();
        return result;
    }
 
// 10                                                                                                                  asdf
    /// Retrieves a list of customers along with the total amount they have spent and their invoice count.
    public async Task<List<CustomerTransactionSummaryDto>> CustomersAndAmountSpent()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.Customer)
            .Select(invoice => new CustomerTransactionSummaryDto
            {
            Id = invoice.Key.CustomerId,
            FirstName = invoice.Key.FirstName,
            LastName = invoice.Key.LastName,
            TransactionTotal = invoice.Sum(invoice => invoice.Total),
            TransactionCount = invoice.Count()
            })
            .ToListAsync();
        return result;
    }

// 11                                                                                                                  asdf
    /// Retrieves a list of customers with more than five purchases, including total spent and purchase count.
    public async Task<List<CustomerTransactionSummaryDto>> CustomersToalPurchaseAmounts()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.Customer)
            .Where(invoice => invoice.Count() > 5)
            .Select(invoice => new CustomerTransactionSummaryDto
            {
            Id = invoice.Key.CustomerId,
            FirstName = invoice.Key.FirstName,
            LastName = invoice.Key.LastName,
            TransactionTotal = invoice.Sum(invoice => invoice.Total),
            TransactionCount = invoice.Count()
            })
            .ToListAsync();
        return result;
    }

// 12                                                                                                                  asdf
    /// Retrieves a list of customers who have made more than five purchases.
    public async Task<List<CustomerTransactionSummaryDto>> CustomersWithMoreThanFivePurchases()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.Customer)
            .Where(invoice => invoice.Count() > 5)
            .Select(invoice => new CustomerTransactionSummaryDto
            {
            Id = invoice.Key.CustomerId,
            FirstName = invoice.Key.FirstName,
            LastName = invoice.Key.LastName,
            TransactionTotal = invoice.Sum(invoice => invoice.Total),
            TransactionCount = invoice.Count()
            })
            .ToListAsync();
        return result;
    }

// 13
    /// Groups invoices by customer.
    public async Task<Dictionary<int, List<Invoice>>> InvoicesGroupedByCustomerAsync()
{
    var result = await _context.Invoice
        .GroupBy(invoice => invoice.CustomerId)
        .ToDictionaryAsync(
            group => group.Key,
            group => group.ToList()
        );
    return result;
}

// 14
    /// Retrieves the top N customers based on total spending.
    public async Task<List<CustomerTransactionSummaryDto>> GetTopCustomersBySpendingAsync(int count)
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.Customer)
            .Select(invoice => new CustomerTransactionSummaryDto
            {
                Id = invoice.Key.CustomerId,
                FirstName = invoice.Key.FirstName,
                LastName = invoice.Key.LastName,
                TransactionTotal = invoice.Sum(invoice => invoice.Total),
                TransactionCount = invoice.Count()
            })
            .OrderByDescending(dto => (double)dto.TransactionTotal)
            .Take(count)
            .ToListAsync();
        return result;
    }

// 15
    /// Calculates total revenue by billing country.
    public async Task<Dictionary<string, decimal>> RevenueByCountryAsync()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.BillingCountry)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Sum(invoice => invoice.Total)
            );
        return result;
    }

// 16                                                                                                              asdf
    /// Calculates the total number of transactions and the total revenue by country.
    public async Task<List<CountryTransactionSummaryDto>> TotalPurchasesByCountry()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.BillingCountry)
            .OrderBy(invoice => invoice.Sum(invoice => (double) invoice.Total))
            .Select(group => new CountryTransactionSummaryDto
            {
                Name = group.Key,
                TransactionCount = group.Count(),
                TransactionTotal = group.Sum(invoice => invoice.Total)
            })
            .ToListAsync();
        return result;
    }

// 17
    /// Retrieves all customers located in the specified country.
    public async Task<List<Customer>> GetCustomersByCountryAsync(string country)
    {
        var result = await _context.Customer
            .Where(customer => customer.Country == country)
            .ToListAsync();
        return result;
    }

// 18
    /// Retrieves all invoices within the specified date range.
    public async Task<List<Invoice>> GetInvoicesInDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var result = await _context.Invoice
            .Where(invoice => invoice.InvoiceDate >= startDate && invoice.InvoiceDate <= endDate)
            .ToListAsync();
        return result;
    }
}