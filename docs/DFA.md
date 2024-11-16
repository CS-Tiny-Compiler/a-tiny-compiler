# Tokens Regex + Deterministic Finite Automata

### Number

- Regex : `(\+|-)?[0-9]+(\.[0-9]+)?`
- Digit : `[0-9]`
- Other : `~[0-9]`
- Any : `Other|Digit`

```mermaid
  flowchart LR
    start(((Start))):::startNode --> q0(("q0")):::normal

    q0 --> |"#43;,#45;"| q1(("q1")):::normal
    q0 --> |Digit| q2((("q2"))):::finalState
    q0 --> |"~[#43;|#45;|Digit]"| trap(("Trap")):::trap
    q1 --> |Digit| q2
    q1 --> |Other| trap
    q2 --> |"."| q3(("q3")):::normal
    q2 --> |"~[.|Digit]"| trap
    q2 --> |Digit| q2
    q3 --> |Digit| q4((("q4"))):::finalState
    q3 --> |Other| trap
    q4 --> |Digit| q4
    q4 --> |Other| trap

    trap --> |Any| trap

    classDef startNode fill:none,stroke:none;
    classDef normal stroke:#000,stroke-width:2px;
    classDef finalState stroke:#000,stroke-width:3px;
    classDef trap stroke:#000,stroke-width:2px;

    linkStyle default stroke:#333,stroke-width:2px,fill:none;

```

### Identifier
- Regex: `[A-Z|a-z][A-Z|a-z|0-9]*`
- Other: `~[A-Z|a-z]`
- Any: `Other|[A-Z|a-z]`

```mermaid
flowchart LR
    start(((Start))):::startNode --> q0(("q0")):::normal
    q0 --> |"[a-z]"| q1(("q1")):::normal
    q0 --> |"[A-Z]"| q1(("q1")):::normal
    q0 --> |Other| Trap(("Trap")):::trap

    q1 --> |"[a-z|A-Z|0-9]"| q1((("q1"))):::finalState
    q1 --> |"~[A-Z|a-z|0-9]"| Trap((("Trap"))):::trap

    Trap --> |Any| Trap(("Trap")):::trap

    classDef startNode fill:none,stroke:none;
    classDef normal stroke:#000,stroke-width:2px;
    classDef finalState stroke:#000,stroke-width:3px;
    classDef trap stroke:#000,stroke-width:2px;

    linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

<div style="page-break-after: always; visibility: hidden"> 
\pagebreak 
</div>

### String

- Regex: `"(~")*"`
- Other: `(~")`
- Any: `Other|"`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal

  q0 --> |``| q1(("q1")):::normal
  q0 --> |"Other"| q3(("Trap")):::trapState
  q1 --> |"Other"| q1
  q1 --> |``| q2((("q2"))):::finalState
  q2 --> |"Any"| q3
  q3 --> |"Any"| q3

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;
  classDef trapState stroke:#000,stroke-width:2px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

### Reserved_Keywords

- Regex: `int | float | string | read | write | repeat | until | if | elseif | else | then | return | endl | end | main`

<div style="page-break-after: always; visibility: hidden"> 
\pagebreak 
</div>

```mermaid
  flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal

  q0 --> |"i"| q1(("q1")):::normal
  q1 --> |"n"| q2(("q2")):::normal
  q2 --> |"t"| q3((("q3"))):::finalState

  q0 --> |"f"| q4(("q4")):::normal
  q4 --> |"l"| q5(("q5")):::normal
  q5 --> |"o"| q6(("q6")):::normal
  q6 --> |"a"| q7(("q7")):::normal
  q7 --> |"t"| q8((("q8"))):::finalState

  q0 --> |"s"| q9(("q9")):::normal
  q9 --> |"t"| q10(("q10")):::normal
  q10 --> |"r"| q11(("q11")):::normal
  q11 --> |"i"| q12(("q12")):::normal
  q12 --> |"n"| q13(("q13")):::normal
  q13 --> |"g"| q14((("q14"))):::finalState

  q0 --> |"r"| q15(("q15")):::normal
  q15 --> |"e"| q16(("q16")):::normal
  q16 --> |"a"| q17(("q17")):::normal
  q17 --> |"d"| q18((("q18"))):::finalState

  q0 --> |"w"| q19(("q19")):::normal
  q19 --> |"r"| q20(("q20")):::normal
  q20 --> |"i"| q21(("q21")):::normal
  q21 --> |"t"| q22(("q22")):::normal
  q22 --> |"e"| q23((("q23"))):::finalState

  q16 --> |"p"| q24(("q24")):::normal
  q24 --> |"e"| q25(("q25")):::normal
  q25 --> |"a"| q26(("q26")):::normal
  q26 --> |"t"| q27((("q27"))):::finalState

  q16 --> |"t"| q28(("q28")):::normal
  q28 --> |"u"| q29(("q29")):::normal
  q29 --> |"r"| q30(("q30")):::normal
  q30 --> |"n"| q31((("q31"))):::finalState

  q0 --> |"u"| q32(("q32")):::normal
  q32 --> |"n"| q33(("q33")):::normal
  q33 --> |"t"| q34(("q34")):::normal
  q34 --> |"i"| q35(("q35")):::normal
  q35 --> |"l"| q36((("q36"))):::finalState

  q1 --> |"f"| q37((("q37"))):::finalState

  q0 --> |"t"| q38(("q38")):::normal
  q38 --> |"h"| q39(("q39")):::normal
  q39 --> |"e"| q40(("q40")):::normal
  q40 --> |"n"| q41((("q41"))):::finalState

  q0 --> |"e"| q42(("q42")):::normal
  q42 --> |"n"| q43(("q43")):::normal
  q43 --> |"d"| q44((("q44"))):::finalState
  q44 --> |"l"| q45((("q45"))):::finalState

  q42 --> |"l"| q46(("q46")):::normal
  q46 --> |"s"| q47(("q47")):::normal
  q47 --> |"e"| q48((("q48"))):::finalState
  q48 --> |"i"| q49(("q49")):::normal
  q49 --> |"f"| q50((("q50"))):::finalState

  q0 --> |"m"| q51(("q51")):::normal
  q51 --> |"a"| q52(("q52")):::normal
  q52 --> |"i"| q53(("q53")):::normal
  q53 --> |"n"| q54((("q54"))):::finalState

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

### Arithmetic_Operators

- Regex: `\+|\-|\*|/`
- Any: `-|~-"`

<!-- I am using these HTML character codes (e.g. #43;) since directly using '+' or '-' or '*' gives syntax errors with the mermaid syntax -->

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |#43;| q1((("q1"))):::finalState
  q0 --> |#45;| q2((("q2"))):::finalState
  q0 --> |#42;| q3((("q3"))):::finalState
  q0 --> |#47;| q4((("q4"))):::finalState
  q0 --> |"~[#43;|#45;|#42;|#47;]"| trap(("Trap")):::trap

  q1 --> |"Any"| trap(("Trap")):::trap
  q2 --> |"Any"| trap(("Trap")):::trap
  q3 --> |"Any"| trap(("Trap")):::trap
  q4 --> |"Any"| trap(("Trap")):::trap

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;
  classDef trap stroke:#000,stroke-width:2px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

### Assignment_Operator

- Regex: `:=`
- Any: `=|~=`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |:| q1(("q1")):::normal
  q1 --> |=| q2((("q2"))):::finalState
  q0 --> |"~:"| trap(("Trap")):::trap
  q1 --> |"~="| trap(("Trap")):::trap
  q2 --> |"Any"| trap(("Trap")):::trap

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;
  classDef trap stroke:#000,stroke-width:2px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

<div style="page-break-after: always; visibility: hidden"> 
\pagebreak 
</div>

### Delimiters

- Regex: `{|}|;|,|(|)`
- Other: `~({|}|;|,|(|))`
- Any: `(|~(`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |"{"| q1((("q1"))):::finalState
  q0 --> |"}"| q2((("q2"))):::finalState
  q0 --> |";"| q3((("q3"))):::finalState
  q0 --> |","| q5((("q5"))):::finalState
  q0 --> |"("| q6((("q6"))):::finalState
  q0 --> |")"| q7((("q7"))):::finalState
  q0 --> |Other| trap(("Trap")):::trap
  q1 --> |Any| trap(("Trap")):::trap
  q2 --> |Any| trap(("Trap")):::trap
  q3 --> |Any| trap(("Trap")):::trap
  q5 --> |Any| trap(("Trap")):::trap
  q6 --> |Any| trap(("Trap")):::trap
  q7 --> |Any| trap(("Trap")):::trap

  trap --> |Any| trap

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;
  classDef trap stroke:#000,stroke-width:2px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

<div style="page-break-after: always; visibility: hidden"> 
\pagebreak 
</div>

### Condition_Operators

- Regex: `< | > | = | <>`
- Any: `=|~=`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |#60;| q1((("q1"))):::finalState
  q0 --> |#62;| q2((("q2"))):::finalState
  q0 --> |#61;| q3((("q3"))):::finalState
  q1 --> |#62;| q4((("q4"))):::finalState
  q0 --> |"~[#60;|#62;|#61;]"| trap(("Trap")):::trap
  q1 --> |"~#62;"| trap(("Trap")):::trap
  q2 --> |"Any"| trap(("Trap")):::trap
  q3 --> |"Any"| trap(("Trap")):::trap
  q4 --> |"Any"| trap(("Trap")):::trap


  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;
  classDef trap stroke:#000,stroke-width:2px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```

### Boolean_Operators

- Regex: `&& | \|\|`
- Any: `&|~&`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |#38;| q1(("q1")):::normal
  q1 --> |#38;| q3((("q3"))):::finalState
  q0 --> |#124;| q2(("q2")):::normal
  q2 --> |#124;| q4((("q4"))):::finalState
  q0 --> |"~[#38;,#124;]"| trap(("Trap")):::trap
  q1 --> |"~#38;"| trap(("Trap")):::trap
  q2 --> |"~#124;"| trap(("Trap")):::trap
  q3 --> |"Any"| trap(("Trap")):::trap
  q4 --> |"Any"| trap(("Trap")):::trap


  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;
  classDef trap stroke:#000,stroke-width:2px;

  linkStyle default stroke:#333,stroke-width:2px,fill:none;
```
