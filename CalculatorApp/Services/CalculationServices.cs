using CalculatorApp.Interfaces;
using CalculatorApp.Models;
using System.Xml.Serialization;


namespace CalculatorApp.Services
{
    public class CalculationServices : ICalculationServices
    {
        private readonly ILogger<CalculationServices> _logger;


        private Dictionary<string, Func<IEnumerable<string>, double>> _listOfOperations;

        public CalculationServices(ILogger<CalculationServices> logger)
        {
            _logger = logger;
            _listOfOperations = new Dictionary<string, Func<IEnumerable<string>, double>> {
                {"plus",  Add},
                {"minus",  Minus},
                {"multiplication", Multiplication  },
                {"divide",  Divide},
            };
        }

        public List<double> ProcessOperations(List<Maths.Operation> operations)
        {
            if (operations == null || !operations.Any())
            {
                _logger.LogError("Operations list is null or empty.");
                throw new ArgumentNullException(nameof(operations), "Operations list is null or empty.");
            }

            _logger.LogInformation($"Processing {operations.Count} operations.");

            var results = new List<double>();

            foreach (var operation in operations)
            {

                if (operation.Values == null || !operation.Values.Any())
                {
                    _logger.LogWarning($"Operation {operation.Id} has no values.");
                    continue;
                }

                //operation.Id = "plus"
                // operation.Id = "minus"
                if (_listOfOperations.TryGetValue(operation.Id , out var operationFunc))
                {

                    double result = operationFunc(operation.Values);
                    results.Add(result);

                } else
                {
                    _logger.LogError($"The operation {operation.Id} is not available");
                }

            }

            return results;
        }

        public void RegisterOperation(string operationName, Func<IEnumerable<string>, double> operationFunc) 
        {
            if (string.IsNullOrEmpty(operationName))
            {
                throw new ArgumentNullException(nameof(operationName), "Operation Name can not be null");
            }

            if (operationFunc == null)
            {
                throw new ArgumentException(nameof(operationFunc), "Operation Delegate can not be null");
            }

            _listOfOperations[operationName] = operationFunc;
        }

        private IEnumerable<double> ConvertValuesToDoubles(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                if (double.TryParse(value, out var doubleValue))
                {
                    yield return doubleValue;
                }
                else
                {
                    _logger.LogError($"Invalid number format: {value}");
                    throw new InvalidOperationException($"Invalid number format: {value}");
                }
            }
        }

        public double Add(IEnumerable<string> inputNumbers)
        {
            var numbers = ConvertValuesToDoubles(inputNumbers);
            _logger.LogInformation($"Adding numbers: {string.Join(", ", numbers)}");
            return numbers.Sum();
        }

        public double Divide(IEnumerable<string> inputNumbers)
        {
            var numbers = ConvertValuesToDoubles(inputNumbers).ToList();
            if (numbers.Skip(1).Contains(0))
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }

            _logger.LogInformation($"Dividing numbers: {string.Join(", ", numbers)}");
            return numbers.Aggregate((a, b) => a / b);
        }

        public double Minus(IEnumerable<string> inputNumbers)
        {
            var numbers = ConvertValuesToDoubles(inputNumbers).ToList();
            if (!numbers.Any())
            {
                throw new ArgumentException("No numbers provided for subtraction");
            }

            double result = numbers.First();
            foreach (var number in numbers.Skip(1))
            {
                result -= number;
            }

            _logger.LogInformation($"Subtracting numbers: {string.Join(", ", numbers)} Result: {result}");
            return result;
        }

        public double Multiplication(IEnumerable<string> inputNumbers)
        {
            var numbers = ConvertValuesToDoubles(inputNumbers);
            _logger.LogInformation($"Multiplying numbers: {string.Join(", ", numbers)}");
            return numbers.Aggregate(1.0, (acc, val) => acc * val);
        }

        public List<double> CalculateFromXml(string input)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Maths));
                using var reader = new StringReader(input);
                var maths = (Maths)serializer.Deserialize(reader);
                if (maths == null || maths.Operations == null)
                {
                    throw new InvalidOperationException("Invalid XML input.");
                }

                var results = ProcessOperations(maths.Operations);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating from XML input.");
                throw;
            }
        }
    }
}