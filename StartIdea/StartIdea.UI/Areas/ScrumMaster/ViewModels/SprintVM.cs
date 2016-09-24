using StartIdea.Model.ScrumEventos;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintVM
    {
        private Sprint _ProximaSprint;

        public SprintVM()
        {
            ActionForm = "Create";
            SubmitValue = "Cadastrar";
            ProximaSprint = new Sprint();
        }

        public Sprint SprintAtual { get; set; }
        public string ActionForm { get; private set; }
        public string SubmitValue { get; private set; }

        public Sprint ProximaSprint
        {
            get { return _ProximaSprint; }
            set
            {
                _ProximaSprint = value;

                if (_ProximaSprint != null)
                {
                    SubmitValue = _ProximaSprint.Id > 0 ? "Editar" : "Cadastrar";
                    ActionForm  = _ProximaSprint.Id > 0 ? "Edit" : "Create";
                }
            }
        }
    }
}