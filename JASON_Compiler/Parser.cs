using System;
using System.Collections.Generic;
using System.Linq;
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
            Node program = new Node("Program");
            program.Children.Add(match(Token_Class.Else));
            program.Children.Add(match(Token_Class.Identifier));
            program.Children.Add(match(Token_Class.Endl));
            return program;
        }

        //Node Program()
        //{
        //    Node program = new Node("Program");
        //    program.Children.Add(Functions());
        //    program.Children.Add(Main());

        //    return program;
        //}
        //Node Functions()
        //{
        //    Node functions = new Node("Functions");

        //    //FunctionStatement Functions | ε

        //    if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
        //    {
        //        functions.Children.Add(FunctionStatement());
        //        Functions();
        //    }

        //    return functions;
        //}

        //Node FunctionStatement()
        //{
        //    Node functionStatement = new Node("FunctionStatement");

        //    //FunctionStatement -> FunctionDeclaration FunctionBody
        //    functionStatement.Children.Add(FunctionDeclaration());
        //    functionStatement.Children.Add(FunctionBody());

        //    return functionStatement;
        //}

        //Node FunctionDeclaration()
        //{
        //    Node functionDeclaration = new Node("FunctionDeclaration");

        //    // DataType identifier ( ParametersList )
        //    functionDeclaration.Children.Add(DataType());
        //    functionDeclaration.Children.Add(match(Token_Class.Identifier));
        //    functionDeclaration.Children.Add(match(Token_Class.LRoundParanthesis));
        //    functionDeclaration.Children.Add(ParametersList());
        //    functionDeclaration.Children.Add(match(Token_Class.RRoundParanthesis));

        //    return functionDeclaration;
        //}
        //Node ParametersList()
        //{
        //    Node parametersList = new Node("ParametersList");

        //    //Parameters | ε
        //    if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
        //    {
        //        parametersList.Children.Add(Parameters());
        //    }

        //    return parametersList;
        //}

        //Node Statement() //Review
        //{
        //    Node statement = new Node("Statement");
        //    //Statement -> ReadStatement | WriteStatement | AssignmentStatement ;
        //    //| DeclarationStatement | IfStatement | RepeatStatement | FunctionCall ; | ε

        //    if (isTokenValid(Token_Class.Read))
        //    {
        //        statement.Children.Add(ReadStatement());
        //    }
        //    else if (isTokenValid(Token_Class.Write))
        //    {
        //        statement.Children.Add(WriteStatement());
        //    }
        //    else if (isTokenValid(Token_Class.Repeat))
        //    {
        //        statement.Children.Add(RepeatStatement());
        //    }
        //    else if (isTokenValid(Token_Class.Identifier))
        //    {
        //        InputPointer++;
        //        if (isTokenValid(Token_Class.AssignOp))
        //        {
        //            InputPointer--;
        //            statement.Children.Add(AssignmentStatement());
        //        }
        //        else
        //        {
        //            InputPointer--;
        //            statement.Children.Add(FunctionCall());
        //        }

        //        statement.Children.Add(match(Token_Class.Semicolon));
        //    }
        //    else if (isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
        //    {
        //        statement.Children.Add(DeclarationStatement());
        //    }
        //    else if (isTokenValid(Token_Class.If))
        //    {
        //        statement.Children.Add(IfStatement());
        //    }

        //    return statement;
        //}
        //Node ReadStatement()
        //{
        //    Node readStatement = new Node("ReadStatement");
        //    // ReadStatement -> read identifier ;

        //    readStatement.Children.Add(match(Token_Class.Read));
        //    readStatement.Children.Add(match(Token_Class.Identifier));
        //    readStatement.Children.Add(match(Token_Class.Semicolon));

        //    return readStatement;
        //}
        //Node WriteStatement()
        //{
        //    Node writeStatement = new Node("WriteStatement");
        //    // WriteStatement -> write WriteContent ;
        //    //  WriteContent-> Expression | endl

        //    writeStatement.Children.Add(WriteContent());
        //    writeStatement.Children.Add(match(Token_Class.Write));
        //    writeStatement.Children.Add(match(Token_Class.Semicolon));

        //    return writeStatement;
        //}

        //Node WriteContent()
        //{
        //    //  WriteContent-> Expression | endl
        //    Node writeContent = new Node("WriteContent");

        //    if (isTokenValid(Token_Class.Endl))
        //    {
        //        writeContent.Children.Add(match(Token_Class.Endl));
        //    }
        //    else
        //    {
        //        writeContent.Children.Add(Expression());
        //    }


        //    return writeContent;
        //}

        //Node Term() //Review
        //{
        //    // Term -> number | identifier | FunctionCall
        //    Node term = new Node("Term");

        //    if (isTokenValid(Token_Class.Constant))
        //    {
        //        term.Children.Add(match(Token_Class.Constant));
        //    }
        //    else if (isTokenValid(Token_Class.Identifier))
        //    {
        //        InputPointer++;
        //        if (isTokenValid(Token_Class.LRoundParanthesis))
        //        {
        //            InputPointer--;
        //            term.Children.Add(FunctionCall());
        //        }
        //        else
        //        {
        //            InputPointer--;
        //            term.Children.Add(match(Token_Class.Identifier));
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return term;
        //}

        //Node ArithmeticTerms()
        //{
        //    Node arithmeticTerms = new Node("ArithmeticTerms");

        //    // ArithmeticTerms->arithmetic_operator ArithmeticTermsTail
        //    //ArithmeticTermsTail->Equation | Term

        //    // arithmeticTerms.Children.Add();
        //    arithmeticTerms.Children.Add(ArithmeticTermsTail());

        //    return arithmeticTerms;
        //}

        //Node ArithmeticTermsTail()
        //{
        //    Node arithmeticTermsTail = new Node("ArithmeticTermsTail");

        //    //ArithmeticTermsTail->Equation | Term

        //    arithmeticTermsTail.Children.Add(Equation());
        //    arithmeticTermsTail.Children.Add(Term());

        //    //term -> number
        //    // equation -> term
        //    return arithmeticTermsTail;
        //}

        ///////////////////////////// Implement your logic here

        //Node Parameters()
        //{
        //    Node parameters = new Node("Parameters");
        //    Node _params = new Node("Params");

        //    //Parameters -> Parameter Params
        //    parameters.Children.Add(Parameter());
        //    parameters.Children.Add(Params(_params));
        //    return parameters;
        //}
        //Node Params(Node _params)
        //{
        //    //Params -> ε | , Parameter Params

        //    if (isTokenValid(Token_Class.Comma))
        //    {
        //        _params.Children.Add(match(Token_Class.Comma));
        //        _params.Children.Add(Parameter());
        //        _params.Children.Add(Params(_params));

        //    }

        //    return _params;
        //}

        //Node Parameter()
        //{
        //    Node Parameter = new Node("Parameter");

        //    //Parameter -> DataType identifier
        //    Parameter.Children.Add(DataType());
        //    Parameter.Children.Add(match(Token_Class.Identifier));
        //    return Parameter;
        //}

        //Node Main()
        //{
        //    Node main = new Node("Main");

        //    //Main -> DataType main ( ) FunctionBody
        //    main.Children.Add(DataType());
        //    main.Children.Add(match(Token_Class.Main));
        //    main.Children.Add(match(Token_Class.LRoundParanthesis));
        //    main.Children.Add(match(Token_Class.RRoundParanthesis));
        //    main.Children.Add(FunctionBody());

        //    return main;
        //}
        //Node FunctionBody()
        //{
        //    Node functionBody = new Node("FunctionBody");

        //    //FunctionBody -> { Statements ReturnStatement }
        //    functionBody.Children.Add(match(Token_Class.LCurlyParanthesis));
        //    functionBody.Children.Add(Statements());
        //    functionBody.Children.Add(ReadStatement());
        //    functionBody.Children.Add(match(Token_Class.RCurlyParanthesis));

        //    return functionBody;
        //}

        //Node Statements()
        //{
        //    Node statements = new Node("statements");
        //    Node stmts = new Node("Stmts");
        //    //Statements -> Statement Stmts
        //    //Stmts-> ε | Statement Stmts

        //    statements.Children.Add(Statement());
        //    statements.Children.Add(Stmts(stmts));

        //    return statements;
        //}
        //Node Stmts(Node stmts)
        //{

        //    //Stmts-> ε | Statement Stmts

        //    if (isTokenValid(Token_Class.Read) || isTokenValid(Token_Class.Write) || isTokenValid(Token_Class.Identifier)
        //        || isTokenValid(Token_Class.Repeat) || isTokenValid(Token_Class.If)
        //        || isTokenValid(Token_Class.Int) || isTokenValid(Token_Class.Float) || isTokenValid(Token_Class.String))
        //    {
        //        stmts.Children.Add(Statement());
        //        stmts.Children.Add(Stmts(stmts));
        //    }

        //    return stmts;
        //}

        //Node FunctionCall()
        //{
        //    Node functionCall = new Node("FunctionCall");
        //    //FunctionCall -> identifier ( ArgList )

        //    functionCall.Children.Add(match(Token_Class.Identifier));
        //    functionCall.Children.Add(match(Token_Class.LRoundParanthesis));
        //    functionCall.Children.Add(ArgList());
        //    functionCall.Children.Add(match(Token_Class.RRoundParanthesis));

        //    return functionCall;
        //}

        //Node ArgList()
        //{
        //    Node argList = new Node("ArgList");

        //    //ArgList -> Arguments | ε

        //    if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
        //    {
        //        argList.Children.Add(Arguments());
        //    }

        //    return argList;
        //}

        //Node Arguments()
        //{
        //    Node arguments = new Node("Arguments");
        //    Node args = new Node("Args");

        //    // Arguments -> Term Args

        //    arguments.Children.Add(match(Token_Class.Identifier));
        //    arguments.Children.Add(Args(args));

        //    return arguments;
        //}
        //Node Args(Node args)
        //{
        //    // Args->ε | , Term Args
        //    if (isTokenValid(Token_Class.Comma))
        //    {
        //        args.Children.Add(match(Token_Class.Comma));
        //        args.Children.Add(Term());
        //        args.Children.Add(Args(args));
        //    }
        //    return args;
        //}

        //Node Expression()
        //{
        //    Node expression = new Node("Expression");

        //    // Expression -> string | Term | Equation
        //    if (isTokenValid(Token_Class.String))
        //    {
        //        expression.Children.Add(match(Token_Class.String));
        //    }
        //    else if (isTokenValid(Token_Class.Constant) || isTokenValid(Token_Class.Identifier))
        //    {
        //        expression.Children.Add(Term());
        //    }
        //    else if () //equation
        //    {
        //        expression.Children.Add(Equation());
        //    }

        //    return expression;
        //}

        //Node DataType()
        //{
        //    Node dataType = new Node("DataType");

        //    if (isTokenValid(Token_Class.Int))
        //    {
        //        dataType.Children.Add(match(Token_Class.Int));
        //    }
        //    else if (isTokenValid(Token_Class.Float))
        //    {
        //        dataType.Children.Add(match(Token_Class.Float));
        //    }
        //    else if (isTokenValid(Token_Class.String))
        //    {
        //        dataType.Children.Add(match(Token_Class.String));
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return dataType;
        //}

        public bool isTokenValid(Token_Class tokenType)
        {
            return (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == tokenType);
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
