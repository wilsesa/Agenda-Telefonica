using AgendaTelefonica.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaTelefonica.Controllers
{
    public class PessoasController : Controller
    {
        //1ro Criamos uma variavel para nosso contexto
        private readonly Contexto _contexto;
        //2do Criamos outra variavel com IWebHostEnvironment
        private readonly IWebHostEnvironment _webHostEnvironment;
        //3ro Inizializamos as variaveis utilizando injeccao de dependencia
        public PessoasController(Contexto contexto, IWebHostEnvironment webHostEnvironment)
        {
            _contexto = contexto;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Pessoas.ToListAsync());
        }

        [HttpGet]
        public IActionResult CriarPessoa()
        {
            return View();
        }

        public async Task<IActionResult> CriarPessoa(Pessoa pessoa, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string diretorio = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    string nomefoto = Guid.NewGuid().ToString() + foto.FileName;

                    using (FileStream fileStream = new FileStream(Path.Combine(diretorio, nomefoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        pessoa.Foto = "~/img/" + nomefoto;
                    }
                }
                else
                {
                    pessoa.Foto = "~/img/usuario.png";
                }

                _contexto.Add(pessoa);
                await _contexto.SaveChangesAsync();
                TempData["ContatoNovo"] = $"Contato com nome {pessoa.Nome} {pessoa.Sobrenome} incluido com sucesso";

                return RedirectToAction(nameof(Index));
            }

            return View(pessoa);
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarPessoa(int pessoaId)
        {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync(pessoaId);

            if (pessoa == null)
                return NotFound();

            TempData["Foto"] = pessoa.Foto;
            return View(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPessoa(Pessoa pessoa, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    string diretorio = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    string nomeFoto = Guid.NewGuid().ToString() + foto.FileName;
                    //Salvamos no diretorio
                    using (FileStream fileStream = new FileStream(Path.Combine(diretorio, nomeFoto), FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                        pessoa.Foto = "~/img/" + nomeFoto;
                        TempData["Foto"] = null;
                    }
                }
                else
                {
                    pessoa.Foto = TempData["Foto"].ToString();
                }
                _contexto.Update(pessoa);
                await _contexto.SaveChangesAsync();
                TempData["ContatoAtualizado"] = $"Contato com nome {pessoa.Nome} {pessoa.Sobrenome} atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        [HttpPost]
        public async Task<JsonResult> ExcluirPessoa(int pessoaId)
        {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync(pessoaId);
            _contexto.Pessoas.Remove(pessoa);
            await _contexto.SaveChangesAsync();
            TempData["ContatoExcluido"] = $"Contato com nome {pessoa.Nome} {pessoa.Sobrenome} excluido com sucesso";
            return Json(true);
        }
    }
}
