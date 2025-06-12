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

    /// Groups customers by their support representative's email.
    public async Task<Dictionary<string, List<Customer>>> CustomersGroupedBySupportRepAsync()
    {
        var result = await _context.Customer
            .GroupBy(customer => customer.SupportRepId)
            .ToDictionaryAsync(
                group => group.Key.ToString(),
                group => group.ToList()
            );
        await _context.SaveChangesAsync();
        return result;
    }

    /// Groups customers by country.
    public async Task<Dictionary<string, List<Customer>>> GetCustomersByCountry()
    {
        var result = await _context.Customer
            .GroupBy(customer => customer.Country)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.ToList()
            );
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves a dictionary mapping album title to the number of tracks in each album.
    public async Task<Dictionary<string, int>> TrackCountByAlbumAsync()
    {
        var result = await _context.Track
            .GroupBy(track => track.Album.Title)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Count()
            );
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves the top three albums with the most tracks.
    public async Task<List<AlbumStatDto>> TopThreeAlbumsMostTracks()
    {
        var result = await _context.Track
            .GroupBy(track => track.Album)
            .Select(group => new AlbumStatDto
            {
                Title = group.Key.Title,
                TrackCount = group.Count()
            })
            .OrderByDescending(dto => dto.TrackCount)
            .Take(3)
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Groups tracks by composer.
    public async Task<Dictionary<string, List<Track>>> TracksByComposer()
    {
        var result = await _context.Track
            .GroupBy(track => track.Composer)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.ToList()
            );
        await _context.SaveChangesAsync();
        return result;
    }

    /// Lists each composer and the total number of tracks they have composed.
    public async Task<List<ComposerStatDto>> ComposersAndTracks()
    {
        var result = await _context.Track
            .GroupBy(track => track.Composer)
            .Select(track => new ComposerStatDto
            {
                Name = track.Composer,
                TrackCount = track.Count()
            })
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves all tracks that belong to a specified genre.
    public async Task<List<Track>> GetTracksByGenreAsync(string genreName)
    {
        var result = await _context.Track
            .Where(track => track.Genre.Name == genreName)
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves tracks longer than a given duration in seconds.
    public async Task<List<Track>> GetTracksLongerThanAsync(int seconds)
    {
        var result = await _context.Track
            .Where(track => (track.Milliseconds * 1000) > seconds)
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves the top five most expensive tracks.
    public async Task<List<TrackStatDto>> FiveMostExpensiveTracks()
    {
        var result = await _context.Track
            .OrderByDescending(track => track.UnitPrice)
            .Take(5)
            .Select(track => new TrackStatDto
            {
                Name = track.Name,
                Price = track.UnitPrice,
                AlbumTitle = track.Album.Title
            })
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves a list of customers along with the total amount they have spent and their invoice count.
    public async Task<List<CustomerTransactionSummaryDto>> CustomersAndAmountSpent()
    {
        var result = await _context.InvoiceLine
            .Select(invoiceLine => new CustomerTransactionSummaryDto
            {
                Id = invoiceLine.Invoice.Customer.CustomerId,
                FirstName = invoiceLine.Invoice.Customer.FirstName,
                LastName = invoiceLine.Invoice.Customer.LastName,
                TransactionTotal = invoiceLine.Invoice.Total,
                TransactionCount = invoiceLine.Quantity
            })
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves a list of customers with more than five purchases, including total spent and purchase count.
    public async Task<List<CustomerTransactionSummaryDto>> CustomersToalPurchaseAmounts()
    {
        var result = await _context.InvoiceLine
            .Select(invoiceLine => new CustomerTransactionSummaryDto
            {
                Id = invoiceLine.Invoice.Customer.CustomerId,
                FirstName = invoiceLine.Invoice.Customer.FirstName,
                LastName = invoiceLine.Invoice.Customer.LastName,
                TransactionTotal = invoiceLine.Invoice.Sum(invoice => invoice.Total),
                TransactionCount = invoiceLine.Quantity
            })
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves a list of customers who have made more than five purchases.
    public async Task<List<CustomerTransactionSummaryDto>> CustomersWithMoreThanFivePurchases()
    {
        var result = await _context.Customer
            .Include(customer => customer.Invoices)
            .Where(customer => customer.Invoices.Count() > 5)
            .Select(customer => new CustomerTransactionSummaryDto
            {
                Id = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                TransactionTotal = customer.Invoices.Sum(invoice => invoice.Total),
                TransactionCount = customer.Invoices.Count()
            })
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Groups invoices by customer.
    public async Task<Dictionary<int, List<Invoice>>> InvoicesGroupedByCustomerAsync()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.CustomerId)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.ToList()
            );
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves the top N customers based on total spending.
    public async Task<List<CustomerTransactionSummaryDto>> GetTopCustomersBySpendingAsync(int count)
    {
        var result = await _context.Invoice
            .Select(invoice => new CustomerTransactionSummaryDto
            {
                Id = invoice.CustomerId,
                FirstName = invoice.Customer.FirstName,
                LastName = invoice.Customer.LastName,
                TransactionTotal = invoice.Sum(invoice => invoice.Total),
                TransactionCount = invoice.Count()
            })
            .OrderByDescending(dto => dto.TransactionTotal)
            .Take(count)
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Calculates total revenue by billing country.
    public async Task<Dictionary<string, decimal>> RevenueByCountryAsync()
    {
        var result = await _context.Invoice
            .GroupBy(invoice => invoice.BillingCountry)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Sum(invoice => invoice.Total)
            );
        await _context.SaveChangesAsync();
        return result;
    }

    /// Calculates the total number of transactions and the total revenue by country.
    public async Task<List<CountryTransactionSummaryDto>> TotalPurchasesByCountry()
    {
        var result = await _context.Invoice
            .Select(invoice => new CountryTransactionSummaryDto
            {
                Name = invoice.BillingCountry,
                TransactionCount = invoice.InvoiceId.Count(),
                TransactionTotal = invoice.Sum(invoice => invoice.Total)
            })
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves all customers located in the specified country.
    public async Task<List<Customer>> GetCustomersByCountryAsync(string country)
    {
        var result = await _context.Customer
            .Where(customer => customer.BillingCountry == country)
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }

    /// Retrieves all invoices within the specified date range.
    public async Task<List<Invoice>> GetInvoicesInDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var result = await _context.Invoice
            .Where(invoice => invoice.DateTime >= startDate && invoice.DateTime <= endDate)
            .ToListAsync();
        await _context.SaveChangesAsync();
        return result;
    }
}