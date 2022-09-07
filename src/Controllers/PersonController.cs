using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

using src.Models;
using src.Persistence;

namespace src.Models;

[ApiController]
[Route("[controller]")]
   public class PersonController : ControllerBase{
   public PersonController(DatabaseContext _context) {
            this._context = _context;
  }
  
     public DatabaseContext _context { get; set; }
   
     [HttpGet]
     public ActionResult<List<Pessoa>> Get() {

      var result = _context.Pessoas.Include(p => p.contratos).ToList();
      if(!result.Any()) return NoContent();
      return Ok(result);         
  }


     [HttpPost]
   public ActionResult<Pessoa> Post([FromBody]Pessoa pessoa)
   { 
    try
    {
       _context.Pessoas.Add(pessoa);
       _context.SaveChanges();
    }
    catch (System.Exception)
    {
      
     return BadRequest();
    }         
      return Created("criado",pessoa);
  }


     [HttpPut("{id}")]
   public ActionResult<Object> Update ([FromRoute]int id, [FromBody]Pessoa pessoa)
   {
    var result = _context.Pessoas.SingleOrDefault(e => e.Id == id );

    if(result is null){
      return NotFound(new{
         msg = "Registro não encontrado.",
         StatusCode = NotFound()

      });
    }
    try{
      _context.Pessoas.Update(pessoa);
      _context.SaveChanges();

    }catch(System.Exception)
    {
      return BadRequest(new{
      msg = "Houve um erro ao enviar a solicitação de atualização do {id} atualizados",
      StatusCode = Ok()
    });
    }
    
    return Ok(new
    {
      msg = $"Dados do id {id} atualizados",
      StatusCode = Ok()
    });
  }

     [HttpDelete("{id}")]
      public ActionResult<Object> Delete(int id)
      {
      var result = _context.Pessoas.SingleOrDefault(e => e.Id == id );

      if(result is null){
      return BadRequest(new {
      msg = "Conteúdo inexixtente, solicitação inválida.", 
      StatusCode = NotFound()
      });
  }

    _context.Pessoas.Remove(result);
    _context.SaveChanges();

      return Ok(new{
      msg = "deletado pessoa de Id " + id,
      StatusCode = Ok()
      });
   }
 }
