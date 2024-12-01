using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PredictiveStackParser.Automatas
{
    internal enum SintacticError
    {
        MissingOpeningBracket,
        MissingClosingBracket,
        MissingOperand,
        MissingOperator,
        InvalidExpression,
        NoError
    }

    internal record SintacticResult
    {
        public string ExpressionText = "";
        public SintacticError errorType = SintacticError.NoError;
        public int LineFound;
    }

    internal class OldSintacticAutomata
    {
        ///Like a sintactic transition
        private readonly (int, char, char)[,] _transitionTable;
        private (int, char, char) _currentState;
        private List<SintacticResult> _results = new();
        private Stack<char> _bracketsStack = new();
        private int[] validStates = { 2, 3 };

        public OldSintacticAutomata()
        {
            _transitionTable = new[,]
            {
                      //NextState, CharToPop, CharToPush = (int, char, char)

                       // Identifier           Operand              (                    )
               /*q0*/ { (3, '\0', '\0'),   (5, '\0', '\0'),   (1, '\0',  '('),    (5, '\0', '\0') },
               /*q1*/ { (3, '\0', '\0'),   (5, '\0', '\0'),   (1, '\0',  '('),    (2, '(', '\0') },
               /*q2*/ { (5, '\0', '\0'),   (4, '\0', '\0'),   (1, '\0',  '('),    (2, '(', '\0') },
               /*q3*/ { (5, '\0', '\0'),   (0, '\0', '\0'),   (5, '\0', '\0'),    (2, '(', '\0') },
               /*q4*/ { (2, '\0', '\0'),   (5, '\0', '\0'),   (1, '\0',  '('),    (5, '\0', '\0')},
               /*q5*/ { (5, '\0', '\0'),   (5, '\0', '\0'),   (5, '\0', '\0'),    (5, '\0', '\0') }
            };

            _currentState = (0, '\0', '\0');
        }

        private bool IsActualStateValid()
        {
            return validStates.Contains(_currentState.Item1);
        }

        private string GetFullText(List<RegistroLexico> registers)
        {
            return string.Join(' ', registers.Select(r => r.TokenText).ToArray());
        }

        public List<SintacticResult> AnalizeLexicResults(List<RegistroLexico> lexicResults)
        {
            _results.Clear();

            var registersPerLine = lexicResults
                .GroupBy(x => x.LineaNum)
                .OrderBy(group => group.Key)
                .Select(group => group.ToList())
                .ToList();

            Token previousToken = Token.CadenaVacia;
            bool alreadyErrorHandled = false;

            foreach (List<RegistroLexico> linea in registersPerLine)
            {
                _bracketsStack.Clear();
                _currentState = (0, '\0', '\0');
                alreadyErrorHandled = false;

                foreach (RegistroLexico r in linea)
                {
                    //Go to the next state
                    if (r.tokenType == Token.Identificador)
                        _currentState = _transitionTable[_currentState.Item1, 0];

                    if (r.tokenType == Token.Operadores)
                        _currentState = _transitionTable[_currentState.Item1, 1];

                    if (r.TokenText == "(")
                        _currentState = _transitionTable[_currentState.Item1, 2];

                    if (r.TokenText == ")")
                        _currentState = _transitionTable[_currentState.Item1, 3];


                    // is there something to extract from the stack? ---------------------
                    if (_currentState.Item2 != '\0')
                    {
                        if (_bracketsStack.Count == 0)
                        {
                            _results.Add(new SintacticResult()
                            {
                                errorType = SintacticError.MissingOpeningBracket,
                                ExpressionText = GetFullText(linea),
                                LineFound = r.LineaNum
                            });
                            alreadyErrorHandled = true;
                            break;
                        }
                        else
                            _bracketsStack.Pop();
                    }

                    // is there something to push to the stack? ----------------------------
                    if (_currentState.Item3 != '\0')
                        _bracketsStack.Push(_currentState.Item3);

                    if (_currentState.Item1 == 5) // its wrong... is invalid
                    {
                        var fullLineText = GetFullText(linea);
                        var errorToSend = SintacticError.InvalidExpression;

                        if (previousToken == Token.Operadores && r.tokenType == Token.Operadores)
                            errorToSend = SintacticError.MissingOperand;

                        if (previousToken == Token.Identificador && r.tokenType == Token.Identificador)
                            errorToSend = SintacticError.MissingOperator;

                        if (previousToken == Token.Identificador && r.tokenType == Token.Delimitadores)
                            errorToSend = SintacticError.MissingOperator;


                        _results.Add(new SintacticResult()
                        {
                            errorType = errorToSend,
                            ExpressionText = fullLineText,
                            LineFound = r.LineaNum
                        });
                        alreadyErrorHandled = true;
                        break;
                    }

                    previousToken = r.tokenType;
                }

                if (alreadyErrorHandled)
                {
                    continue;
                }

                if (IsActualStateValid() && _bracketsStack.Count == 0)
                {
                    _results.Add(new SintacticResult()
                    {
                        errorType = SintacticError.NoError,
                        ExpressionText = GetFullText(linea),
                        LineFound = linea[0].LineaNum
                    });
                }
                else if (!IsActualStateValid())
                {
                    if (previousToken == Token.Operadores)
                    {
                        _results.Add(new SintacticResult()
                        {
                            errorType = SintacticError.MissingOperand,
                            ExpressionText = GetFullText(linea),
                            LineFound = linea[0].LineaNum
                        });
                    }
                    else
                    {
                        _results.Add(new SintacticResult()
                        {
                            errorType = SintacticError.InvalidExpression,
                            ExpressionText = GetFullText(linea),
                            LineFound = linea[0].LineaNum
                        });
                    }
                }
                else if (_bracketsStack.Count > 0)
                {
                    _results.Add(new SintacticResult()
                    {
                        errorType = SintacticError.MissingClosingBracket,
                        ExpressionText = GetFullText(linea),
                        LineFound = linea[0].LineaNum
                    });
                }

            }

            return _results;
        }
    }
}
