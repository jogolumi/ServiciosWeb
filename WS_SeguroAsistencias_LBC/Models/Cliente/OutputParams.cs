using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_SeguroAsistencias_LBC.Models.Cliente
{
    public class OutputParams
    {
        public string CodigoCliente { get; set; }
        public string TipoPersona { get; set; }
        public string TipoDocumento { get; set; }
        public string LugarExpedicion { get; set; }
        public string NumeroDocumentoIdentificacion { get; set; }
        public string Complemento { get; set; }
        public string PrimerNombreCliente { get; set; }
        public string SegundoNombreCliente { get; set; }
        public string ApellidoPaternoCliente { get; set; }
        public string ApellidoMaternoCliente { get; set; }
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Nacionalidad { get; set; }
        public string EstadoCivil { get; set; }
        public string NombreConyugue { get; set; }
        public string ApellidoConyugue { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string PaisDomicilio { get; set; }
        public string DepartamentoDomicilio { get; set; }
        public string ProvinciaDomicilio { get; set; }
        public string ZonaDomicilio { get; set; }
        public string CiudadDomicilio { get; set; }
        public string DireccionDomicilio { get; set; }
        public string RazonSocialDenominacion { get; set; }
        public string NroMatriculaRegistroComercio { get; set; }
        public string LugarPaisConstitucion { get; set; }
        public string NombreRepresentanteLegal { get; set; }
        public string NombreResponsableSeguros { get; set; }
        public string CodigoRetorno { get; set; }
        public string DescripcionRetorno { get; set; }
    }
}