// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.CommandHandler.cs
// Created on: 20201211
// -----------------------------------------------

using System;
using System.Windows.Input;

namespace Pony.Support
{
    /// <summary>
    ///     https://stackoverflow.com/questions/12422945/how-to-bind-wpf-button-to-a-command-in-viewmodelbase
    ///     https://stackoverflow.com/questions/35370749/passing-parameters-to-mvvm-command
    /// </summary>
    public class CommandHandler : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<bool>     _canExecute;

        /// <summary>
        ///     Creates instance of the command handler
        /// </summary>
        /// <param name="action">Action to be executed by the command</param>
        /// <param name="canExecute">A boolean property to containing current permissions to execute the command</param>
        public CommandHandler(Action<object> action, Func<bool> canExecute)
        {
            _action     = action;
            _canExecute = canExecute;
        }

        /// <summary>
        ///     Wires CanExecuteChanged event
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        ///     Forces checking if execute is allowed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => _canExecute.Invoke();

        public void Execute(object parameter) => _action(parameter);
    }
}