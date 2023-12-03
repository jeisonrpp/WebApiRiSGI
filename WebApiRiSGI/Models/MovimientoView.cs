namespace WebApiRiSGI.Models
{
    public partial class MovimientoView
    {
 
        public string Codigo { get; set; }

        public string Serial { get; set; }

        public string ActivoPrincipal { get; set; }

        public string? ActivoSecundario { get; set; }

        public string TipoActivo { get; set; }

        public string Descripcion { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string LocalidadRemitente { get; set; }

        public string OrganoRemitente { get; set; }

        public string DepartamentoRemitente { get; set; }

        public string AreaRemitente { get; set; }

        public string UsuarioRemitente { get; set; }

        public string LocalidadDestino { get; set; }

        public string OrganoDestino { get; set; }

        public string DepartamentoNombre { get; set; }

        public string AreaNombre { get; set; }

        public string UsuarioDestino { get; set; }

        public DateTime Fecha { get; set; }

        public string TipoMovimiento { get; set; }

        public string Observacion { get; set; }
    }
}
