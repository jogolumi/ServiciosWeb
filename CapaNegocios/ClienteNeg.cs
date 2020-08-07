
using Auditoria.CapaEntidades;
using Auditoria.CapaNegocio;
using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CapaNegocios
{
    public class ClienteNeg
    {
        private string _CodigoRetorno = "";
        private string _DescripcionRetorno = "";
        private DateTime dFechaHoraServidor = DateTime.Now;
        private string[,] aAuditoriaDetalleSalida = new string[32, 3];
        private string[,] aAuditoriaDetalleEntrada = new string[3, 3];
        public OutputClienteEn DatosCliente(InputClienteEn entradaEnCliente)
        {
            OutputClienteEn enCliente = new OutputClienteEn();
            try
            {
                ClienteIFX daCliente = new ClienteIFX();
                enAuditoriaResumen EnAudiResumen = new enAuditoriaResumen();
                enAuditoriaDetalle EnAudiDetalle = new enAuditoriaDetalle();
                neAuditoriaResumen NeAudiResumen = new neAuditoriaResumen();
                neAuditoriaDetalle NeAudiDetalle = new neAuditoriaDetalle();
                DataTable dt = new DataTable();
                int iNumeroCorrelativo = 1;
                bool bRegistroAuditoriaDetalleEntrada = false;
                bool bRegistroAuditoriaDetalleSalida = false;
                dt = daCliente.ObtenerCliente(entradaEnCliente);
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
                            LugarExpedicion = item["lugar_expedicion"].ToString().Trim(),
                            NumeroDocumentoIdentificacion = item["numero_documento_identificacion"].ToString().Trim(),
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
                            NombreResponsableSeguros = item["nombre_responsable_seguros"].ToString().Trim(),
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
                //REGISTRO AUDITORIA
                using (TransactionScope ts = new TransactionScope())
                {
                    //AUDITORIA ENTRADA
                    Int64 iCodigoAuditoria = 0;
                    EnAudiResumen = PrepararAuditoriaResumen(entradaEnCliente, "TRAER CLIENTE ENTRADA");
                    iCodigoAuditoria = NeAudiResumen.FN_NeRegistrarProcesoResumen(EnAudiResumen);
                    if (iCodigoAuditoria == 0)
                    {
                        //agregar el .log
                        throw new Exception();
                    }
                    aAuditoriaDetalleEntrada = new string[3, 3];
                    PrepararAuditoriaDetalleEntrada(entradaEnCliente, 1);
                    for (int i = aAuditoriaDetalleEntrada.GetLowerBound(0); i <= aAuditoriaDetalleEntrada.GetUpperBound(0); i++)
                    {
                        EnAudiDetalle.CodigoAuditoria = iCodigoAuditoria;
                        EnAudiDetalle.Correlativo = iNumeroCorrelativo;
                        EnAudiDetalle.Nombre = aAuditoriaDetalleEntrada[i, 0];
                        EnAudiDetalle.DetalleAntes = aAuditoriaDetalleEntrada[i, 1];
                        if (string.IsNullOrEmpty(aAuditoriaDetalleEntrada[i, 2]))
                            EnAudiDetalle.DetalleDespues = string.Empty;
                        else
                            EnAudiDetalle.DetalleDespues = aAuditoriaDetalleEntrada[i, 2];

                        bRegistroAuditoriaDetalleEntrada = NeAudiDetalle.FN_NeRegistrarProcesoDetalle(EnAudiDetalle);

                        if (!bRegistroAuditoriaDetalleEntrada)
                        {
                            //agregar el .log
                            throw new Exception();
                        }

                        iNumeroCorrelativo = iNumeroCorrelativo + 1;
                    }

                    //AUDITORIA SALIDA
                    iCodigoAuditoria = 0;
                    EnAudiResumen = PrepararAuditoriaResumen(entradaEnCliente, "TRAER CLIENTE SALIDA");
                    iCodigoAuditoria = NeAudiResumen.FN_NeRegistrarProcesoResumen(EnAudiResumen);
                    if (iCodigoAuditoria == 0)
                    {
                        //agregar el .log
                        throw new Exception();
                    }
                    aAuditoriaDetalleSalida = new string[32, 3];
                    PrepararAuditoriaDetalleSalida(enCliente, 1);
                    for (int i = aAuditoriaDetalleSalida.GetLowerBound(0); i <= aAuditoriaDetalleSalida.GetUpperBound(0); i++)
                    {
                        EnAudiDetalle.CodigoAuditoria = iCodigoAuditoria;
                        EnAudiDetalle.Correlativo = iNumeroCorrelativo;
                        EnAudiDetalle.Nombre = aAuditoriaDetalleSalida[i, 0];
                        EnAudiDetalle.DetalleAntes = aAuditoriaDetalleSalida[i, 1];
                        if (string.IsNullOrEmpty(aAuditoriaDetalleSalida[i, 2]))
                            EnAudiDetalle.DetalleDespues = string.Empty;
                        else
                            EnAudiDetalle.DetalleDespues = aAuditoriaDetalleSalida[i, 2];

                        bRegistroAuditoriaDetalleSalida = NeAudiDetalle.FN_NeRegistrarProcesoDetalle(EnAudiDetalle);

                        if (!bRegistroAuditoriaDetalleSalida)
                        {
                            //agregar el .log
                            throw new Exception();
                        }

                        iNumeroCorrelativo = iNumeroCorrelativo + 1;
                    }
                    if (bRegistroAuditoriaDetalleEntrada)
                    {
                        if (bRegistroAuditoriaDetalleSalida)
                            ts.Complete();
                    }    
                }

                return enCliente;
            }
            catch (Exception ex)
            {
                enCliente = new OutputClienteEn
                {
                    CodigoRetorno = "102",
                    DescripcionRetorno = "Error en retorno excepcion "
                };
                return enCliente;
            }
        }
        //VALIDACION INFORMACION CLIENTE
        private bool ValidarDatosCliente(DataTable dt)
        {
            //DATA TABLE NULL
            if (dt is null)
            {
                _CodigoRetorno = "102";
                _DescripcionRetorno = "Error en retorno validación tabla null";
                return false;
            }
            //DATA TABLE VACIO
            if (dt.Rows.Count == 0)
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
                if (string.IsNullOrEmpty(item["codigo_cliente"].ToString().Trim()))
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
        //OBTENER FECHA HORA SERVIDOR
        private void PrepararFechaHoraServidor()
        {
            try
            {
                ServidorNeg neDatosServidor = new ServidorNeg();
                dFechaHoraServidor = neDatosServidor.DevolverFechaHoraServidor();
            }
            catch (Exception ex)
            {
                //agregar el .log
                throw new Exception();
            }
        }
        //PREPARAR AUDITORIA
        private enAuditoriaResumen PrepararAuditoriaResumen(InputClienteEn enClienteEntrada, string CodigoOpcion)
        {
            try
            {
                enAuditoriaResumen enAudiResumen = new enAuditoriaResumen();
                //REGISTRO PISTAS AUDITORIA
                enAudiResumen.CodigoUsuario = enClienteEntrada.CodigoUsuario;
                enAudiResumen.CodigoOpcion = CodigoOpcion;
                enAudiResumen.FechaProceso = Convert.ToDateTime(dFechaHoraServidor.ToShortDateString());
                enAudiResumen.HoraProceso = dFechaHoraServidor;
                enAudiResumen.CodigoMetodo = "CONSULTAR";

                return enAudiResumen;
            }
            catch (Exception ex)
            {
                //agregar el .log
                throw new Exception();
            }
        }
        //PREPARAR AUDITORIA DETALLE ENTRADA
        private void PrepararAuditoriaDetalleEntrada(InputClienteEn enClienteEntrada, int iColumna)
        {
            int iFila = 0;
            if (iColumna == 1)
            {
                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "TIPO DOCUMENTO";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.TipoDocumento;
                iFila = iFila + 1;


                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "DOCUMENTO";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.Documento;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "CODIGO USUARIO";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.CodigoUsuario;
            }
            else
            {
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.TipoDocumento;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.Documento;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.CodigoUsuario;
            }
        }
        //PREPARAR AUDITORIA DETALLE SALIDA
        private void PrepararAuditoriaDetalleSalida(OutputClienteEn enClienteSalida, int iColumna)
        {
            int iFila = 0;
            if (iColumna == 1)
            {
                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CODIGO CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CodigoCliente;
                iFila = iFila + 1;


                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "TIPO PERSONA";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TipoPersona;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "TIPO DOCUMENTO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TipoDocumento;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "LUGAR EXPEDICION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LugarExpedicion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NUMERO DOCUMENTO IDENTIFICACION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NumeroDocumentoIdentificacion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "COMPLEMENTO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Complemento;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "PRIMER NOMBRE CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PrimerNombreCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "SEGUNDO NOMBRE CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.SegundoNombreCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "APELLIDO PATERNO CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ApellidoPaternoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "APELLIDO MATERNO CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ApellidoMaternoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "FECHA NACIMIENTO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.FechaNacimiento;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "GENERO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Genero;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NACIONALIDAD";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Nacionalidad;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "ESTADO CIVIL";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.EstadoCivil;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NOMBRE CONYUGUE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NombreConyugue;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "APELLIDO CONYUGUE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ApellidoConyugue;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CORREO ELECTRONICO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CorreoElectronico;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "TELEFONO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Telefono;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CELULAR";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Celular;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "PAIS DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PaisDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DEPARTAMENTO DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DepartamentoDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "PROVINCIA DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ProvinciaDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "ZONA DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ZonaDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CIUDAD DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CiudadDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DIRECCION DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DireccionDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "RAZON SOCIAL DENOMINACION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.RazonSocialDenominacion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NRO MATRICULA REGISTRO COMERCIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NroMatriculaRegistroComercio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "LUGAR PAIS CONSTITUCION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LugarPaisConstitucion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NOMBRE REPRESENTANTE LEGAL";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NombreRepresentanteLegal;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NOMBRE RESPONSABLE SEGUROS";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NombreResponsableSeguros;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CODIGO RETORNO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CodigoRetorno;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DESCRIPCION RETORNO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DescripcionRetorno;
            }
            else
            {
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CodigoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TipoPersona;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TipoDocumento;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LugarExpedicion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NumeroDocumentoIdentificacion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Complemento;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PrimerNombreCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.SegundoNombreCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ApellidoPaternoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ApellidoMaternoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.FechaNacimiento;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Genero;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Nacionalidad;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.EstadoCivil;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NombreConyugue;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ApellidoConyugue;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CorreoElectronico;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Telefono;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.Celular;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PaisDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DepartamentoDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ProvinciaDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ZonaDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CiudadDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DireccionDomicilio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.RazonSocialDenominacion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NroMatriculaRegistroComercio;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LugarPaisConstitucion;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NombreRepresentanteLegal;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NombreResponsableSeguros;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CodigoRetorno;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DescripcionRetorno;
            }
        }
    }
}
