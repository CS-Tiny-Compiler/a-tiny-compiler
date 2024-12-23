using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler 
{
    
    public class GlobalVariables
    {
        //public static int existMain = 0;
        public static bool existMain = false;
    }
    public class Node
    {
        public List<Node> Children = new List<Node>();
        
        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public  Node root;
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = Program();
            return root;
        }

        Node Program()
        {
            // Program -> Functions Main

            Node program = new Node("Program");
            program.Children.Add(Functions());
            program.Children.Add(Main());
            GlobalVariables.existMain = true;
            
            if(InputPointer < TokenStream.Count)  //extra lexems
            {
                // check if it is a main function
                if(checkMain())
                {
                    Errors.Error_List.Add("Parsing Error: there shouldn't be multiple mains or anything after main. \r\n");
                }
                else
                {
                   Errors.Error_List.Add("Parsing Error: the code should end with the main function only. \r\n");
                }  
                
            }

            return program;
        }
        bool checkMain(){
          // int main(){}
            //Main -> DataType main ( ) FunctionBody
            if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                InputPointer++;
                if(InputPointer < TokenStream.Count && Token_Class.Main == TokenStream[InputPointer].token_type){
                    InputPointer++;
                    if(InputPointer < TokenStream.Count && Token_Class.LRoundParanthesis == TokenStream[InputPointer].token_type ){
                         InputPointer++;   
                         if(InputPointer < TokenStream.Count && Token_Class.RRoundParanthesis == TokenStream[InputPointer].token_type){
                            InputPointer++;
                            if(InputPointer < TokenStream.Count && Token_Class.LCurlyParanthesis == TokenStream[InputPointer].token_type){
                               // InputPointer++;
                                //if(InputPointer < TokenStream.Count && Token_Class.RCurlyParanthesis == TokenStream[InputPointer].token_type){
                                    return true;
                                //}
                            }
                         }
                    }
                }
            }
            return false;
        }
        Node Functions()
        {
            Node functions = new Node("Functions");

            // Functions -> FunctionStatement Functions | ε

            if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                InputPointer++;
                if(isTokenValid(Token_Class.Main))
                {
                    InputPointer--;
                    return null;
                }

                InputPointer--;
                functions.Children.Add(FunctionStatement());
                Functions();
            }
            return functions;
        }

        Node FunctionStatement()
        {
            Node functionStatement = new Node("FunctionStatement");

            //FunctionStatement -> FunctionDeclaration FunctionBody
         
            functionStatement.Children.Add(FunctionDeclaration());
            functionStatement.Children.Add(FunctionBody());

            return functionStatement;
        }

        Node FunctionDeclaration()
        {
            Node functionDeclaration = new Node("FunctionDeclaration");

            // DataType identifier ( ParametersList )
            
            functionDeclaration.Children.Add(DataType());
            functionDeclaration.Children.Add(match(Token_Class.Identifier));
            functionDeclaration.Children.Add(match(Token_Class.LRoundParanthesis));
            functionDeclaration.Children.Add(ParametersList());
            functionDeclaration.Children.Add(match(Token_Class.RRoundParanthesis));

            return functionDeclaration;
        }

        Node DataType()
        {
            Node dataType = new Node("DataType");

            // DataType -> int | float | string

            if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                dataType.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            else
            {
                // parse error ""
               if(InputPointer < TokenStream.Count)
               {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + " Data type (int or float or string) and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");
                    InputPointer++;
               }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                     + " Data type (int or float or string) but nothing was found. \r\n");
               
                }     
            }

            return dataType;
        }
        Node ParametersList()
        {
            Node parametersList = new Node("ParametersList");

           // ParametersList->Parameters | ε

            if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                parametersList.Children.Add(Parameters());
            }

            return parametersList;
        }

        
        Node Statement() //Review
        {
            Node statement = new Node("Statement");
            //Statement -> ReadStatement | WriteStatement | AssignmentStatement ;
            //| DeclarationStatement | IfStatement | RepeatStatement | FunctionCall ; | ε

            if (isTokenValid(Token_Class.Read))
            {
                statement.Children.Add(ReadStatement());
            }
            else if (isTokenValid(Token_Class.Write))
            {
                statement.Children.Add(WriteStatement());
            }
            else if (isTokenValid(Token_Class.Repeat))
            {
                statement.Children.Add(RepeatStatement());
            }
            else if (isTokenValid(Token_Class.Identifier))
            {
                InputPointer++;
                if (isTokenValid(Token_Class.AssignOp))
                {
                    InputPointer--;
                    statement.Children.Add(AssignmentStatement());
                }
                else
                {
                    InputPointer--;
                    statement.Children.Add(FunctionCall());
                }

                statement.Children.Add(match(Token_Class.Semicolon));
            }
            else if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                statement.Children.Add(DeclarationStatement());
            }
            else if (isTokenValid(Token_Class.If))
            {
                statement.Children.Add(IfStatement());
            }

            return statement;
        }
        Node ReadStatement()
        {
            Node readStatement = new Node("ReadStatement");
            // ReadStatement -> read identifier ;

            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Identifier));
            readStatement.Children.Add(match(Token_Class.Semicolon));

            return readStatement;
        }
        Node WriteStatement()
        {
            Node writeStatement = new Node("WriteStatement");
            // WriteStatement -> write WriteContent ;
            //  WriteContent-> Expression | endl
            writeStatement.Children.Add(match(Token_Class.Write));
            writeStatement.Children.Add(WriteContent());
            writeStatement.Children.Add(match(Token_Class.Semicolon));

            return writeStatement;
        }

        Node WriteContent()
        {
            //  WriteContent-> Expression | endl
            Node writeContent = new Node("WriteContent");

            if (isTokenValid(Token_Class.Endl))
            {
                writeContent.Children.Add(match(Token_Class.Endl));
            }
            else if(isTokenValid(Token_Class.Literal) || isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier) 
                || isTokenValid(Token_Class.LRoundParanthesis))
            {
                writeContent.Children.Add(Expression());
            }
            else
            {
                //parse error
                if(InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + " Expresssion or endl and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + " Expresssion or endl but nothing was found. \r\n");
                }

            }

            return writeContent;
        }

        Node Term() 
        {
            // Term -> number | identifier | FunctionCall
            Node term = new Node("Term");

            if (isTokenValid(Token_Class.Constant))
            {
                term.Children.Add(match(Token_Class.Constant));
            }
            else if (isTokenValid(Token_Class.Identifier))
            {
                InputPointer++;
                if (isTokenValid(Token_Class.LRoundParanthesis))
                {
                    InputPointer--;
                    term.Children.Add(FunctionCall());
                }
                else
                {
                    InputPointer--;
                    term.Children.Add(match(Token_Class.Identifier));
                }
            }
            else
            {
                //parse error
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + " Term [ number or identifier or functionCall ] " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + " Term [ number or identifier or functionCall ] but nothing was found. \r\n");
                }
            }

            return term;
        }

        // ArithmeticTerms -> arithmetic_operator ArithmeticTermsTail
        // ArithmeticTermsTail -> Equation | Term
        Node ArithmeticTerms()
        {
            Node arithmeticTerms = new Node("ArithmeticTerms");
            if (isArithmeticOperator())
            {
                if (TokenStream[InputPointer].token_type == Token_Class.PlusOp)
                {
                    arithmeticTerms.Children.Add(match(Token_Class.PlusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MinusOp)
                {
                    arithmeticTerms.Children.Add(match(Token_Class.MinusOp));
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
                {
                    arithmeticTerms.Children.Add(match(Token_Class.MultiplyOp));
                }
                else
                {
                    arithmeticTerms.Children.Add(match(Token_Class.DivideOp));
                }

            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {

                    Errors.Error_List.Add("Parsing Error: Expected "
                        + "an arithmatic operator [+ - / *] and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + "an arithmatic operator [+ - / *] but nothing was found. \r\n");
                }

            }

            arithmeticTerms.Children.Add(ArithmeticTermsTail());

            return arithmeticTerms;
        }
        Node ArithmeticTermsTail()
        {
            Node arithmeticTermsTail = new Node("ArithmeticTermsTail");

            //ArithmeticTermsTail -> Equation | Term

            if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                InputPointer++;

                if (isArithmeticOperator())
                {
                    InputPointer--;
                    Node n = new Node("Equation");
                    arithmeticTermsTail.Children.Add(Equation(n));
                }
                else
                {
                    InputPointer--;
                    arithmeticTermsTail.Children.Add(Term());
                }
            }
            else if(isTokenValid(Token_Class.LRoundParanthesis))
            {
                Node n = new Node("Equation");
                arithmeticTermsTail.Children.Add(Equation(n));
            }
            else
            {
                //parse

                if (InputPointer < TokenStream.Count)
                {

                    Errors.Error_List.Add("Parsing Error: Expected "
                     + "an Equation or Term and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found.\r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                     + "an Equation or Term but nothing was found. \r\n");
                }

            }

            return arithmeticTermsTail;
        }

        ///////////////////////////  Implement your logic here

        Node Parameters()
        {
            Node parameters = new Node("Parameters");
            Node _params = new Node("Params");

            //Parameters -> Parameter Params
            parameters.Children.Add(Parameter());
            parameters.Children.Add(Params(_params));
            return parameters;
        }
        Node Params(Node _params)
        {
            //Params -> ε | , Parameter Params

            if (isTokenValid(Token_Class.Comma))
            {
                _params.Children.Add(match(Token_Class.Comma));
                _params.Children.Add(Parameter());
                Params(_params);

            }

            return _params;
        }

        Node Parameter()
        {
            Node Parameter = new Node("Parameter");

            //Parameter -> DataType identifier
            Parameter.Children.Add(DataType());
            Parameter.Children.Add(match(Token_Class.Identifier));
            return Parameter;
        }

        Node Main()
        {
            Node main = new Node("Main");

            //Main -> DataType main ( ) FunctionBody
            main.Children.Add(DataType());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.LRoundParanthesis)); //can there be arguments?
            main.Children.Add(match(Token_Class.RRoundParanthesis));
            main.Children.Add(FunctionBody());
            
            
            
            return main;
        }
        Node FunctionBody()
        {
            Node functionBody = new Node("FunctionBody");

            //FunctionBody -> { Statements ReturnStatement }
            functionBody.Children.Add(match(Token_Class.LCurlyParanthesis));
            functionBody.Children.Add(Statements());
            functionBody.Children.Add(ReturnStatement());
            functionBody.Children.Add(match(Token_Class.RCurlyParanthesis));

            return functionBody;
        }

        Node Statements()
        {
            Node statements = new Node("statements");
            Node stmts = new Node("Stmts");
            //Statements -> Statement Stmts
            //Stmts-> ε | Statement Stmts

            statements.Children.Add(Statement());
            statements.Children.Add(Stmts(stmts));

            return statements;
        }
        Node Stmts(Node stmts)
        {

            //Stmts-> ε | Statement Stmts

            if (isTokenValid(Token_Class.Read) || isTokenValid(Token_Class.Write) || isTokenValid(Token_Class.Identifier)
                || isTokenValid(Token_Class.Repeat) || isTokenValid(Token_Class.If)
                || isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                stmts.Children.Add(Statement());
                Stmts(stmts); 
            }

            return stmts;
        }

        Node FunctionCall()
        {
            Node functionCall = new Node("FunctionCall");
            //FunctionCall -> identifier ( ArgList )

            functionCall.Children.Add(match(Token_Class.Identifier));
            functionCall.Children.Add(match(Token_Class.LRoundParanthesis));
            functionCall.Children.Add(ArgList());
            functionCall.Children.Add(match(Token_Class.RRoundParanthesis));

            return functionCall;
        }

        Node ArgList()
        {
            Node argList = new Node("ArgList");

            //ArgList -> Arguments | ε

            if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                argList.Children.Add(Arguments());
            }

            return argList;
        }

        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            Node args = new Node("Args");

            // Arguments -> Term Args
            arguments.Children.Add(Term());
            arguments.Children.Add(Args(args));

            return arguments;
        }
        Node Args(Node args)
        {
            // Args->ε | , Term Args
            if (isTokenValid(Token_Class.Comma))
            {
                args.Children.Add(match(Token_Class.Comma));
                args.Children.Add(Term());
                Args(args);
            }
            return args;
        }

        Node Expression()
        {
            Node expression = new Node("Expression");

            // Expression -> string | Term | Equation
            if (isTokenValid(Token_Class.Literal))
            {
                expression.Children.Add(match(Token_Class.Literal));
            }

            else if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                InputPointer++;
                if(isArithmeticOperator()) 
                {
                    InputPointer--;
                    Node n = new Node("Equation");
                    expression.Children.Add(Equation(n));
                }
                else
                {
                    InputPointer--;
                    expression.Children.Add(Term());
                }
            }
            else if (isTokenValid(Token_Class.LRoundParanthesis)) //Equation
            {
                Node n = new Node("Equation");

                expression.Children.Add(Equation(n));
            }
            else
            {
                // parse error
                if(InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + "an Expression [string or equation or term] and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found.\r\n");
                    InputPointer++;
                }
               else
               {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + "an Expression [string or equation or term] but nothing was found. \r\n");
               
               }

            }

            return expression;
        }

        Node Equation(Node node) /////// 
        {
            // Equation -> Term ArithmeticTerms | (Equation) EquationTail

            if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                node.Children.Add(Term());
                node.Children.Add(ArithmeticTerms());
            
            }
            else if (isTokenValid(Token_Class.LRoundParanthesis)) //Equation
            {

                node.Children.Add(match(Token_Class.LRoundParanthesis));
                Node n = new Node("Equation");
                node.Children.Add(Equation(n));
                node.Children.Add(match(Token_Class.RRoundParanthesis));
                node.Children.Add(EquationTail());

            }
            else
            {
                // parse error
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                    + "an Equation and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected an Equation but nothing was found. \r\n");
                }   

            }

            return node;
        }

        Node EquationTail()
        {
            Node equationTail = new Node("EquationTail");

            // EquationTail -> ε | ArithmeticTerms

            if (InputPointer < TokenStream.Count && isArithmeticOperator())
            {
                equationTail.Children.Add(ArithmeticTerms());
            }
            return equationTail;
        }

        bool isArithmeticOperator()
        {
            return InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.PlusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.MultiplyOp ||
                TokenStream[InputPointer].token_type == Token_Class.DivideOp;
        }

        bool isConditionOperator()
        {
            return InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LessThanOp ||
                TokenStream[InputPointer].token_type == Token_Class.LessThanOrEqualOp ||
                TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp ||
                TokenStream[InputPointer].token_type == Token_Class.GreaterThanOrEqualOp||
                TokenStream[InputPointer].token_type == Token_Class.EqualOp ||
                TokenStream[InputPointer].token_type == Token_Class.NotEqualOp||
                TokenStream[InputPointer].token_type == Token_Class.AndOp ||
                TokenStream[InputPointer].token_type == Token_Class.OrOp;
        }

        public bool isTokenValid(Token_Class tokenType)
        {
            return (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == tokenType);
        }

        
        
        Node AssignmentStatement()
        {
           // identifier assignment_operator Expression
           
            Node node = new Node("Assignment Statement");
            //match identifier
            node.Children.Add(match(Token_Class.Identifier));
            //match assignment operator
            node.Children.Add(match(Token_Class.AssignOp));
            //check expression
            Node expression=Expression();

            node.Children.Add(expression);
            
            return node;
        }

        //### DeclarationStatement -> DataType Declarations ;
        Node DeclarationStatement()
        {
            Node CurVar = new Node("DeclarationStatement");
            CurVar.Children.Add(DataType());
            CurVar.Children.Add(Declarations());
            CurVar.Children.Add(match(Token_Class.Semicolon));

            return CurVar;
        }

        //### Declarations -> identifier Decls | AssignmentStatement Decls
        Node Declarations()
        {
            Node node = new Node("Declarations");
            Node node2 = new Node("Decls");

            if (TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                if(InputPointer+1 <TokenStream.Count && TokenStream[InputPointer+1].token_type!=Token_Class.Comma &&
                TokenStream[InputPointer + 1].token_type != Token_Class.Semicolon)
                {
                    node.Children.Add(AssignmentStatement());
                    node.Children.Add(Decls(node2));
                }
                else
                {
                    node.Children.Add(match(Token_Class.Identifier));
                    node.Children.Add(Decls(node2));
                }
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                     + " Declarations [identifier or AssignmentStatement] and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                     + " Declarations [identifier or AssignmentStatement] but nothing was found. \r\n");
                }
            }

            return node;
        }

        // Decls -> ε | , identifier Decls | , AssignmentStatement Decls
        Node Decls(Node _Decls)
        {
            //Node node = new Node("Decls");
            if (isTokenValid(Token_Class.Comma))
            {
                _Decls.Children.Add(match(Token_Class.Comma));

                if (AssignmentStart())
                {
                    _Decls.Children.Add(AssignmentStatement());
                    Decls(_Decls);
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.Identifier)
                {
                    _Decls.Children.Add(match(Token_Class.Identifier));
                    Decls(_Decls);
                }
                else
                {
                    if (InputPointer < TokenStream.Count)
                    {

                        Errors.Error_List.Add("Parsing Error: Expected "
                         + " Decls [identifier or AssignmentStatement] and " +
                            TokenStream[InputPointer].token_type.ToString() +
                            "  found. \r\n");
                        InputPointer++;
                    }
                    else
                    {
                        Errors.Error_List.Add("Parsing Error: Expected "
                         + " Decls [identifier or AssignmentStatement] but nothing was found. \r\n");
                    }
                }
            }
            //else nothing return empty
            return _Decls;
        }
        bool AssignmentStart()
        {
            return TokenStream[InputPointer].token_type == Token_Class.Identifier && FindNext();
        }
        bool FindNext()
        {
            if (InputPointer + 1 < TokenStream.Count)
                return TokenStream[InputPointer + 1].token_type == Token_Class.AssignOp;

            return false;
        }

        Node ReturnStatement()
        {
            // ReturnStatement -> return Expression ;

            Node CurVar = new Node("Return Statement");

            if (TokenStream.Count > InputPointer)
            {
                CurVar.Children.Add(match(Token_Class.Return));
                CurVar.Children.Add(Expression());
                CurVar.Children.Add(match(Token_Class.Semicolon));
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {

                    Errors.Error_List.Add("Parsing Error: Expected "
                     + " return and  " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected return but nothing was found. \r\n");
                }

            }
            return CurVar;
        }

        Node IfStatement()
        {
            //IfStatement -> if ConditionStatement then Statements ElseIfStatements ElseStatement end
            Node node = new Node("If Statement");
            node.Children.Add(match(Token_Class.If));
            node.Children.Add(ConditionStatement());
            node.Children.Add(match(Token_Class.Then));
            node.Children.Add(Statements());
            Node ElseIf = new Node("ElseIf Statements");
            node.Children.Add(ElseIfStatements(ElseIf));
            node.Children.Add(ElseStatement());
            node.Children.Add(match(Token_Class.End));
            return node;
        }

        //ConditionStatement -> Condition CondStmts
        Node ConditionStatement()
        {
            Node node = new Node("Condition Statement");
            Node _CondStmts = new Node("CondStmts");
            node.Children.Add(Condition());
            node.Children.Add(CondStmts(_CondStmts));

            return node;
        }
        //CondStmts -> ε | boolean_operator Condition CondStmts
        Node CondStmts(Node _CondStmts)
        {
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.AndOp ||
                    TokenStream[InputPointer].token_type == Token_Class.OrOp))
            {
                _CondStmts.Children.Add(match(TokenStream[InputPointer].token_type));
                _CondStmts.Children.Add(Condition());
                CondStmts(_CondStmts);
            }

            return _CondStmts;
        }

        //### 28. Condition -> identifier condition_operator Term
        Node Condition()
        {
            Node node = new Node("Condition");
            
            node.Children.Add(match(Token_Class.Identifier));
          
            if (isConditionOperator())
            {
                node.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                 + "a condition_operator  and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                 + "a condition_operator but nothing was found.\r\n");
                }

            }
            node.Children.Add(Term());
            return node;
        }
        //### 29. ElseIfStatements -> elseif ConditionStatement then Statements ElseIfStatements | ε
        Node ElseIfStatements(Node ElseIf)
        {
            if (isTokenValid(Token_Class.ElseIf))
            {
                ElseIf.Children.Add(match(Token_Class.ElseIf));
                ElseIf.Children.Add(ConditionStatement());
                ElseIf.Children.Add(match(Token_Class.Then));
                ElseIf.Children.Add(Statements());
                ElseIfStatements(ElseIf);
            }

            return ElseIf;
        }
        Node ElseStatement()
        {
            Node node = new Node("Else Statement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                node.Children.Add(match(Token_Class.Else));
                node.Children.Add(Statements());
            }
            return node;

            //return node inside if condition and null outside to avoid printing empty branches if not handled in the print function
        }

        Node RepeatStatement()
        {
            Node node = new Node("RepeatStatement");

            node.Children.Add(match(Token_Class.Repeat));
            node.Children.Add(Statements());
            node.Children.Add(match(Token_Class.Until));
            node.Children.Add(ConditionStatement());

            return node;
        }


        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
