using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class BandejaDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }
    }

    public class BandejaDTODocumento
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public List<Expediente> bandejadocumento { get; set; }

    }

    public class BandejaDTODocumentoExpediente
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public Expediente bandejadocumento { get; set; }
    }

    public class BandejaExpedienteDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public ExpedienteDocumentoDTO bandejadocumento { get; set; }
    }

    public class BandejaExpDocDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public ExpedienteDocumentoDTO bandejadocumento { get; set; }

        public List<Documento> documentosobj { get; set; }
    }

    public class BandejaExpDocUndDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        public BandejaDocumento bandejasalida { get; set; }

        public ExpedienteDocumentoDTO bandejadocumento { get; set; }

        public Documento documentosobj { get; set; }
    }

    public class BandejaExpDocGroupDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public List<Documento> documentosobj { get; set; } = new List<Documento>();
        public BandejaDocumento bandejasalida { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string idbandeja { get; set; }
    }

    public class BandejaDTOR
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public Documento documento { get; set; } = new Documento();
        public List<Documento> documentosobj { get; set; } = new List<Documento>();
        public List<BandejaDocumento> bandejaentrada { get; set; } = new List<BandejaDocumento>();
        [BsonRepresentation(BsonType.ObjectId)]
        public string idbandeja { get; set; }

    }

    public class BandejaRPDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string idbandeja { get; set; }

        public List<BandejaDocumento> bandejaentrada { get; set; } = new List<BandejaDocumento>();

        public ExpedienteBandejaDTO expedientesalida { get; set; }
    }

    public class BandejaRV1DTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public List<BandejaDocumento> bandejaentrada { get; set; } = new List<BandejaDocumento>();
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; } = new List<ExpedienteBandejaDTO>();
    }

    public class ExpedienteBandejaDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string idexpediente { get; set; }
        public Cliente cliente { get; set; }
        public DocumentoDTO documento { get; set; }
        public List<DocumentoDTO> documentosobj { get; set; }
        public string tipo { get; set; }
    }

    public class BandejaDTOE
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; } = new List<ExpedienteBandejaDTO>();
    }

    public class BandejaDTOEDocumento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; } = new List<ExpedienteBandejaDTO>();
        public List<Expediente> bandejadocumento { get; set; }
    }

    public class BandejaDTOEDocumentoExpediente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
            = new List<ExpedienteBandejaDTO>();

        public Expediente bandejadocumento { get; set; }
    }

    public class BandejaExpedienteDTOE
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
            = new List<ExpedienteBandejaDTO>();

        public ExpedienteDocumentoDTO bandejadocumento { get; set; }
    }

    public class BandejaExpDocDTOE
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
            = new List<ExpedienteBandejaDTO>();

        public ExpedienteDocumentoDTO bandejadocumento { get; set; }

        public List<Documento> documentosobj { get; set; }
    }

    public class BandejaExpDocUndDTOE
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
            = new List<ExpedienteBandejaDTO>();

        public ExpedienteDocumentoDTO bandejadocumento { get; set; }

        public Documento documentosobj { get; set; }
    }

    public class BandejaExpDocGroupDTOE
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string tipo { get; set; }

        public Cliente cliente { get; set; }

        public List<Documento> documentosobj { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string idbandeja { get; set; }

        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
           = new List<ExpedienteBandejaDTO>();

        public BandejaDocumento bandejaentrada { get; set; }
    }

    public class BandejaESDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
          = new List<ExpedienteBandejaDTO>();
        public BandejaDocumento bandejaentrada { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string idbandeja { get; set; }
        public List<Documento> documentosobj { get; set; }
        public string tipo { get; set; }
        public Cliente cliente { get; set; }
        public Documento documento { get; set; }
    }

    public class BandejaESDTOP
    {
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
          = new List<ExpedienteBandejaDTO>();
        [BsonRepresentation(BsonType.ObjectId)]
        public string idbandeja { get; set; }
        public ExpedienteBandejaDTO expedienteentrada { get; set; }
    }
    
    public class BandejaESDTOR
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public List<ExpedienteBandejaDTO> expedientesalida { get; set; }
          = new List<ExpedienteBandejaDTO>();
        public List<ExpedienteBandejaDTO> expedienteentrada { get; set; }
          = new List<ExpedienteBandejaDTO>();
    }

    #region BandejaEntradaDTO

    public class BandejaEntradaDTO
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<BandejaDocumento> bandejasalida { get; set; }
    }

    public class BandejaEntradaDTODocumento
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<BandejaDocumento> bandejasalida { get; set; }
        public List<Expediente> bandejadocumento { get; set; }

    }

    public class BandejaEntradaDTODocumentoExpediente
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string tipo { get; set; }
        public string usuario { get; set; }
        public BandejaDocumento bandejaentrada { get; set; }
        public List<BandejaDocumento> bandejasalida { get; set; }

        public Expediente bandejadocumento { get; set; }
    }

    public class BandejaEntradaDTOR
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public BandejaDocumento bandejaentrada { get; set; } = new BandejaDocumento();
        public DocumentoExpediente documento { get; set; } = new DocumentoExpediente();
        public string tipoexpediente { get; set; }
        public Cliente cliente { get; set; } = new Cliente();
    }
    #endregion
}
