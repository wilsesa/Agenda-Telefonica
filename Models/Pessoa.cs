using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaTelefonica.Models
{
    public class Pessoa
    {
        public int PessoaId { get; set; }
        
        [Required(ErrorMessage ="{0} é obrigátorio")]
        [StringLength(40, ErrorMessage ="Use menos carateres")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "{0} é obrigátorio")]
        [StringLength(40, ErrorMessage ="Use menos carateres")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "{0} é obrigátorio")]
        [Range(18, int.MaxValue, ErrorMessage ="Idade inválida")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "{0} é obrigátorio")]
        [StringLength(40, ErrorMessage ="Use menos carateres")]
        [EmailAddress(ErrorMessage ="Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="{0} é obrigatorio")]

        public string Telefone { get; set; }

        [Required(ErrorMessage ="{0} é obrigatorio")]
        public TipoTelefone TipoTelefone { get; set; }

        public string Foto { get; set; }
    }

    public enum TipoTelefone
    {
        Pessoal, Comercial
    }
}
