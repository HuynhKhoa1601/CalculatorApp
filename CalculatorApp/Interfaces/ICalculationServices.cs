using System.Collections.Generic;
using CalculatorApp.Models;

namespace CalculatorApp.Interfaces
{
    public interface ICalculationServices
    {
        List<double> ProcessOperations(List<Maths.Operation> operations);
        void RegisterOperation(string operationName, Func<IEnumerable<string>, double> operationFunc);
        double Add(IEnumerable<string> inputNumbers);
        double Multiplication(IEnumerable<string> inputNumbers);
        double Minus(IEnumerable<string> inputNumbers);
        double Divide(IEnumerable<string> inputNumbers);
        List<double> CalculateFromXml(string input);
    }
}
