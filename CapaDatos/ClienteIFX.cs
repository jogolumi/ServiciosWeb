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
    public class ClienteIFX
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
        public DataTable ObtenerCliente(InputClienteEn oCliente)
        {
            string consultaIfx = "select c.gbagecage codigo_cliente " +
                                ", c.gbagetper tipo_persona " +
                                ", c.gbagetdid tipo_documento " +
                                ", SUBSTRING (c.gbagendid from length (c.gbagendid) - 1 for 2) lugar_expedicion " +
                                ", SUBSTRING(c.gbagendid from 1 for length(c.gbagendid) - 2) numero_documento_identificacion " +
                                ", c.gbagecomp complemento, da.gbdacnom1 primer_nombre_cliente, da.gbdacnom2 segundo_nombre_cliente, da.gbdacape1 apellido_paterno_cliente " +
                                ", da.gbdacape2 apellido_materno_cliente, TO_CHAR(c.gbagefnac, '%Y%m%d') fecha_nacimiento " +
                                ", c.gbagesexo genero, con.gbconcorr nacionalidad " +
                                ", c.gbageeciv estado_civil " +
                                ", (select con.gbdacnom1 from gbdac con where con.gbdaccage = da.gbdaccony) nombre_conyugue " +
                                ", (select con.gbdacape1 from gbdac con where con.gbdaccage = da.gbdaccony) apellido_conyugue " +
                                ", da.gbdacmail correo_electronico, c.gbagetlfd telefono, da.gbdaccelu celular " +
                                ", dir.gbdirpais pais_domicilio " +
                                ", dir.gbdirdpto departamento_domicilio, dir.gbdircprv provincia_domicilio, dir.gbdirzona zona_domicilio, dir.gbdirciud ciudad_domicilio " +
                                ", CONCAT(c.gbageddo1, c.gbageddo2) direccion_domicilio " +
                                ", '' razon_social_denominacion, '' nro_matricula_registro_comercio, '' lugar_pais_constitucion, '' nombre_representante_legal, '' nombre_responsable_seguros " +
                                "from gbage c " +
                                "inner join gbdac da on da.gbdaccage = c.gbagecage " +
                                "left outer join gbdir dir on c.gbagecage = dir.gbdircage and dir.gbdirtdir = 1 " +
                                "left outer join gbcon con on c.gbagenaci = con.gbcondesc " +
                                "where trim(c.gbagendid) = '" + oCliente.Documento + "' and c.gbagetdid = '" + oCliente.TipoDocumento + "'";
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
