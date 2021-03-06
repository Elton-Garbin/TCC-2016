﻿using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StartIdea.Model.TimeScrum
{
    public class MembroTime
    {
        public MembroTime()
        {
            HistoricoEstimativas = new HashSet<HistoricoEstimativa>();
            Tarefas = new HashSet<Tarefa>();
            StatusTarefas = new HashSet<StatusTarefa>();
            IsActive = true;
            DataManutencao = DateTime.Now;
        }

        #region Fields
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Funcao { get; set; }

        public bool IsActive { get; set; }
        public DateTime DataManutencao { get; set; }
        public int UsuarioId { get; set; }
        public int TimeId { get; set; }
        #endregion

        #region References
        public virtual Usuario Usuario { get; set; }
        public virtual Time Time { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<HistoricoEstimativa> HistoricoEstimativas { get; set; }
        public virtual ICollection<Tarefa> Tarefas { get; set; }
        public virtual ICollection<StatusTarefa> StatusTarefas { get; set; }
        #endregion
    }
}
