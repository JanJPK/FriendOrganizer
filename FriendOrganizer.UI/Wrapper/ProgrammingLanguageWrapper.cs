using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrapper
{
    public class ProgrammingLanguageWrapper : ModelWrapper<ProgrammingLanguage>
    {
        #region Constructors and Destructors

        public ProgrammingLanguageWrapper(ProgrammingLanguage model) : base(model)
        {
        }

        #endregion

        #region Public Properties

        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion
    }
}