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
            Node functions = new Node("Functions");
            program.Children.Add(Functions(functions));
            program.Children.Add(Main());
            
            if(InputPointer < TokenStream.Count)  //extra lexems
            {
                if(checkMain())
                {
                    Errors.Error_List.Add("Parsing Error: there shouldn't be multiple mains or anything after main. \r\n");
                }
                else
                {
                   Errors.Error_List.Add("Parsing Error: the code should end with the main function only. \r\n");
                   //Errors.Error_List.Add(InputPointer + "\n" + TokenStream.Count);

                }

            }

            return program;
        }

        bool checkMain()
        {
            // Main -> DataType main ( ) FunctionBody
            if (!(isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String)))
            {
                return false;
            }

            InputPointer++;
            if (InputPointer >= TokenStream.Count || TokenStream[InputPointer].token_type != Token_Class.Main)
            {
                return false;
            }

            InputPointer++;
            if (InputPointer >= TokenStream.Count || TokenStream[InputPointer].token_type != Token_Class.LRoundParanthesis)
            {
                return false;
            }

            InputPointer++;
            if (InputPointer >= TokenStream.Count || TokenStream[InputPointer].token_type != Token_Class.RRoundParanthesis)
            {
                return false;
            }

            InputPointer++;
            if (InputPointer >= TokenStream.Count || TokenStream[InputPointer].token_type != Token_Class.LCurlyParanthesis)
            {
                return false;
            }

            return true;
        }
        Node Functions(Node function)
        {
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
                function.Children.Add(FunctionStatement());
                Functions(function);
            }

            return function;
        }

        Node FunctionStatement()
        {
            //FunctionStatement -> FunctionDeclaration FunctionBody

            Node functionStatement = new Node("FunctionStatement");

            functionStatement.Children.Add(FunctionDeclaration());
            functionStatement.Children.Add(FunctionBody());

            return functionStatement;
        }

        Node FunctionDeclaration()
        {
            // DataType identifier ( ParametersList )

            Node functionDeclaration = new Node("FunctionDeclaration");
            
            functionDeclaration.Children.Add(DataType());
            functionDeclaration.Children.Add(match(Token_Class.Identifier));
            functionDeclaration.Children.Add(match(Token_Class.LRoundParanthesis));
            functionDeclaration.Children.Add(ParametersList());
            functionDeclaration.Children.Add(match(Token_Class.RRoundParanthesis));

            return functionDeclaration;
        }

        Node DataType()
        {
            // DataType -> int | float | string

            Node dataType = new Node("DataType");

            if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                dataType.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            else
            {
               if(InputPointer < TokenStream.Count)
               {
                    Errors.Error_List.Add("Parsing Error: Expected Data type (int or float or string) and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");
                    InputPointer++;
               }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected Data type (int or float or string) but nothing was found. \r\n");
               
                }     
            }

            return dataType;
        }
        Node ParametersList()
        {
            // ParametersList->Parameters | ε

            Node parametersList = new Node("ParametersList");

            if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                parametersList.Children.Add(Parameters());
            }

            return parametersList;
        }

        
        Node Statement() 
        {
            //Statement -> ReadStatement | WriteStatement | AssignmentStatement ;
            //| DeclarationStatement | IfStatement | RepeatStatement | FunctionCall ; | ε

            Node statement = new Node("Statement");
            
            if (isTokenValid(Token_Class.Read))
            {
                statement.Children.Add(ReadStatement());
                // read  ;
                // if 
                while (!(isTokenValid(Token_Class.Read) || isTokenValid(Token_Class.End) || isTokenValid(Token_Class.Write) || isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String) || isTokenValid(Token_Class.Repeat) || isTokenValid(Token_Class.If) || isTokenValid(Token_Class.Return) || isTokenValid(Token_Class.Until) || isTokenValid(Token_Class.Identifier)))
                {
                    InputPointer++;
                }
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
                //loop 
                while (!(isTokenValid(Token_Class.Read) || isTokenValid(Token_Class.Identifier) || isTokenValid(Token_Class.Write) || isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String) || isTokenValid(Token_Class.Repeat) || isTokenValid(Token_Class.If) || isTokenValid(Token_Class.Return) || isTokenValid(Token_Class.Until) || isTokenValid(Token_Class.Else) || isTokenValid(Token_Class.ElseIf) || isTokenValid(Token_Class.End)))
                {
                    InputPointer++;
                }

            }
            else if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
            {
                statement.Children.Add(DeclarationStatement());
            }
            else if (isTokenValid(Token_Class.If))
            {
                statement.Children.Add(IfStatement());
            }

            //////
            while (isTokenValid(Token_Class.Semicolon))
            { 
                InputPointer++;
            }

            return statement;
        }
        Node ReadStatement()
        {
            // ReadStatement -> read identifier ;

            Node readStatement = new Node("ReadStatement");
            //read ;
            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Identifier));
            readStatement.Children.Add(match(Token_Class.Semicolon));

            return readStatement;
        }
        Node WriteStatement()
        {
            // WriteStatement -> write WriteContent ;
            // WriteContent-> Expression | endl

            Node writeStatement = new Node("WriteStatement");
           
            writeStatement.Children.Add(match(Token_Class.Write));
            writeStatement.Children.Add(WriteContent());
            writeStatement.Children.Add(match(Token_Class.Semicolon));

            return writeStatement;
        }

        Node WriteContent()
        {
            // WriteContent-> Expression | endl
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
                if(InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected Expresssion or endl and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");

                    while(!(isTokenValid(Token_Class.Read) || isTokenValid(Token_Class.End) || isTokenValid(Token_Class.Write) || isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String) || isTokenValid(Token_Class.Repeat) || isTokenValid(Token_Class.If) || isTokenValid(Token_Class.Return) || isTokenValid(Token_Class.Until) || isTokenValid(Token_Class.Identifier)))
                    {
                        InputPointer++;
                    }
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected Expresssion or endl but nothing was found. \r\n");
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
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected Term [ number or identifier or functionCall ] " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected Term [ number or identifier or functionCall ] but nothing was found. \r\n");
                }
            }

            return term;
        }

        
        Node ArithmeticTerms()
        {
            // ArithmeticTerms -> arithmetic_operator ArithmeticTermsTail
            // ArithmeticTermsTail -> Equation | Term

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

                    Errors.Error_List.Add("Parsing Error: Expected an arithmatic operator [+ - / *] and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected an arithmatic operator [+ - / *] but nothing was found. \r\n");
                }

            }

            arithmeticTerms.Children.Add(ArithmeticTermsTail());

            return arithmeticTerms;
        }
        Node ArithmeticTermsTail()
        {
            //ArithmeticTermsTail -> Equation | Term

            Node arithmeticTermsTail = new Node("ArithmeticTermsTail");

            if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                InputPointer++;

                if (isArithmeticOperator())
                {
                    InputPointer--;
                    Node equation = new Node("Equation");
                    arithmeticTermsTail.Children.Add(Equation(equation));
                }
                else
                {
                    InputPointer--;
                    arithmeticTermsTail.Children.Add(Term());
                }
            }
            else if(isTokenValid(Token_Class.LRoundParanthesis))
            {
                Node equation = new Node("Equation");
                arithmeticTermsTail.Children.Add(Equation(equation));
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {

                    Errors.Error_List.Add("Parsing Error: Expected an Equation or Term and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found.\r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected an Equation or Term but nothing was found. \r\n");
                }

            }

            return arithmeticTermsTail;
        }

        Node Parameters()
        {
            //Parameters -> Parameter Params

            Node parameters = new Node("Parameters");
            Node _params = new Node("Params");
   
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
            //Parameter -> DataType identifier

            Node parameter = new Node("Parameter");
   
            parameter.Children.Add(DataType());
            parameter.Children.Add(match(Token_Class.Identifier));

            return parameter;
        }

        Node Main()
        {
            Node main = new Node("Main");

            //Main -> DataType main ( ) FunctionBody
            main.Children.Add(DataType());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.LRoundParanthesis)); 
            main.Children.Add(match(Token_Class.RRoundParanthesis));
            main.Children.Add(FunctionBody());
              
            return main;
        }
        Node FunctionBody()
        {
            //FunctionBody -> { Statements ReturnStatement }

            Node functionBody = new Node("FunctionBody");
      
            functionBody.Children.Add(match(Token_Class.LCurlyParanthesis));
            functionBody.Children.Add(Statements());
            functionBody.Children.Add(ReturnStatement());
            functionBody.Children.Add(match(Token_Class.RCurlyParanthesis));

            return functionBody;
        }

        Node Statements()
        {
            // Statements -> Statement Stmts
            // Stmts-> ε | Statement Stmts

            Node statements = new Node("Statements");
            Node stmts = new Node("Stmts");
           

            statements.Children.Add(Statement());
            statements.Children.Add(Stmts(stmts));

            return statements;
        }
        Node Stmts(Node stmts)
        {
            // 1 ;
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
            //FunctionCall -> identifier ( ArgList )
            Node functionCall = new Node("FunctionCall");
            
            functionCall.Children.Add(match(Token_Class.Identifier));
            functionCall.Children.Add(match(Token_Class.LRoundParanthesis));
            functionCall.Children.Add(ArgList());
            functionCall.Children.Add(match(Token_Class.RRoundParanthesis));

            return functionCall;
        }

        Node ArgList()
        {
            //ArgList -> Arguments | ε
            Node argList = new Node("ArgList");

            if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                argList.Children.Add(Arguments());
            }

            return argList;
        }

        Node Arguments()
        {
            // Arguments -> Term Args
            Node arguments = new Node("Arguments");
            Node args = new Node("Args");
     
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
            // x:= x 1;
            // Expression -> string | Term | Equation
            Node expression = new Node("Expression");
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
                    Node equation = new Node("Equation");
                    expression.Children.Add(Equation(equation));
                }
                else
                {
                    InputPointer--;
                    expression.Children.Add(Term());
                }
            }
            else if (isTokenValid(Token_Class.LRoundParanthesis)) 
            {
                Node equation = new Node("Equation");

                expression.Children.Add(Equation(equation));
            }
            else
            {
                if(InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected an Expression [string or equation or term] and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found.\r\n");
                    //InputPointer++;
                }
               else
               {
                    Errors.Error_List.Add("Parsing Error: Expected an Expression [string or equation or term] but nothing was found. \r\n");
               
               }

            }

            return expression;
        }

        Node Equation(Node node) 
        {
            // Equation -> Term ArithmeticTerms | (Equation) EquationTail

            if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
            {
                node.Children.Add(Term());
                node.Children.Add(ArithmeticTerms());
            
            }
            else if (isTokenValid(Token_Class.LRoundParanthesis)) 
            {
                node.Children.Add(match(Token_Class.LRoundParanthesis));
                Node equation = new Node("Equation");
                node.Children.Add(Equation(equation));
                node.Children.Add(match(Token_Class.RRoundParanthesis));
                node.Children.Add(EquationTail());

            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected an Equation and " +
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
            // EquationTail -> ε | ArithmeticTerms
            Node equationTail = new Node("EquationTail");

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
           
            Node assignmentStatement = new Node("AssignmentStatement");
            assignmentStatement.Children.Add(match(Token_Class.Identifier));
            assignmentStatement.Children.Add(match(Token_Class.AssignOp));
            assignmentStatement.Children.Add(Expression());
            
            return assignmentStatement;
        }
        // int s,22 
        
        Node DeclarationStatement()
        {
            // DeclarationStatement -> DataType Declarations ;

            Node declarationStatement = new Node("DeclarationStatement");

            declarationStatement.Children.Add(DataType());
            declarationStatement.Children.Add(Declarations());
            declarationStatement.Children.Add(match(Token_Class.Semicolon));

            return declarationStatement;
        }

        Node Declarations()
        {
            //Declarations -> identifier Decls | AssignmentStatement Decls

            Node declarations = new Node("Declarations");
            Node decls = new Node("Decls");
            // int ;

            if (TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                if(InputPointer+1 <TokenStream.Count && TokenStream[InputPointer+1].token_type!=Token_Class.Comma &&
                TokenStream[InputPointer + 1].token_type != Token_Class.Semicolon)
                {
                    //int x:= , t;
                    declarations.Children.Add(AssignmentStatement());
                    declarations.Children.Add(Decls(decls));
                }
                else
                {
                    declarations.Children.Add(match(Token_Class.Identifier));
                    declarations.Children.Add(Decls(decls));
                }
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected Declarations [identifier or AssignmentStatement] and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    //InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected Declarations [identifier or AssignmentStatement] but nothing was found. \r\n");
                }
            }

            return declarations;
        }

        Node Decls(Node decls)
        {
            // Decls -> ε | , DeclsTail  Decls
            Node declsTail = new Node("Decls");
            if (isTokenValid(Token_Class.Comma))
            {
                decls.Children.Add(match(Token_Class.Comma));
                decls.Children.Add(DeclsTail());
                decls.Children.Add(Decls(declsTail));
                

            }

            return decls;
        }
        Node DeclsTail()
        {
            // DeclsTail -> identifier|AssignmentStatement
            Node declsTail = new Node("DeclsTail");
            if (AssignmentStart())
            {
                declsTail.Children.Add(AssignmentStatement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                declsTail.Children.Add(match(Token_Class.Identifier));
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {

                    Errors.Error_List.Add("Parsing Error: Expected Decls [identifier or AssignmentStatement] and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    while(!ShouldIStop())
                    {
                        InputPointer++;
                    }
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected Decls [identifier or AssignmentStatement] but nothing was found. \r\n");
                }
            }

            return declsTail;
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
        bool ShouldIStop()
        {
            return (isTokenValid(Token_Class.Read) || isTokenValid(Token_Class.Write) || isTokenValid(Token_Class.Semicolon) || isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String) || isTokenValid(Token_Class.Repeat) || isTokenValid(Token_Class.If) || isTokenValid(Token_Class.Return) || isTokenValid(Token_Class.Until));
        }

        Node ReturnStatement()
        {
            // ReturnStatement -> return Expression ;

            Node returnStatement = new Node("ReturnStatement");

            if (TokenStream.Count > InputPointer)
            {
                returnStatement.Children.Add(match(Token_Class.Return));
                returnStatement.Children.Add(Expression());
                returnStatement.Children.Add(match(Token_Class.Semicolon));
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {

                    Errors.Error_List.Add("Parsing Error: Expected return and  " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found. \r\n");
                    InputPointer++;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected return but nothing was found. \r\n");
                }

            }
            return returnStatement;
        }

        Node IfStatement()
        {
            //IfStatement -> if ConditionStatement then Statements ElseIfStatements ElseStatement end

            Node ifStatement = new Node("IfStatement");
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(ConditionStatement());
            ifStatement.Children.Add(match(Token_Class.Then));
            ifStatement.Children.Add(Statements());

            Node elseIfStatements = new Node("ElseIfStatements");
            ifStatement.Children.Add(ElseIfStatements(elseIfStatements));
            ifStatement.Children.Add(ElseStatement());

            ifStatement.Children.Add(match(Token_Class.End));


            return ifStatement;
        }

        
        Node ConditionStatement()
        {
            //ConditionStatement -> Condition CondStmts

            Node conditionStatement = new Node("ConditionStatement");
            Node condStmts = new Node("CondStmts");
            conditionStatement.Children.Add(Condition());
            conditionStatement.Children.Add(CondStmts(condStmts));

            return conditionStatement;
        }
        //CondStmts -> ε | boolean_operator Condition CondStmts
        Node CondStmts(Node condStmts)
        {
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.AndOp ||
                    TokenStream[InputPointer].token_type == Token_Class.OrOp))
            {
                condStmts.Children.Add(match(TokenStream[InputPointer].token_type));
                condStmts.Children.Add(Condition());
                CondStmts(condStmts);
            }

            return condStmts;
        }

       
        Node Condition()
        {
            // Condition -> identifier condition_operator Term

            Node condition = new Node("Condition");
            
            condition.Children.Add(match(Token_Class.Identifier));
            
            if (isConditionOperator())
            {
                condition.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            else
            {
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected a condition_operator  and " +
                    TokenStream[InputPointer].token_type.ToString() +
                    "  found. \r\n");

                    while (!( isTokenValid(Token_Class.Return) || isTokenValid(Token_Class.End) ))
                    {
                        InputPointer++;
                    }
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected a condition_operator but nothing was found.\r\n");
                }

            }
            condition.Children.Add(Term());
            return condition;
        }
         
        Node ElseIfStatements(Node elseIfStatements)
        {
            // ElseIfStatements -> elseif ConditionStatement then Statements ElseIfStatements | ε

            if (isTokenValid(Token_Class.ElseIf))
            {
                elseIfStatements.Children.Add(match(Token_Class.ElseIf));
                elseIfStatements.Children.Add(ConditionStatement());
                elseIfStatements.Children.Add(match(Token_Class.Then));
                elseIfStatements.Children.Add(Statements());
                ElseIfStatements(elseIfStatements);
            }

            return elseIfStatements;
        }
        Node ElseStatement()
        {
            Node elseStatement = new Node("ElseStatement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                elseStatement.Children.Add(match(Token_Class.Else));
                elseStatement.Children.Add(Statements());
            }

            return elseStatement;

        }

        Node RepeatStatement()
        { 
            //repeat statements until condition
            Node repeatStatement = new Node("RepeatStatement");

            repeatStatement.Children.Add(match(Token_Class.Repeat));
            repeatStatement.Children.Add(Statements());
            repeatStatement.Children.Add(match(Token_Class.Until));
            repeatStatement.Children.Add(ConditionStatement());

            return repeatStatement;
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
                    //InputPointer++;
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