using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using QuanLyChamThi.Command;
using QuanLyChamThi.View;

namespace QuanLyChamThi.ViewModel
{
    class MainWindowViewModel: ViewModelBase
    {
        private MainWindowViewModel() { }

        private static MainWindowViewModel _ins;
        public static MainWindowViewModel Ins
        {
            get 
            {
                if (_ins == null)
                    _ins = new MainWindowViewModel();
                return _ins;
            }
            set { _ins = value; }
        }

        private Page[] listPage = { new PageMain(), new PageReport(), new PageTestResult(), new PageTestResultDetailed(), new PageQuestion(), new PageTestSearch(), new PageTest() };

        #region CloseWindow Command
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(param => CloseWindow());
                }
                return _closeCommand;
            }
            set
            {
                _closeCommand = value;
            }
        }

        private void CloseWindow()
        {

        }
        #endregion
        
        private Page _selectedPage;
        public Page SelectedPage
        {
            get
            {
                if (_selectedPage == null)
                    _selectedPage = listPage[0];
                return _selectedPage;
            }
            set
            {
                _selectedPage = value;
                OnPropertyChange("SelectedPage");
            }
        }

        ICommand _switchViewCommand;
        public ICommand SwitchViewCommand
        {
            get
            {
                if (_switchViewCommand == null)
                    _switchViewCommand = new RelayCommand(param => SwitchView(param as int?));
                return _switchViewCommand;
            }
            set { _switchViewCommand = value; }
        }

        #region Thanh - Test Result
        bool _editingTestResult = false;
        public bool EditingTestResult
        {
            get { return _editingTestResult; }
            set { _editingTestResult = value; }
        }

        public void StartEditingTestResult(object arg)
        {
            if (listPage[2].DataContext is TestResultViewModel)
            {
                (listPage[2].DataContext as TestResultViewModel).StartEditing(arg);
            }
            SelectedPage = listPage[2];
            EditingTestResult = true;
        }

        public void FinishEditingTestResult()
        {
            SelectedPage = listPage[3];
            EditingTestResult = false;
        }
        #endregion

        #region Long - Test Search new/edit
        Pair<bool, Page> _editingTest = new Pair<bool, Page>(false, null);
        public Pair<bool, Page> EditingTest
        {
            get { return _editingTest; }
            set { _editingTest = value; }
        }

        public void StartEditingTest(object arg)
        {
            if (EditingTest.First == true)
                ;// Uh... what?
            EditingTest.First = true;
            EditingTest.Second = SelectedPage;
            if (!(arg is string) || !(listPage[6].DataContext is TestViewModel))
                // ERROR
                return;
            SelectedPage = listPage[6];
            (listPage[6].DataContext as TestViewModel).ViewMode(arg as string);
        }

        public void FinishEditingTest()
        {
            SelectedPage = EditingTest.Second;
            EditingTest.First = false;
            EditingTest.Second = null;
        }
        #endregion

        public void SwitchView(int? id)
        {
            switch (id)
            {
                case 0: SelectedPage = listPage[0]; break;
                case 1: SelectedPage = listPage[4]; break;
                case 2:
                    {
                        if (EditingTest.First)
                            SelectedPage = listPage[6];
                        else SelectedPage = listPage[5];
                        break;
                    }
                case 3:
                    { 
                        if (EditingTestResult) 
                            SelectedPage = listPage[2]; 
                        else SelectedPage = listPage[3]; 
                        break; 
                    }
                case 4: SelectedPage = listPage[1]; break;
                // Thanh - Editing Test Result
                case 7: break;
                case 8: FinishEditingTestResult(); break;
                // Long - Editing Test
                case 9: StartEditingTest(""); break;
                case 10: FinishEditingTest(); break;
                default: return;
            }
        }

        public void SwitchView(int? id, object message)
        {
            if(id <= 4)
            {
                SwitchView(id);
                return;
            }
            switch(id)
            {
                case 5: break;
                case 6: break;
                // Thanh - Editing Test Result
                case 7:
                    {
                        StartEditingTestResult(message);
                        break;
                    }
                case 8: FinishEditingTestResult(); break;
                // Long - Editing Test
                case 9: StartEditingTest(message); break;
                case 10: FinishEditingTest(); break;
                default: return;
            }
        }
    }
}
