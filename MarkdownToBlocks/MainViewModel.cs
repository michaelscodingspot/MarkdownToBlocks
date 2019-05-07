using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkdownToBlocks
{
    public class MainViewModel : BaseViewModel
    {
        private Transformer _transformer = new Transformer();

        private bool _changeCodeBlocks = true;
        public bool ChangeCodeBlocks
        {
            get { return _changeCodeBlocks; }
            set
            {
                _changeCodeBlocks = value;
                OnPropertyChanged();
            }
        }

        private bool _changeImageSource = true;

        public bool ChangeImageSource
        {
            get { return _changeImageSource; }
            set
            {
                _changeImageSource = value; 
                OnPropertyChanged();
            }
        }

        private bool _changeImageAltLikeName = true;

        public bool ChangeImageAltLikeName
        {
            get { return _changeImageAltLikeName; }
            set
            {
                _changeImageAltLikeName = value;
                OnPropertyChanged();
            }
        }

        private bool _replaceSpecialCharacters = true;

        public bool ReplaceSpecialCharacters
        {
            get { return _replaceSpecialCharacters; }
            set { _replaceSpecialCharacters = value; }
        }

        private string _inputText;

        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        private string _outputText;

        public string OutputText
        {
            get { return _outputText; }
            set
            {
                _outputText = value;
                OnPropertyChanged();
            }
        }

        private ICommand _transformCommand = null;

        public ICommand TransformCommand
        {
            get
            {
                if (_transformCommand == null)
                {
                    _transformCommand = new DelegateCommand(() =>
                    {
                        OutputText = _transformer.Transform(InputText, ChangeCodeBlocks, ChangeImageSource, ChangeImageAltLikeName, ReplaceSpecialCharacters,
                            DateTime.Now);
                    });
                }

                return _transformCommand;
            }
        }


    }
}
