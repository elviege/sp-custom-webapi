using SP.Portal.Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SP.Portal.Core.Entities
{
    [Table("[BoTR].[Requests]")]
    public class TransportRequestEntity : BaseEntity
    {
        [Column("author_id")]
        public Guid AuthorId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("departure_dt")]
        public DateTime DepartureDt { get; set; }

        [Column("arrival_dt")]
        public DateTime ArrivalDt { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("business_type_id")]
        public Guid BusinessTypeId { get; set; }

        [Column("departure_city_id")]
        public string DepartureCityId { get; set; }

        [Column("arrival_city_id")]
        public string ArrivalCityId { get; set; }

        [Column("departure_street_id")]
        public string DepartureStreetId { get; set; }

        [Column("arrival_street_id")]
        public string ArrivalStreetId { get; set; }

        [Column("departure_address")]
        public string DepartureAddress { get; set; }

        [Column("arrival_address")]
        public string ArrivalAddress { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("subdivision_id")]
        public int SubdivisionId { get; set; }

        [Column("car_type_id")]
        public int CarTypeId { get; set; }
    }
}
