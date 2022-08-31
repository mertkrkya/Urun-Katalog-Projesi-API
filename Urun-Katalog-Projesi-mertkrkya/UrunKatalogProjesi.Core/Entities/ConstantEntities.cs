using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrunKatalogProjesi.Data.Entities
{
    public enum UserStatuses : int
    {
        Active = 1,
        Block = 2
    }
    public enum EmailTypes : int
    {
        Welcome = 1,
        Block = 2,
        Sold = 3,
        UnOffer = 4,
        AcceptOffer = 5,
        RejectOffer = 6
    }
    public enum CategoryStatuses : int
    {
        Active = 1,
        Delete = 2
    }
    public enum OfferStatuses : int
    {
        Wait = 1,
        Accept = 2,
        Reject = 3,
        Cancel = 4
    }
    public enum ProductStatuses : int
    {
        New = 1,
        Excellent = 2,
        Good = 3,
        Medium = 4,
        Bad = 5
    }
}
