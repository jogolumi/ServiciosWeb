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
    public class MedioPagoNeg
    {
        private string _CodigoRetorno = "";
        private string _DescripcionRetorno = "";
        private DateTime dFechaHoraServidor = DateTime.Now;
        private string[,] aAuditoriaDetalleSalida = new string[3, 3];
        private string[,] aAuditoriaDetalleEntrada = new string[2, 3];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public OutputMedioPagoEn DatosMedioPago(InputMedioPagoEn entradaEnMedioPago)
        {
            OutputMedioPagoEn enMedioPago = new OutputMedioPagoEn();
            MediosPagoEn enListaMedioPago = new MediosPagoEn();
            MedioPagoIFX daMedioPago = new MedioPagoIFX();
            enAuditoriaResumen EnAudiResumen = new enAuditoriaResumen();
            enAuditoriaDetalle EnAudiDetalle = new enAuditoriaDetalle();
            enAuditoriaTablas EnAudiTablas = new enAuditoriaTablas();
            neAuditoriaResumen NeAudiResumen = new neAuditoriaResumen();
            neAuditoriaDetalle NeAudiDetalle = new neAuditoriaDetalle();
            neAuditoriaTablas NeAudiTablas = new neAuditoriaTablas();
            DataTable dt = new DataTable();
            int iNumeroCorrelativo = 1;
            int iNumeroCorrelativoTabla = 1;
            bool bRegistroAuditoriaDetalleEntrada = false;
            bool bRegistroAuditoriaDetalleSalida = false;
            bool bRegistroAuditoriaTablas = false;
            dt = daMedioPago.ObtenerMediosPago(entradaEnMedioPago);
            //VALIDAR MEDIO PAGO
            if (ValidarMedioPago(dt))
            {
                List<MediosPagoEn> ListaMedioPago = new List<MediosPagoEn>();
                foreach (DataRow item in dt.Rows)
                {
                    enListaMedioPago = new MediosPagoEn
                    {
                        NumeroCuenta = item["numero_cuenta"].ToString().Trim(),
                        TipoCuenta = item["tipo_cuenta"].ToString().Trim(),
                        Moneda = item["moneda"].ToString().Trim()
                    };
                    ListaMedioPago.Add(enListaMedioPago);
                }
                
                
                enMedioPago = new OutputMedioPagoEn
                {
                    ListaMediosPago = ListaMedioPago,
                    CodigoRetorno = _CodigoRetorno,
                    DescripcionRetorno = _DescripcionRetorno
                };
            }
            else
            {
                enMedioPago = new OutputMedioPagoEn
                {
                    CodigoRetorno = _CodigoRetorno,
                    DescripcionRetorno = _DescripcionRetorno
                };
            }
            //REGISTRO AUDITORIA
            using (TransactionScope ts = new TransactionScope())
            {
                //OBTENER FECHA HORA SERVIDOR
                PrepararFechaHoraServidor();
                //AUDITORIA ENTRADA
                Int64 iCodigoAuditoria = 0;
                EnAudiResumen = PrepararAuditoriaResumen(entradaEnMedioPago, "TRAER MEDIO PAGO ENTRADA");
                iCodigoAuditoria = NeAudiResumen.FN_NeRegistrarProcesoResumen(EnAudiResumen);
                if (iCodigoAuditoria == 0)
                {
                    Log.Debug("No se registró datos de ingreso, proceso resumen de auditoria");
                    throw new Exception();
                }
                aAuditoriaDetalleEntrada = new string[2, 3];
                PrepararAuditoriaDetalleEntrada(entradaEnMedioPago, 1);
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
                EnAudiResumen = PrepararAuditoriaResumen(entradaEnMedioPago, "TRAER MEDIO PAGO SALIDA");
                iCodigoAuditoria = NeAudiResumen.FN_NeRegistrarProcesoResumen(EnAudiResumen);
                if (iCodigoAuditoria == 0)
                {
                    Log.Debug("No se registró datos proceso resumen salida de auditoria");
                    throw new Exception();
                }
                aAuditoriaDetalleSalida = new string[3, 3];
                DataTable dTablaDetalle = new DataTable();
                dTablaDetalle = PrepararAuditoriaDetalleSalida(enMedioPago, dt, 1);
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

                    //REGISTRAR AUDITORIA DETALLE
                    foreach (DataRow item in dTablaDetalle.Rows)
                    {
                        if(aAuditoriaDetalleSalida[i,0].Trim() == item["NombreTabla"].ToString().Trim())
                        {
                            EnAudiTablas.CodigoAuditoria = iCodigoAuditoria;
                            EnAudiTablas.Correlativo = iNumeroCorrelativo;
                            EnAudiTablas.CodigoTabla = item["CodigoTabla"].ToString().Trim();
                            EnAudiTablas.CodigoPosicion = iNumeroCorrelativoTabla.ToString(); //item["CodigoPosicion"].ToString().Trim();
                            EnAudiTablas.Nombre = item["NombreInformacion"].ToString().Trim();
                            EnAudiTablas.Detalle = item["DetalleInformacion"].ToString().Trim();

                            bRegistroAuditoriaTablas = NeAudiTablas.FN_NeRegistrarAuditoriaTablas(EnAudiTablas);
                            if (!bRegistroAuditoriaTablas)
                            {
                                Log.Debug("No se registró datos tablas de auditoria");
                                throw new Exception();
                            }
                            iNumeroCorrelativoTabla = iNumeroCorrelativoTabla + 1;
                        }
                    }

                    iNumeroCorrelativo = iNumeroCorrelativo + 1;
                }
                if (bRegistroAuditoriaDetalleEntrada)
                {
                    if (bRegistroAuditoriaDetalleSalida)
                        ts.Complete();
                }
            }

            return enMedioPago;
        }
        //VALIDAR MEDIOS DE PAGO
        public bool ValidarMedioPago(DataTable dt)
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
            //VALIDAR NUMERO CUENTA NULL O VACIO
            foreach (DataRow item in dt.Rows)
            {
                if (string.IsNullOrEmpty(item["numero_cuenta"].ToString().Trim()))
                {
                    _CodigoRetorno = "103";
                    _DescripcionRetorno = "No existen datos para devolver";
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
        private enAuditoriaResumen PrepararAuditoriaResumen(InputMedioPagoEn enMedioPago, string CodigoOpcion)
        {
            try
            {
                enAuditoriaResumen enAudiResumen = new enAuditoriaResumen();
                //REGISTRO PISTAS AUDITORIA
                enAudiResumen.CodigoUsuario = enMedioPago.CodigoUsuario;
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
        private void PrepararAuditoriaDetalleEntrada(InputMedioPagoEn enMedioPagoEntrada, int iColumna)
        {
            int iFila = 0;
            if (iColumna == 1)
            {
                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "CODIGO CLIENTE";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enMedioPagoEntrada.CodigoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna - 1] = "CODIGO USUARIO";
                aAuditoriaDetalleEntrada[iFila, iColumna] = enMedioPagoEntrada.CodigoUsuario;
            }
            else
            {
                aAuditoriaDetalleEntrada[iFila, iColumna] = enMedioPagoEntrada.CodigoCliente;
                iFila = iFila + 1;

                aAuditoriaDetalleEntrada[iFila, iColumna] = enMedioPagoEntrada.CodigoUsuario;
            }
        }
        //PREPARAR AUDITORIA DETALLE SALIDA
        private DataTable PrepararAuditoriaDetalleSalida(OutputMedioPagoEn enMedioPagoSalida,DataTable dTablaMedioPago, int iColumna)
        {
            DataTable dTabla = new DataTable();
            int iFila = 0;
            if (iColumna == 1)
            {
                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "CODIGO RETORNO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enMedioPagoSalida.CodigoRetorno;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "DESCRIPCION RETORNO";
                aAuditoriaDetalleSalida[iFila, iColumna] = enMedioPagoSalida.DescripcionRetorno;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna - 1] = "LISTA MEDIOS DE PAGO";
                aAuditoriaDetalleSalida[iFila, iColumna] = string.Empty;
                dTabla = PrepararAuditoriaTabla(dTablaMedioPago, "LISTA MEDIOS DE PAGO", "A", 1);

            }
            else
            {
                aAuditoriaDetalleSalida[iFila, iColumna] = enMedioPagoSalida.CodigoRetorno;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = enMedioPagoSalida.DescripcionRetorno;
                iFila = iFila + 1;

                aAuditoriaDetalleSalida[iFila, iColumna] = string.Empty;
                dTabla = PrepararAuditoriaTabla(dTablaMedioPago, "LISTA MEDIOS DE PAGO", "D",2);
            }
            return dTabla;
        }
        private DataTable PrepararAuditoriaTabla(DataTable Tabla,string cNombreTabla,string cCodigoTabla,int iNumeroTabla)
        {
            //Tabla = new DataTable();
            DataTable TablaDetalle = new DataTable();
            TablaDetalle.Columns.Add("NombreTabla",typeof(string));
            TablaDetalle.Columns.Add("CodigoTabla",typeof(string));
            TablaDetalle.Columns.Add("CodigoPosicion",typeof(string));
            TablaDetalle.Columns.Add("NombreInformacion",typeof(string));
            TablaDetalle.Columns.Add("DetalleInformacion",typeof(string));
            //int iNumeroFila = 0;
            DataRow dr;
            if(iNumeroTabla == 1)
            {
                foreach (DataRow item in Tabla.Rows)
                {
                    dr = TablaDetalle.NewRow();
                    //iNumeroFila = iNumeroFila + 1;
                    dr["NombreTabla"] = cNombreTabla;
                    dr["CodigoTabla"] = cCodigoTabla;
                    dr["CodigoPosicion"] = "0";
                    dr["NombreInformacion"] = "NUMERO CUENTA";
                    dr["DetalleInformacion"] = item["numero_cuenta"].ToString().Trim();
                    TablaDetalle.Rows.Add(dr);

                    dr = TablaDetalle.NewRow();
                    //iNumeroFila = iNumeroFila + 1;
                    dr["NombreTabla"] = cNombreTabla;
                    dr["CodigoTabla"] = cCodigoTabla;
                    dr["CodigoPosicion"] = "0";
                    dr["NombreInformacion"] = "TIPO CUENTA";
                    dr["DetalleInformacion"] = item["tipo_cuenta"].ToString().Trim();
                    TablaDetalle.Rows.Add(dr);

                    dr = TablaDetalle.NewRow();
                    //iNumeroFila = iNumeroFila + 1;
                    dr["NombreTabla"] = cNombreTabla;
                    dr["CodigoTabla"] = cCodigoTabla;
                    dr["CodigoPosicion"] = "0";
                    dr["NombreInformacion"] = "MONEDA";
                    dr["DetalleInformacion"] = item["moneda"].ToString().Trim();
                    TablaDetalle.Rows.Add(dr);
                }
            }
            return TablaDetalle;
        }
    }
}
