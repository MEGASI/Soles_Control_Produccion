﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Input;




namespace CrtProduccion.entidades
{
    class dmColor
    {
        #region Atributos

        public int fld_oldidColor = 0;
        public int fld_idColor { get; set; }
        public int fld_ValorRGB { get; set; }
        public string fld_Descripcion { get; set; }
        public string errormsg = "";

        #endregion

        #region Constructores
        public dmColor()
        {
            limpiar();
        }
        public dmColor(
            byte pidColor,
            int fld_idColor,
             String PValorRGB,
             String PDescripcion)

        {
            fld_idColor = pidColor;
            fld_Descripcion = PDescripcion;
        }
        #endregion

        #region Métodos y funciones
        /// <summary>
        /// <para>Inicializa cada una de las propiedades de la clase.</para>
        /// </summary>
        public void limpiar()
        {
            fld_idColor = 0;
            fld_ValorRGB = 0;
            fld_Descripcion = "";

        }

        /// <summary>
        /// <para>Validar las propiedades antes de guardarla, si se detecta algun error
        /// El mensage del error es retornado en la propiedad errormsg.</para>
        /// </summary>
        /// <returns>true : cuando no se encuentran errores y false cuando se encuentran errores.</returns>
        public bool validar()
        {
            bool lret = true;

            if (lret && fld_Descripcion.Equals(""))
            {
                errormsg = "Nombre de Color no puede estar vacío.";
                lret = false;
            }


            return lret;
        }


      
    
        /// <summary>
        /// <para>CRUD  -- C = Create</para> 
        /// <para>Método que inserta los datos en la tabla segGrupo</para>
        /// </summary>
        /// <returns>El Numero de identificación generado, cero cuando no logra insertar el registro.</returns>
        public int crearDatos()
        {
            fld_idColor = 0;

            if (datamanager.ConexionAbrir())
            {

                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("Insert into color(Descripcion,valorRGB)" +
                                                " output INSERTED.idColor" +
                                                " Values(@Descripcion,@valorRGB)", datamanager.ConexionSQL);


                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@valorRGB", Convert.ToInt32(fld_ValorRGB));

                // Ejecutamos consulta de Actualización
                // y Retornamos el idColor Insertado.
                fld_idColor = (byte)cmd.ExecuteScalar();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            // si no logra insertar nada el idColor Retornado es Cero
            return fld_idColor;
        }


        /// <summary>
        /// <para>CRUD  -- R = Read</para>
        ///  Lee los datos extraido de la tabla segGrupo.
        /// </summary>
        /// <param name="dr">Objeto SqlDataReader que contiene los datos extraido de la tabla.</param>
        /// <param name="asignar">true para asignar los campos del registro leido a las propiedades.</param>
        /// <returns>True : cuando el dato existe, false cuando no existe.</returns>
        public bool leerDatos(SqlDataReader dr, bool asignar)
        {

            bool encontrado = false;
            if (dr.Read())
            {
                encontrado = true;
                if (asignar)
                {
                    fld_idColor = (int)dr["idColor"];
                    fld_Descripcion = dr["Descripcion"].ToString();
                    fld_ValorRGB = (int)dr["valorRGB"];
                }
            }
            else
            {
                if (asignar) limpiar();
            }

            return encontrado;
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el Nombre del usuario.
        /// </summary>
        /// <param name="pNombre"> Nombre único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(String pNombre, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idColor, Descripcion,valorRGB" +
                                               " from color" +
                                               " where Descripcion = '" + pNombre + "'");
            return leerDatos(dr, asignar);
        }
        /// <summary>
        ///  Buscar en la tabla de segGrupo por el idColor
        /// </summary>
        /// <param name="idColor"> código único que identifica el grupo.</param>
        /// <param name="asignar"> true = Asigna los campos de la tabla a las propiedadades, false = no los asigna.</param>
        /// <returns>true : si lo encuentra y false cuando no lo encuentra.</returns>
        public bool buscar(int idColor, bool asignar)
        {
            var dr = datamanager.ConsultaLeer("select idColor, Descripcion,valorRGB" +
                                               " from color" +
                                               " where idColor = " + idColor.ToString());
            return leerDatos(dr, asignar);
        }

        /// <summary>
        /// Lee el último registro insertado en la tabla segGrupo.
        /// </summary>
        /// <returns>true cuando existe por lo menos un registro en la tabla segGrupo</returns>
        public bool buscarUltimo()
        {
            var dr = datamanager.ConsultaLeer("select top 1 idColor, Descripcion,valorRGB" +
                                               " from color" +
                                               " order by idColor desc ");
            return leerDatos(dr, true);
        }

        /// <summary>
        /// <para>CRUD  -- U = Update</para> 
        /// <para>Método que actualiza los datos de la tabla segGrupo</para>
        /// </summary>
        /// <returns>True cuando logra actualizar los datos.</returns>
        public bool actualizarDatos()
        {
            int lRet = 0;

            if (datamanager.ConexionAbrir())
            {
          
                // Preparamos consulta pra la actualización
                SqlCommand cmd = new SqlCommand("update color" +
                                                " Set Descripcion = @Descripcion,valorRGB=@valorRGB" +
                                                " Where idColor = @idColor ", datamanager.ConexionSQL);

                // Ponemos valores a los Parametros incluidos en la consulta de actualización
                cmd.Parameters.AddWithValue("@idColor",Convert.ToByte( fld_idColor));
                cmd.Parameters.AddWithValue("@Descripcion", fld_Descripcion);
                cmd.Parameters.AddWithValue("@valorRGB", fld_ValorRGB);


                // Ejecutamos consulta de Actualización
                lRet = cmd.ExecuteNonQuery();

                // Cerramos conexión.
                datamanager.ConexionCerrar();

            }
            return lRet > 0;
        }

        /// <summary>
        /// <para>CRUD -- D = Delete</para> 
        /// <para>Método que elimina un registro de la tabla segGrupo</para>
        /// </summary>
        /// <returns>True cuando logra eliminar el registro.</returns>
        public bool borrarDatos(int pidColor)
        {
            // Intentamos Borrarlo
            bool lret = datamanager.ConsultaNodata("delete " +
                                               " from color" +
                                               " where idColor = " + pidColor.ToString());

            // Si logramos borrarlo limpiamos 
            if (lret) limpiar();
            // Retornamos true si lo Borra y false de No poder hacerlo.
            return lret;
        }

        #endregion
    }
}






