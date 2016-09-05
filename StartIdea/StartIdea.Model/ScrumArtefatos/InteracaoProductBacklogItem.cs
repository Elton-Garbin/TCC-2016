using StartIdea.Model.TimeScrum;
using System;

namespace StartIdea.Model.ScrumArtefatos
{
    public enum TipoInteracao
    {
        StoryPoint = 1,
        Mensagem = 2
    }

    public class InteracaoProductBacklogItem
    {
        public InteracaoProductBacklogItem()
        {
            DataInteracao = DateTime.Now;
        }

        public int Id { get; set; }
        public TipoInteracao TipoInteracao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInteracao { get; set; }
        public int ProductBacklogItemId { get; set; }
        public int? MembroTimeId { get; set; }

        public virtual ProductBacklogItem ProductBacklogItem { get; set; }
        public virtual MembroTime MembroTime { get; set; }
    }
}