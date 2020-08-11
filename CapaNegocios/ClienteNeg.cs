
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
using log4net;

namespace CapaNegocios
{
    public class ClienteNeg
    {
        private string _CodigoRetorno = "";
        private string _DescripcionRetorno = "";
        private DateTime dFechaHoraServidor = DateTime.Now;
        private string[,] aAuditoriaDetalleSalida = new string[32, 3];
        private string[,] aAuditoriaDetalleEntrada = new string[3, 3];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                            CODIGO_CLIENTE = item["codigo_cliente"].ToString().Trim(),
                            TIPO_PERSONA = item["tipo_persona"].ToString().Trim(),
                            TIPO_DOCUMENTO = Convert.ToInt32(item["tipo_documento"].ToString().Trim()),
                            LUGAR_EXPEDICION = item["lugar_expedicion"].ToString().Trim(),
                            NUMERO_DOCUMENTO_IDENTIFICACION = item["numero_documento_identificacion"].ToString().Trim(),
                            COMPLEMENTO = item["complemento"].ToString().Trim(),
                            PRIMER_NOMBRE_CLIENTE = item["primer_nombre_cliente"].ToString().Trim(),
                            SEGUNDO_NOMBRE_CLIENTE = item["segundo_nombre_cliente"].ToString().Trim(),
                            APELLIDO_PATERNO_CLIENTE = item["apellido_paterno_cliente"].ToString().Trim(),
                            APELLIDO_MATERNO_CLIENTE = item["apellido_materno_cliente"].ToString().Trim(),
                            FECHA_NACIMIENTO = Convert.ToInt32(item["fecha_nacimiento"].ToString().Trim()),
                            GENERO = item["genero"].ToString().Trim(),
                            NACIONALIDAD = Convert.ToInt32(item["nacionalidad"].ToString().Trim()),
                            ESTADO_CIVIL = Convert.ToInt32(item["estado_civil"].ToString().Trim()),
                            NOMBRE_CONYUGUE = item["nombre_conyugue"].ToString().Trim(),
                            APELLIDO_CONYUGUE = item["apellido_conyugue"].ToString().Trim(),
                            CORREO_ELECTRONICO = item["correo_electronico"].ToString().Trim(),
                            TELEFONO = item["telefono"].ToString().Trim(),
                            CELULAR = item["celular"].ToString().Trim(),
                            PAIS_DOMICILIO = Convert.ToInt32(item["pais_domicilio"].ToString().Trim()),
                            DEPARTAMENTO_DOMICILIO = item["departamento_domicilio"].ToString().Trim(),
                            PROVINCIA_DOMICILIO = item["provincia_domicilio"].ToString().Trim(),
                            ZONA_DOMICILIO = item["zona_domicilio"].ToString().Trim(),
                            CIUDAD_DOMICILIO = item["ciudad_domicilio"].ToString().Trim(),
                            DIRECCION_DOMICILIO = item["direccion_domicilio"].ToString().Trim(),
                            RAZON_SOCIAL_DOMICILIO = item["razon_social_denominacion"].ToString().Trim(),
                            NRO_MATRICULA_REGISTRO_COMERCIO = item["nro_matricula_registro_comercio"].ToString().Trim(),
                            LUGAR_PAIS_CONSTITUCION = Convert.ToInt32(item["lugar_pais_constitucion"].ToString().Trim()),
                            NOMBRE_REPRESENTANTE_LEGAL = item["nombre_representante_legal"].ToString().Trim(),
                            NOMBRE_RESPONSABLE_SEGUROS = item["nombre_responsable_seguros"].ToString().Trim(),
                            CODIGO_RETORNO = _CodigoRetorno,
                            DESCRIPCION_RETORNO = _DescripcionRetorno
                        };
                    }
                }
                else
                {
                    enCliente = new OutputClienteEn
                    {
                        CODIGO_RETORNO = _CodigoRetorno,
                        DESCRIPCION_RETORNO = _DescripcionRetorno
                    };
                }
                //REGISTRO AUDITORIA
                using (TransactionScope ts = new TransactionScope())
                {
                    //OBTENER FECHA HORA SERVIDOR
                    PrepararFechaHoraServidor();
                    //AUDITORIA ENTRADA
                    Int64 iCodigoAuditoria = 0;
                    EnAudiResumen = PrepararAuditoriaResumen(entradaEnCliente, "TRAER CLIENTE ENTRADA");
                    iCodigoAuditoria = NeAudiResumen.FN_NeRegistrarProcesoResumen(EnAudiResumen);
                    if (iCodigoAuditoria == 0)
                    {
                        Log.Debug("No se registró datos de ingreso, proceso resumen de auditoria");
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
                            Log.Debug("No se registró datos proceso detalle ingreso de auditoria");
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
                        Log.Debug("No se registró datos proceso resumen salida de auditoria");
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
                            Log.Debug("No se registró datos proceso detalle salida de auditoria");
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
                    CODIGO_RETORNO = "102",
                    DESCRIPCION_RETORNO = "Error en retorno excepcion "
                };
                Log.Debug("Error: " + ex);
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
                Log.Debug("Error: " + ex);
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
                enAudiResumen.CodigoUsuario = enClienteEntrada.CODIGO_USUARIO;
                enAudiResumen.CodigoOpcion = CodigoOpcion;
                enAudiResumen.FechaProceso = Convert.ToDateTime(dFechaHoraServidor.ToShortDateString());
                enAudiResumen.HoraProceso = dFechaHoraServidor;
                enAudiResumen.CodigoMetodo = "CONSULTAR";

                return enAudiResumen;
            }
            catch (Exception ex)
            {
                Log.Debug("Error: " + ex);
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
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.TIPO_DOCUMENTO.ToString();
                iFila = iFila + 1;


                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "DOCUMENTO";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.NUMERO_DOCUMENTO;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "CODIGO USUARIO";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.CODIGO_USUARIO;
            }
            else
            {
                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.TIPO_DOCUMENTO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.NUMERO_DOCUMENTO;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna] = enClienteEntrada.CODIGO_USUARIO;
            }
        }
        //PREPARAR AUDITORIA DETALLE SALIDA
        private void PrepararAuditoriaDetalleSalida(OutputClienteEn enClienteSalida, int iColumna)
        {
            int iFila = 0;
            if (iColumna == 1)
            {
                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CODIGO CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CODIGO_CLIENTE;
                iFila = iFila + 1;


                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "TIPO PERSONA";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TIPO_PERSONA;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "TIPO DOCUMENTO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TIPO_DOCUMENTO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "LUGAR EXPEDICION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LUGAR_EXPEDICION;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NUMERO DOCUMENTO IDENTIFICACION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NUMERO_DOCUMENTO_IDENTIFICACION;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "COMPLEMENTO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.COMPLEMENTO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "PRIMER NOMBRE CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PRIMER_NOMBRE_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "SEGUNDO NOMBRE CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.SEGUNDO_NOMBRE_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "APELLIDO PATERNO CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.APELLIDO_PATERNO_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "APELLIDO MATERNO CLIENTE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.APELLIDO_MATERNO_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "FECHA NACIMIENTO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.FECHA_NACIMIENTO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "GENERO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.GENERO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NACIONALIDAD";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NACIONALIDAD.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "ESTADO CIVIL";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ESTADO_CIVIL.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NOMBRE CONYUGUE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NOMBRE_CONYUGUE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "APELLIDO CONYUGUE";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.APELLIDO_CONYUGUE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CORREO ELECTRONICO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CORREO_ELECTRONICO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "TELEFONO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TELEFONO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CELULAR";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CELULAR;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "PAIS DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PAIS_DOMICILIO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DEPARTAMENTO DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DEPARTAMENTO_DOMICILIO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "PROVINCIA DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PROVINCIA_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "ZONA DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ZONA_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CIUDAD DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CIUDAD_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DIRECCION DOMICILIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DIRECCION_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "RAZON SOCIAL DENOMINACION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.RAZON_SOCIAL_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NRO MATRICULA REGISTRO COMERCIO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NRO_MATRICULA_REGISTRO_COMERCIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "LUGAR PAIS CONSTITUCION";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LUGAR_PAIS_CONSTITUCION.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NOMBRE REPRESENTANTE LEGAL";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NOMBRE_REPRESENTANTE_LEGAL;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "NOMBRE RESPONSABLE SEGUROS";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NOMBRE_RESPONSABLE_SEGUROS;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CODIGO RETORNO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CODIGO_RETORNO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DESCRIPCION RETORNO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DESCRIPCION_RETORNO;
            }
            else
            {
                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CODIGO_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TIPO_PERSONA;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TIPO_DOCUMENTO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LUGAR_EXPEDICION;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NUMERO_DOCUMENTO_IDENTIFICACION;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.COMPLEMENTO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PRIMER_NOMBRE_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.SEGUNDO_NOMBRE_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.APELLIDO_PATERNO_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.APELLIDO_MATERNO_CLIENTE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.FECHA_NACIMIENTO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.GENERO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NACIONALIDAD.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ESTADO_CIVIL.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NOMBRE_CONYUGUE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.APELLIDO_CONYUGUE;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CORREO_ELECTRONICO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.TELEFONO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CELULAR;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PAIS_DOMICILIO.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DEPARTAMENTO_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.PROVINCIA_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.ZONA_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CIUDAD_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DIRECCION_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.RAZON_SOCIAL_DOMICILIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NRO_MATRICULA_REGISTRO_COMERCIO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.LUGAR_PAIS_CONSTITUCION.ToString();
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NOMBRE_REPRESENTANTE_LEGAL;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.NOMBRE_RESPONSABLE_SEGUROS;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.CODIGO_RETORNO;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enClienteSalida.DESCRIPCION_RETORNO;
            }
        }
    }
}
