namespace StartIdea.Model.ScrumArtefatos
{
    public class TarefaStatus
    {
        public TarefaStatus()
        {

        }

        public int Id { get; set; }
        public int TarefaId { get; set; }
        public int StatusId { get; set; }
        public int MembroTimeId { get; set; }

        public virtual MembroTime MembroTime { get; set; }
        public virtual Status Status { get; set; }
        public virtual Tarefa Tarefa { get; set; }
    }
}