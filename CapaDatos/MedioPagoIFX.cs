using CapaEntidad;
using IBM.Data.Informix;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class MedioPagoIFX
    {
        public static string ConexionInformix()
        {
            try
            {
                string conexion = ConfigurationSettings.AppSettings["ConnectionStringIFX"].ToString();
                return conexion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en parametros de cadena de conexion de aplicacion: " + ex.Message);
            }
        }
        public string error { get; set; }
        //MEDIOS DE PAGO
        public DataTable ObtenerMediosPago(InputMedioPagoEn oMediosPago)
        {
            string consultaIfx = "select camcancta numero_cuenta,1 tipo_cuenta,camcacmon moneda " +
                                "from camca " +
                                "where camcastat = 1 and camcacage = " + oMediosPago.CodigoCliente + "";
            IfxConnection conn = new IfxConnection(ConexionInformix());
            try
            {
                DataTable Consulta = new DataTable();
                conn.Open();
                IfxDataAdapter resp1 = new IfxDataAdapter(consultaIfx, conn);
                resp1.Fill(Consulta);

                return Consulta;
            }
            catch (IfxException ex)
            {
                this.error = ex.ToString();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
