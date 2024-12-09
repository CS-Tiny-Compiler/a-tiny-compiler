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

### 27. /

### 28. *

### 29. delimiters

### 30. :

### 31. "

### 32. '

### 33. &

### 34. |

### 35. .

### 36. –

### 37. [a-z]

### 38. [A-Z]

## Production Rules

**Note:** **Non-terminals** start with a **capital** letter and **terminals** start with a **small** letter.

A program in TINY consists of a set of functions (any number of functions (**zero or more**) and ends with a main function), each function is a sequence of statements including (declaration, assignment, write, read, if, repeat, function, comment, etc).

### 1. Program -> Functions Main

Note: Check difference when switching Functions and FunctionStatement + Check if we need to add `| FunctionStatement` (I don't think we do but they for some reason have it added in the labs in JASON).

### 2. Functions -> Functions FunctionStatement | ε
```
Functions -> ε Funcs
Funcs -> ε | FunctionStatement Funcs
```

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

### 12. Statement -> ReadStatement | WriteStatement | AssignmentStatement ; | DeclarationStatement | IfStatement | RepeatStatement | CommentStatement | FunctionCall ; | ε

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

Note: Equation is tricky

### 20. Equation -> Term ArithmeticTerms | ( Equation ) | ( Equation ) ArithmeticTerms
```
Equation -> Term ArithmeticTerms | ( Equation ) EquationTail
EquationTail -> ε | ArithmeticTerms
```

Note: Also here can remove left recursion to be right recursion easily and it would be correct.

### 21. ArithmeticTerms -> ArithmeticTerms arithmetic_operator Term | ArithmeticTerms arithmetic_operator ( Equation ) | arithmetic_operator Term | arithmetic_operator ( Equation )
```
ArithmeticTerms -> arithmetic_operator ArithFactors ArithTerms
ArithFactors -> Term | ( Equation )
ArithTerms -> ε | arithmetic_operator ArithFactors ArithTerms
```

### 22. AssignmentStatement -> identifier assignment_operator Expression

### 23. DeclarationStatement -> DataType Declarations ;

Note: Here also I think we can switch "Declarations, identifier" and "Declarations, AssignmentStatement" to remove left recursion directly, but not sure if we can do that.

### 24. Declarations -> Declarations, identifier | Declarations, AssignmentStatement | identifier | AssignmentStatement
```
Declarations -> identifier Decls | AssignmentStatement Decls
Decls -> ε | , identifier Decls | , AssignmentStatement Decls
```

### 25. ReturnStatement -> return Expression ;

### 26. IfStatement -> if ConditionStatement then Statements ElseIfStatements ElseStatement end

### 27. ConditionStatement -> ConditionStatement boolean_operator Condition | Condition
```
ConditionStatement -> Condition CondStmts
CondStmts -> ε | boolean_operator Condition CondStmts
```

### 28. Condition -> identifier condition_operator Term

### 29. ElseIfStatements -> ElseIfStatements elseif ConditionStatement then Statements | ε
```
ElseIfStatements -> elseif ConditionStatement then Statements ElseIfStmts | ε
ElseIfStmts -> ε | elseif ConditionStatement then Statements ElseIfStmts
```

### 30. ElseStatement -> else Statements | ε

### 31. RepeatStatement -> repeat Statements until ConditionStatement

### 32. CommentStatement -> / * CommentContent * /

### 33. CommentContent -> CommentContent Character | ε
```
CommentContent -> Character CommentCont | ε
CommentCont -> Character CommentCont | ε
```

### 34. Character → Letter | number | Symbol

### 35. Letter → [a-z] | [A-Z]

### 36. Symbol → + | - | * | / | ( | ) | { | } | [ | ] | , | ; | : | " | ' | < | > | & | "|" | . | –

## Questions to ask
- Is it allowed to apply right recursion directly if it is correct?
- Is it ok to use something like 'condition_operator' as a terminal? or do we have to create a non-terminal ConditionOperators = > | < | >= | <= ?
- Are our rules correct? (specifically equation & arithmetic terms)
- Is our document structure correct (Terminals and Non-terminals section) ?
- Should we create a rule for comments?
- Is the letter rule correct?