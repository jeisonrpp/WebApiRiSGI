namespace WebApiRiSGI.Models
{
    public partial class ActivosView
    {

        public string ActivoPrincipal { get; set; }

        public string? ActivoSecundario { get; set; }

        public string Serial { get; set; }

        public string TipoNombre { get; set; }

        public string Descripcion { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Localidad { get; set; }

        public string Organo { get; set; }

        public string DepartamentoNombre { get; set; }

        public string AreaNombre { get; set; }

        public string DisplayName { get; set; }

        public DateTime FechaAdquisicion { get; set; }
    }
}
