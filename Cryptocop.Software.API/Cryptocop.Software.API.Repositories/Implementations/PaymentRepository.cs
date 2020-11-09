using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
  public class PaymentRepository : IPaymentRepository
  {
    private readonly CryptocopDbContext _dbContext;

    public PaymentRepository(CryptocopDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
    {
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      var entity = new PaymentCard
      {
        UserId = user.Id,
        CardholderName = paymentCard.CardholderName,
        CardNumber = paymentCard.CardNumber,
        Month = paymentCard.Month,
        Year = paymentCard.Year
      };
      _dbContext.PaymentCards.Add(entity);
      _dbContext.SaveChanges();
    }

    public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
    {
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      return _dbContext.PaymentCards
                .Where(c => c.UserId == user.Id)
                .Select(c => new PaymentCardDto
                {
                  Id = c.Id,
                  CardholderName = c.CardholderName,
                  CardNumber = c.CardNumber,
                  Month = c.Month,
                  Year = c.Year
                }).ToList();
    }
  }
}