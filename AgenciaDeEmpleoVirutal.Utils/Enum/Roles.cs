using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Utils.Enum
{
    public enum Roles
    {
        /// <summary>
        /// oferente
        /// </summary>
        [Description("Oferente")]
        Oferente = 0,

        /// <summary>
        /// Administrador
        /// </summary>
        [Description("Administrador")]
        Administrador = 1,

        /// <summary>
        /// Supervisor_de_Agencia
        /// </summary>
        [Description("Supervisor de Agencia")]
        Supervisor_de_Agencia = 2,

        /// <summary>
        /// OrientadorLaboral
        /// </summary>
        [Description("Orientador Laboral")]
        Orientador_Laboral = 3,
        /// <summary>
        /// Analista_Revisor_FOSFEC
        /// </summary>
        [Description("Analista Revisor FOSFEC")]
        Analista_Revisor_FOSFEC = 4,
    }
}