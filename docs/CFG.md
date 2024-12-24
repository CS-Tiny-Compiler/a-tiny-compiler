# CFG

## Terminals

### 1. identifier

### 2. (

### 3. )

### 4. int

### 5. float

### 6. string

### 7. {

### 8. }

### 9. main

### 10. read

### 11. write

### 12. number

### 13. endl

### 14. string

### 15. arithmetic_operator

### 16. assignment_operator

### 17. return

### 18. condition_operator

### 19. if

### 20. then

### 21. end

### 22. boolean_operator

### 23. elseif

### 24. else

### 25. repeat

### 26. until

### 27. ;

### 28. ,

## Production Rules

**Note:** **Non-terminals** start with a **capital** letter and **terminals** start with a **small** letter.

A program in TINY consists of a set of functions (any number of functions (**zero or more**) and ends with a main function), each function is a sequence of statements including (declaration, assignment, write, read, if, repeat, function, comment, etc).

**Important Note**

When you find
```
rules that look like this
```
This is the one you work with since these are the correct ones after removing left recursion and left factors.

### 1. Program -> Functions Main

### 2. Functions -> FunctionStatement Functions | ε

### 3. FunctionStatement -> FunctionDeclaration FunctionBody

### 4. FunctionDeclaration -> DataType identifier ( ParametersList )

### 5. DataType -> int | float | string

### 6. ParametersList -> Parameters | ε

### 7. Parameters -> Parameters, Parameter | Parameter
```
Parameters -> Parameter Params
Params -> ε | , Parameter Params
```

### 8. Parameter -> DataType identifier

### 9. Main -> DataType main ( ) FunctionBody

### 10. FunctionBody -> { Statements ReturnStatement }

### 11. Statements -> Statements Statement | Statement
```
Statements -> Statement Stmts
Stmts -> ε | Statement Stmts
```

### 12. Statement -> ReadStatement | WriteStatement | AssignmentStatement ; | DeclarationStatement | IfStatement | RepeatStatement | FunctionCall ; | ε

### 13. ReadStatement -> read identifier ;

### 14. WriteStatement -> write Expression ; | write endl ;
```
WriteStatement -> write WriteContent ;
WriteContent -> Expression | endl
```

### 15. Term -> number | identifier | FunctionCall

### 16. FunctionCall -> identifier ( ArgList )

### 17. ArgList -> Arguments | ε

### 18. Arguments -> Arguments, Term | Term
```
Arguments -> Term Args
Args -> ε | , Term Args
```

### 19. Expression -> string | Term | Equation

### 20. Equation -> Term ArithmeticTerms | ( Equation ) | ( Equation ) ArithmeticTerms
```
Equation -> Term ArithmeticTerms | ( Equation ) EquationTail
EquationTail -> ε | ArithmeticTerms
```

### 21. ArithmeticTerms -> arithmetic_operator Equation | arithmetic_operator Term
```
ArithmeticTerms -> arithmetic_operator ArithmeticTermsTail
ArithmeticTermsTail -> Equation | Term
```

### 22. AssignmentStatement -> identifier assignment_operator Expression

### 23. DeclarationStatement -> DataType Declarations ;

### 24. Declarations -> Declarations, identifier | Declarations, AssignmentStatement | identifier | AssignmentStatement
```
Declarations -> identifier Decls | AssignmentStatement Decls
Decls -> ε | , DeclsTail Decls
DeclsTail -> identifier | AssignmentStatement
```

### 25. ReturnStatement -> return Expression ;

### 26. IfStatement -> if ConditionStatement then Statements ElseIfStatements ElseStatement end

### 27. ConditionStatement -> ConditionStatement boolean_operator Condition | Condition
```
ConditionStatement -> Condition CondStmts
CondStmts -> ε | boolean_operator Condition CondStmts
```

### 28. Condition -> identifier condition_operator Term

### 29. ElseIfStatements -> elseif ConditionStatement then Statements ElseIfStatements | ε

### 30. ElseStatement -> else Statements | ε

### 31. RepeatStatement -> repeat Statements until ConditionStatement

## Questions to ask
- Are our rules correct? (specifically equation & arithmetic terms)