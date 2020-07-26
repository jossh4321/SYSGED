using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class StatisticsDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public int mes { get; set; }
    }
    public class StatisticsDTOR
    {
        [BsonId]
        public string id { get; set; }
        public Int32 cantidad { get; set; }
    }

    public class StatisticsDTO4_project1
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Evaluacion evaluacion { get; set; }
        public Int32 mes { get; set; }
    }

    public class StatisticsDTO4R
    {
        [BsonId]
        public string id { get; set; }
        public Int32 aprobados { get; set; }
        public Int32 rechazados { get; set; }
    }

    public class StatisticsDTO3_project
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Int32 mes { get; set; }
    }

    public class StatisticsDTO3_group
    {
        [BsonId]
        public string id { get; set; }
        public Int32 caducados { get; set; }
    }
}
