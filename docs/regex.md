# Regular Expressions

<!--
1.Added rounded brackets in regex of deliminters.
2.Removed dot operator in delimiter regex
3.Update Arithmatic operator
  -> '-' has a meta meaning (range) -
  ->no need for / for divide operator, it doesn't have a meta meaning.
 -->
- Number -> `(\+|\-)?[0-9]+(\.[0-9]+)?`
- String -> ` "(~")*"`
- Reserved_Keywords -> `int | float | string | read | write | repeat | until | if | elseif | else | then | return | endl | end | main`
- Arithmetic_Operators -> `\+|\-|\*|/`
- Assignment_Operator -> `:=`
- Delimiters -> `{|}|;|,|(|)`
- Condition_Operators -> `<|>|=|(<>)|(>=)|(<=)`
- Boolean_Operators -> `(&&)|(\|\|)`
- Identifiers -> `[A-Z|a-z][A-Z|a-z|0-9]*`

