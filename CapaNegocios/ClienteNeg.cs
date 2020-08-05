
using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class ClienteNeg
    {
        private string _CodigoRetorno = "";
        private string _DescripcionRetorno = "";
        public OutputClienteEn DatosCliente(InputClienteEn oCliente)
        {
            OutputClienteEn enCliente = new OutputClienteEn();
            try
            {
                ClienteIFX daCliente = new ClienteIFX();
                DataTable dt = new DataTable();
                using (IDataReader dr = daCliente.ObtenerCliente(oCliente))
                {
                    dt.Load(dr);
                    dr.Close();
                }
                //OBJETO NULO
                if(dt == null)
                {
                    enCliente = new OutputClienteEn
                    {
                        CodigoRetorno = "102",
                        DescripcionRetorno = "Error en retorno"
                    };
                    return enCliente;
                }
                //VALIDAR CLIENTE
                if (ValidarDatosCliente(dt))
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        enCliente = new OutputClienteEn
                        {
                            CodigoCliente = item["codigo_cliente"].ToString().Trim(),
                            TipoPersona = item["tipo_persona"].ToString().Trim(),
                            TipoDocumento = item["tipo_documento"].ToString().Trim(),
                            LugarExpedicion = item["numero_documento_expedicion"].ToString().Trim(),
                            Complemento = item["complemento"].ToString().Trim(),
                            PrimerNombreCliente = item["primer_nombre_cliente"].ToString().Trim(),
                            SegundoNombreCliente = item["segundo_nombre_cliente"].ToString().Trim(),
                            ApellidoPaternoCliente = item["apellido_paterno_cliente"].ToString().Trim(),
                            ApellidoMaternoCliente = item["apellido_materno_cliente"].ToString().Trim(),
                            FechaNacimiento = item["fecha_nacimiento"].ToString().Trim(),
                            Genero = item["genero"].ToString().Trim(),
                            Nacionalidad = item["nacionalidad"].ToString().Trim(),
                            EstadoCivil = item["estado_civil"].ToString().Trim(),
                            NombreConyugue = item["nombre_conyugue"].ToString().Trim(),
                            ApellidoConyugue = item["apellido_conyugue"].ToString().Trim(),
                            CorreoElectronico = item["correo_electronico"].ToString().Trim(),
                            Telefono = item["telefono"].ToString().Trim(),
                            Celular = item["celular"].ToString().Trim(),
                            PaisDomicilio = item["pais_domicilio"].ToString().Trim(),
                            DepartamentoDomicilio = item["departamento_domicilio"].ToString().Trim(),
                            ProvinciaDomicilio = item["provincia_domicilio"].ToString().Trim(),
                            ZonaDomicilio = item["zona_domicilio"].ToString().Trim(),
                            CiudadDomicilio = item["ciudad_domicilio"].ToString().Trim(),
                            DireccionDomicilio = item["direccion_domicilio"].ToString().Trim(),
                            RazonSocialDenominacion = item["razon_social_denominacion"].ToString().Trim(),
                            NroMatriculaRegistroComercio = item["nro_matricula_registro_comercio"].ToString().Trim(),
                            LugarPaisConstitucion = item["lugar_pais_constitucion"].ToString().Trim(),
                            NombreRepresentanteLegal = item["nombre_representante_legal"].ToString().Trim(),
                            NombreResponsableSeguros = item["nombr_responsable_seguros"].ToString().Trim(),
                            CodigoRetorno = _CodigoRetorno,
                            DescripcionRetorno = _DescripcionRetorno
                        };
                    }
                }
                else
                {
                    enCliente = new OutputClienteEn
                    {
                        CodigoRetorno = _CodigoRetorno,
                        DescripcionRetorno = _DescripcionRetorno
                    };
                }
                return enCliente;
            }
            catch (Exception ex)
            {
                enCliente = new OutputClienteEn
                {
                    CodigoRetorno = "102",
                    DescripcionRetorno = "Error en retorno"
                };
                return enCliente;
            }
        }
        private bool ValidarDatosCliente(DataTable dt)
        {
            //DATA TABLE NULL
            if(dt is null)
            {
                _CodigoRetorno = "102";
                _DescripcionRetorno = "Error en retorno";
                return false;
            }
            //DATA TABLE VACIO
            if(dt.Rows.Count == 0)
            {
                _CodigoRetorno = "103";
                _DescripcionRetorno = "No existen datos para devolver";
                return false;
            }
            //VALIDAR DUPLICIDAD EN CLIENTE
            if (dt.Rows.Count > 1)
            {
                _CodigoRetorno = "104";
                _DescripcionRetorno = "Cliente duplicado contactarse con el banco para continuar";
                return false;
            }
            //VALIDAR CLIENTE NULL O VACIO
            foreach (DataRow item in dt.Rows)
            {
                if(string.IsNullOrEmpty(item["codigo_cliente"].ToString().Trim()))
                {
                    _CodigoRetorno = "103";
                    _DescripcionRetorno = "No existen datos para devolver";
                    return false;
                }
            }
            //VALIDAR PAIS
            foreach (DataRow item in dt.Rows)
            {
                if (string.IsNullOrEmpty(item["pais_domicilio"].ToString().Trim()))
                {
                    _CodigoRetorno = "105";
                    _DescripcionRetorno = "No puede continuar, debe registrar el país del cliente";
                    return false;
                }
            }
            _CodigoRetorno = "000";
            _DescripcionRetorno = "Envío exitoso!!";
            return true;
        }
    }
}
