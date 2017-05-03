using RW_backend;

namespace RW_Frontend
{
    /// <summary>
    /// Konwerter Model-ViewModel
    /// </summary>
    class ModelConverter
    {
        /// <summary>
        /// Zamień obiekt VM na równoważny obiekt Modelu
        /// </summary>
        /// <param name="vm">obiekt ViewModel</param>
        /// <returns>obiekt Modelu</returns>
        public Model ConvertToModel(VM vm)
        {
            //TODO konwersja VM-M
            return new Model();
        }

        /// <summary>
        /// Zamień obiekt Modelu na równoważny obiekt ViewModel
        /// </summary>
        /// <param name="model">obiekt Modelu</param>
        /// <returns>obiekt ViewModel</returns>
        public VM ConvertToVM(Model model)
        {
            //TODO konwersja M-VM (? - o ile będzie potrzebna)
            return new VM();
        }
    }
}
