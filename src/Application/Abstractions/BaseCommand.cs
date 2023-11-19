using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Commands
{
    public abstract class BaseCommand : ICommand
    {

        public BaseCommand() 
        {
            CommandResult = new CommandResult();
        }

        public abstract bool IsValid();


        public ValidationResult ValidationResult { get; set; }

        public CommandResult CommandResult { get; set; }

        public void AddError(string message)
        {
            if (CommandResult.CommandErrorList == null)
                CommandResult.CommandErrorList = new List<CommandError>();
            
            CommandResult.CommandErrorList.Add(new CommandError() { Message = message });
        }

        public bool IsCommandValid()
        {
            return this.IsValid() && !CommandResult.CommandErrorList.Any();
        }

        public List<CommandError> Errors
        {
            get
            {
                if (this.ValidationResult == null || this.ValidationResult.Errors == null)
                {
                    return new List<CommandError>();
                }

                if (this.ValidationResult.Errors.Any())
                {
                    foreach (var item in this.ValidationResult.Errors)
                    {
                        this.AddError($"{item.PropertyName}.{item.ErrorMessage}");
                    }
                }

                return CommandResult.CommandErrorList;
            }
        }
    }

    public class CommandResult
    {
        public CommandResult()
        {
            CommandErrorList = new List<CommandError>();
        }
        public List<CommandError> CommandErrorList { get; set; }
    }

    public class CommandError
    {
        public string Message { get; set; }
    }
}
